﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Crags;
using System;
using System.Linq;

namespace CalamityMod.Tiles.Crags.Tree
{
    internal class SpineTree : ModTile
    {
        /*
        What each frame of the tree means
        (refer to the sprite sheet for the colors)
        X frame 0 (orange) = bottom segment
        X frame 18 (dark purple) = main body segment, each of the three y-frame variants draws a different tree segment variant
        X frame 36 (light purple) = top of the tree
        X frame 54 (teal) = small rib branch segments
        X frame 72 (lime) = medium rib branch segments
        X frame 90 (yellow) = large rib branch segments
        
        each of the branch frames have a pattern where
        y-frame 1 = left branch
        y-frame 2 = right branch
        y-frame 3 = both left and right branch
        */

        public override void SetStaticDefaults()
        {
            TileID.Sets.IsATreeTrunk[Type] = true;
			Main.tileFrameImportant[Type] = true;
            Main.tileAxe[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = false;
            Main.tileBlockLight[Type] = false;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Giant Spine");
            AddMapEntry(new Color(38, 25, 27), name);
            DustType = 155;
			HitSound = SoundID.DD2_SkeletonHurt;
            ItemDrop = ModContent.ItemType<Items.Placeables.ScorchedBone>();
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            resetFrame = false;
            noBreak = true;
            return false;
        }

        public static bool SolidTile(int i, int j) 
        {
            return Framing.GetTileSafely(i, j).HasTile && Main.tileSolid[Framing.GetTileSafely(i, j).TileType];
        }

        public static bool SolidTopTile(int i, int j) 
        {
            return Framing.GetTileSafely(i, j).HasTile && (Main.tileSolidTop[Framing.GetTileSafely(i, j).TileType] || 
            Main.tileSolid[Framing.GetTileSafely(i, j).TileType]);
        }

        public static bool Spawn(int i, int j, int minSize = 5, int maxSize = 18, bool leaves = false, int leavesType = -1, bool saplingExists = false)
        {
            //if this tree grew from a sapling, then kill the sapling its growing from
            if (saplingExists)
            {
                WorldGen.KillTile(i, j, false, false, true);
                WorldGen.KillTile(i, j - 1, false, false, true);
            }

            //set the minimum and maximum height the tree can grow to
            int height = Main.rand.Next(minSize, maxSize);
            for (int k = 1; k < height; ++k)
            {
                if (SolidTile(i, j - k))
                {
                    height = k - 2;
                    break;
                }
            }

            if (height < 4 || height < minSize) 
            {
                return false;
            }

            //this places the base, probably a bit weird looking here but it works
            //problem is messing around with this completely broke the hell out of the tree so i refuse to modify it for now
            //if someone else by some miracle can figure out how to change it, please let me know :)
            bool[] extraPlaces = new bool[5];
            for (int k = -2; k <= 2; k++)
            {
                extraPlaces[k + 2] = false;

                if ((SolidTopTile(i + k, j + 1) || SolidTile(i + k, j + 1)) && !Framing.GetTileSafely(i + k, j).HasTile)
                {
                    extraPlaces[k + 2] = true;
                }
            }

            if (!extraPlaces[1]) extraPlaces[0] = false;
            if (!extraPlaces[3]) extraPlaces[4] = false;

            if (!extraPlaces[2]) 
            {
                return false;
            }

            extraPlaces = new bool[5] { false, false, true, false, false };

            //place the actual base
            for (int k = -2; k <= 2; k++)
            {
                if (extraPlaces[k + 2])
                {
                    WorldGen.PlaceTile(i + k, j, ModContent.TileType<SpineTree>(), true);

                    Framing.GetTileSafely(i + k, j).TileFrameX = 0;
                    Framing.GetTileSafely(i, j - k).TileFrameY = (short)(Main.rand.Next(3) * 18);
                }
                else
                {
                    continue;
                }
            }

            int branchSegmentDelay = 0;

            for (int k = 1; k < height; k++)
            {
                WorldGen.PlaceTile(i, j - k, ModContent.TileType<SpineTree>(), true);

                if (branchSegmentDelay > 0)
                {
                    branchSegmentDelay--;
                }

                //chance to place a branch segment
                //also dont place branches below a certain threshold
                if (Main.rand.Next(2) == 0 && branchSegmentDelay == 0 && k > 5)
                {
                    float divide = 1.65f;

                    if (k > 1 && k < 10)
                    {
                        Framing.GetTileSafely(i, j - k).TileFrameX = 3 * 18;
                    }
                    if (k >= 10 && k < 17)
                    {
                        Framing.GetTileSafely(i, j - k).TileFrameX = 4 * 18;
                    }
                    if (k >= 17)
                    {
                        Framing.GetTileSafely(i, j - k).TileFrameX = 5 * 18;
                    }

                    Framing.GetTileSafely(i, j - k).TileFrameY = (short)(Main.rand.Next(3) * 18);

                    branchSegmentDelay = 3;
                }
                //otherwise place a normal segment
                else
                {
                    Framing.GetTileSafely(i, j - k).TileFrameX = 18;
                    Framing.GetTileSafely(i, j - k).TileFrameY = (short)(Main.rand.Next(3) * 18);
                }

                //once the top is reached, place the top segment
                if (k == height - 1)
                {
                    Framing.GetTileSafely(i, j - k).TileFrameX = 2 * 18;
                    Framing.GetTileSafely(i, j - k).TileFrameY = (short)(Main.rand.Next(3) * 18);
                }
            }

            return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            //kill the tree tiles if there are no tiles below it
            if (!Framing.GetTileSafely(i, j + 1).HasTile)
            {
                WorldGen.KillTile(i, j, false, false, false);
            }
        }

        public override bool Drop(int i, int j)
        {
            /*
            //drop seeds from the top of the tree
            if (Framing.GetTileSafely(i, j).TileFrameX == 36)
            {
                int totalSeeds = Main.rand.Next(1, 3);
                for (int numSeed = 0; numSeed < totalSeeds; numSeed++)
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), 
                    Main.rand.Next(-40, 40) - 66), ModContent.ItemType<CustomTreeSeed>(), Main.rand.Next(1, 5));
                }
            }
            */

            //chance to drop extra wood
            if (Main.rand.NextBool())
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), (new Vector2(i, j) * 16) + new Vector2(Main.rand.Next(-46, 46), 
                Main.rand.Next(-40, 40) - 66), ModContent.ItemType<Items.Placeables.ScorchedBone>(), Main.rand.Next(1, 2));
            }

