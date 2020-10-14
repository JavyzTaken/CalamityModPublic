using CalamityMod.Tiles.DraedonStructures;
using CalamityMod.TileEntities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityMod.Schematics.SchematicLoader;

namespace CalamityMod.Schematics
{
    public static class SchematicPlacementHelpers
    {
        public enum PlacementAnchorType
        {
            TopLeft,
            TopRight,
            Center,
            BottomLeft,
            BottomRight,
        }

        internal static Tile SchematicTileConversion(Tile oldTile, Tile toReplaceWith, Color schematicColorAtPosition)
        {
            float opacity = schematicColorAtPosition.A / 255f;

            Tile newTile = new Tile();
            // Destroy the original tile completely.
            if (opacity == 0f)
                return newTile;

            // Destroy the original tile itself but preserve the wall.
            if (oldTile.wall != 0 && toReplaceWith.wall == 0 && opacity > 0f && opacity <= 0.33f)
                newTile.wall = oldTile.wall;

            // Do nothing to the tile.
            if (opacity > 0.33f && opacity <= 0.66f)
                newTile = (Tile)oldTile.Clone();

            // Completely replace the tile.
            if (opacity > 0.66f)
                newTile = (Tile)toReplaceWith.Clone();
            return newTile;
        }

        public static void PlaceStructure<T>(string mapKey, Point placementPosition, PlacementAnchorType placementAnchor, ref bool specialCondition, T chestInteraction = null) where T : Delegate
        {
            if (chestInteraction != null &&
                !(chestInteraction is Action<Chest>) && 
                !(chestInteraction is Action<Chest, int, bool>))
            {
                throw new ArgumentException("The chest interaction function has invalid parameters.", nameof(chestInteraction));
            }

            LoadEverything(); // Just in case they weren't loaded properly beforehand.

            // If no structure schematic matching that name was found, cancel.
            if (!TileMaps.ContainsKey(mapKey))
                return;
            PilePlacementMaps.TryGetValue(mapKey, out PilePlacementFunction pilePlacementFunction);
            ColorTileCombination[,] schematic = TileMaps[mapKey];

            // Make an array for the tiles that used to be where this schematic will be pasted.
            int sWidth = schematic.GetLength(0);
            int sHeight = schematic.GetLength(1);
            Tile[,] oldTiles = new Tile[sWidth, sHeight];

            int xOffset = placementPosition.X;
            int yOffset = placementPosition.Y;

            // TopLeft is the default because anchoring things at their top-left corner is the default Terraria behavior.
            // This is why it does nothing.

            switch (placementAnchor)
            {
                case PlacementAnchorType.TopRight:
                    xOffset += sWidth;
                    break;
                case PlacementAnchorType.Center:
                    xOffset += sWidth / 2;
                    yOffset += sHeight / 2;
                    break;
                case PlacementAnchorType.BottomLeft:
                    yOffset += sHeight;
                    break;
                case PlacementAnchorType.BottomRight:
                    xOffset += sWidth;
                    yOffset += sHeight;
                    break;
                case PlacementAnchorType.TopLeft:
                default:
                    break;
            }
            
            // Fill the old tiles array while simultaneously destroying everything in the target rectangle.
            for (int x = 0; x < sWidth; x++)
            {
                for (int y = 0; y < sHeight; y++)
                {
                    oldTiles[x, y] = (Tile)Main.tile[x + xOffset, y + yOffset].Clone();

                    // Attempting to break chests causes the game to attempt to infinitely recurse in an attempt to break the tile, resulting in a stack overflow.
                    if (oldTiles[x, y].type == TileID.Containers)
                        continue;
                    WorldGen.KillTile(x + xOffset, y + yOffset);
                }
            }

            // Lay down the schematic. If the schematic calls for it, bring back tiles that are stored in the old tiles array.
            for (int x = 0; x < sWidth; x++)
            {
                for (int y = 0; y < sHeight; y++)
                {
                    if (!WorldGen.InWorld(x + xOffset, y + yOffset))
                        continue;

                    Tile tile = schematic[x, y].InternalTile;
                    ModTile modTile = TileLoader.GetTile(tile.type);
                    bool isChest = tile.type == TileID.Containers || (modTile != null && modTile.chest != "");

                    // If the determined tile type is a chest, define it appropriately.
                    if (isChest)
                    {
                        if (tile.frameX % 36 == 0 && tile.frameY == 0)
                        {
                            Chest chest = PlaceChest(x + xOffset, y + yOffset, tile.type);

                            if (chestInteraction is Action<Chest, int, bool>)
                            {
                                (chestInteraction as Action<Chest, int, bool>)?.Invoke(chest, tile.type, specialCondition);
                                specialCondition = true;
                            }
                            else if (chestInteraction is Action<Chest>)
                                (chestInteraction as Action<Chest>)?.Invoke(chest);
                        }
                    }

                    // Trees and cacti get special treatment and are always imported back in to prevent world corruption.
                    if (tile.type == TileID.Trees || tile.type == TileID.PineTree || tile.type == TileID.Cactus)
                    {
                        ushort oldWall = oldTiles[x, y].wall;
                        oldTiles[x, y] = new Tile
                        {
                            wall = oldWall
                        };
                    }
                    else
                    {
                        Main.tile[x + xOffset, y + yOffset] = (Tile)SchematicTileConversion(oldTiles[x, y], tile, schematic[x, y].InternalColor).Clone();
                        TryToPlaceTileEntities(x + xOffset, y + yOffset);
                    }

                    Rectangle placeInArea = new Rectangle(x, y, sWidth, sHeight);
                    pilePlacementFunction?.Invoke(x + xOffset, y + yOffset, placeInArea);
                }
            }
        }

        private static void TryToPlaceTileEntities(int x, int y)
        {
            Tile t = CalamityUtils.ParanoidTileRetrieval(x, y);
            int tileType = t.type;

            // A tile entity in an empty spot would make no sense.
            if (!t.active())
                return;

            // Ignore tiles that aren't at the top left of the tile.
            // All of Calamity's worldgen-placed tile entities refuse to exist except at the top left corner of their host tile.
            if (t.frameX != 0 || t.frameY != 0)
                return;

            // This cannot be a switch because switch cases must be compile time constants, which ModContent calls are not.
            // Therefore the only option is an if-else ladder.
            else if (tileType == ModContent.TileType<ChargingStation>())
                TileEntity.PlaceEntityNet(x, y, ModContent.TileEntityType<TEChargingStation>());
            else if (tileType == ModContent.TileType<DraedonLabTurret>())
                TileEntity.PlaceEntityNet(x, y, ModContent.TileEntityType<TEDraedonLabTurret>());
            else if (tileType == ModContent.TileType<LabHologramProjector>())
                TileEntity.PlaceEntityNet(x, y, ModContent.TileEntityType<TELabHologramProjector>());
        }

        public static Chest PlaceChest(int x, int y, int chestType)
        {
            int chestIndex = Chest.FindEmptyChest(x, y, chestType);
            Main.chest[chestIndex] = new Chest()
            {
                x = x,
                y = y
            };
            Main.chest[chestIndex].item = new Item[Main.chest[chestIndex].item.Length];
            for (int i = 0; i < Main.chest[chestIndex].item.Length; i++)
            {
                Main.chest[chestIndex].item[i] = new Item();
            }
            return Main.chest[chestIndex];
        }
    }
}