            return true;
        }

        //this checks the entire tree from bottom to top
        private void CheckEntireTree(ref int x, ref int y)
        {
            while (Main.tile[x, y].TileType == Type)
			{
                y--;
			}

            y++;

            if (Main.tile[x, y].TileFrameX == 16)
            {
                for (int k = 0; k < WorldGen.numTreeShakes; k++)
				{
                    if (WorldGen.treeShakeX[k] == x && WorldGen.treeShakeY[k] == y)
					{
                        return;
					}
				}

                WorldGen.treeShakeX[WorldGen.numTreeShakes] = x;
                WorldGen.treeShakeY[WorldGen.numTreeShakes] = y;
                WorldGen.numTreeShakes++;

                //this is where you can make stuff happen when the tree is shaken like in vanilla
                //for example, when trees drop a fruit when you begin to break it
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (fail && !effectOnly && !noItem)
            {
                (int x, int y) = (i, j);
                CheckEntireTree(ref x, ref y);
            }

            if (fail)
            {
                return;
            }

            int belowFrame = Framing.GetTileSafely(i, j + 1).TileFrameX;

            SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, (new Vector2(i, j) * 16));

            //here is where you can make effects happen when the tree segments break, like adding dust, dropping gore, ect

            /*
            //TODO: save this since this tree will also produce bone gores when broken (gores for this tree still need assets)
            Gore.NewGore(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), (new Vector2(i, j - 2) * 16),
            new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), ModContent.Find<ModGore>("CalamityMod/WhateverGore").Type);
            */

            //TODO: spawn all the gores here

            //bottom segment
            if (Framing.GetTileSafely(i, j).TileFrameX == 0)
            {
            }

            //regular segment
            if (Framing.GetTileSafely(i, j).TileFrameX == 18)
            {
            }

            //small branch
            if (Framing.GetTileSafely(i, j).TileFrameX == 54)
            {
            }

            //medium branch
            if (Framing.GetTileSafely(i, j).TileFrameX == 72)
            {
            }

            //large branch
            if (Framing.GetTileSafely(i, j).TileFrameX == 90)
            {
            }

            //top segment
            if (Framing.GetTileSafely(i, j).TileFrameX == 36)
            {
            }
        }

        public static Vector2 TileOffset => Lighting.LegacyEngine.Mode > 1 ? Vector2.Zero : Vector2.One * 12;

        public static Vector2 TileCustomPosition(int i, int j, Vector2? off = null)
        {
            return ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition - (off ?? new Vector2(0, -2));
        }

        internal static void DrawTreeSegments(int i, int j, Texture2D tex, Rectangle? source, Vector2? offset = null, Vector2? origin = null, bool Glow = false)
        {
            Tile tile = Main.tile[i, j];
            Vector2 drawPos = new Vector2(i, j).ToWorldCoordinates() - Main.screenPosition + (offset ?? new Vector2(0, -2));
            Color color = Lighting.GetColor(i, j);

            Main.spriteBatch.Draw(tex, drawPos, source, Glow ? Color.White : color, 0, origin ?? source.Value.Size() / 3f, 1f, SpriteEffects.None, 0f);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Color col = Lighting.GetColor(i, j);
            float xOff = (float)Math.Sin((j * 19) * 0.04f) * 1.2f;

            if (xOff == 1 && (j / 4f) == 0)
            {
                xOff = 0;
            }

            int frameSizeX = 16;
            int frameSizeY = 16;
            int frameSize = 16;
            int frameOff = 0;

            Vector2 offset = new((xOff * 2) - (frameOff / 2), 0);
            Vector2 pos = TileCustomPosition(i, j) - offset;

            Vector2 baseSegmentOffset = new Vector2((xOff * 2) - (frameOff / 2) + 26, 14);
            Vector2 treeSegmentOffset = new Vector2((xOff * 2) - (frameOff / 2) + 25, 14);
            Vector2 topSegmentOffset = new Vector2((xOff * 2) - (frameOff / 2) + 20, 16);

            Texture2D baseTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineBottom").Value;
            Texture2D segmentTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineSegments").Value;

            //draw the base of the tree
            if (Framing.GetTileSafely(i, j).TileFrameX == 0)
            {
                int frame = tile.TileFrameY / 18;

                DrawTreeSegments(i, j, baseTex, new Rectangle(34 * frame, 0, 32, 20), TileOffset.ToWorldCoordinates(), baseSegmentOffset, false);
            }

            //draw the different segments of the tree
            if (Framing.GetTileSafely(i, j).TileFrameX == 18)
            {
                int frame = tile.TileFrameY / 18;

                DrawTreeSegments(i, j, segmentTex, new Rectangle(34 * frame, 0, 32, 20), TileOffset.ToWorldCoordinates(), treeSegmentOffset, false);
            }

            //draw small branch segments
            if (Framing.GetTileSafely(i, j).TileFrameX == 54)
            {
                int frame = tile.TileFrameY / 18;

                //branch drawing stuff
                Texture2D leftBranchTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineRib1Left").Value;
                Texture2D rightBranchTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineRib1Right").Value;

                Vector2 leftBranchOffset = new Vector2((xOff * 2) - (frameOff / 2) + 50, 14);
                Vector2 rightBranchOffset = new Vector2((xOff * 2) - (frameOff / 2) + 4, 14);

                //ok so, terraria just does not want to cooperate when i draw a single branch texture on the side of the tree segments
                //i changed them into a horitonzal sheet with blanks in it so it gives the illusion of branches being random
                //not a good idea if branches are meant to be separate tiles, but here they are purely visual so it doesnt matter
                DrawTreeSegments(i, j, leftBranchTex, new Rectangle(38 * frame, 0, 38, 40), TileOffset.ToWorldCoordinates(), leftBranchOffset, false);
                DrawTreeSegments(i, j, rightBranchTex, new Rectangle(38 * frame, 0, 38, 40), TileOffset.ToWorldCoordinates(), rightBranchOffset, false);

                //draw segments
                DrawTreeSegments(i, j, segmentTex, new Rectangle(34 * frame, 0, 32, 20), TileOffset.ToWorldCoordinates(), treeSegmentOffset, false);
            }

            //draw medium branch segments
            if (Framing.GetTileSafely(i, j).TileFrameX == 72)
            {
                int frame = tile.TileFrameY / 18;

                //branch drawing stuff
                Texture2D leftBranchTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineRib2Left").Value;
                Texture2D rightBranchTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineRib2Right").Value;

                Vector2 leftBranchOffset = new Vector2((xOff * 2) - (frameOff / 2) + 60, 14);
                Vector2 rightBranchOffset = new Vector2((xOff * 2) - (frameOff / 2) + 4, 14);

                DrawTreeSegments(i, j, leftBranchTex, new Rectangle(50 * frame, 0, 50, 54), TileOffset.ToWorldCoordinates(), leftBranchOffset, false);
                DrawTreeSegments(i, j, rightBranchTex, new Rectangle(50 * frame, 0, 50, 54), TileOffset.ToWorldCoordinates(), rightBranchOffset, false);

                //draw segments
                DrawTreeSegments(i, j, segmentTex, new Rectangle(34 * frame, 0, 32, 20), TileOffset.ToWorldCoordinates(), treeSegmentOffset, false);
            }

            //draw large branch segments
            if (Framing.GetTileSafely(i, j).TileFrameX == 90)
            {
                int frame = tile.TileFrameY / 18;

                //branch drawing stuff
                Texture2D leftBranchTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineRib3Left").Value;
                Texture2D rightBranchTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineRib3Right").Value;

                Vector2 leftBranchOffset = new Vector2((xOff * 2) - (frameOff / 2) + 75, 14);
                Vector2 rightBranchOffset = new Vector2((xOff * 2) - (frameOff / 2) + 4, 14);

                DrawTreeSegments(i, j, leftBranchTex, new Rectangle(62 * frame, 0, 62, 60), TileOffset.ToWorldCoordinates(), leftBranchOffset, false);
                DrawTreeSegments(i, j, rightBranchTex, new Rectangle(62 * frame, 0, 62, 60), TileOffset.ToWorldCoordinates(), rightBranchOffset, false);

                //draw segments
                DrawTreeSegments(i, j, segmentTex, new Rectangle(34 * frame, 0, 32, 20), TileOffset.ToWorldCoordinates(), treeSegmentOffset, false);
            }

            //draw top segment
            if (Framing.GetTileSafely(i, j).TileFrameX == 36)
            {
                Texture2D topTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineTop").Value;
                int frame = tile.TileFrameY / 18;

                DrawTreeSegments(i, j - 1, topTex, new Rectangle(26 * frame, 0, 24, 24), TileOffset.ToWorldCoordinates(), topSegmentOffset, false);

                //draw segments
                DrawTreeSegments(i, j, segmentTex, new Rectangle(34 * frame, 0, 32, 20), TileOffset.ToWorldCoordinates(), treeSegmentOffset, false);
            }

            /*
            //keep this here for debugging
            Texture2D treeTex = ModContent.Request<Texture2D>("CalamityMod/Tiles/Crags/Tree/SpineTree").Value;

            spriteBatch.Draw(treeTex, pos, new Rectangle(tile.TileFrameX + frameOff, tile.TileFrameY, frameSize, frameSizeY), 
            new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            */

            return false;
        }
    }
}