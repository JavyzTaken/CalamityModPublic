using CalamityMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityMod.Tiles.Furniture
{
    public class BabyGhostBellJarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
            animationFrameHeight = 36;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Baby Ghost Bell Jar");
            AddMapEntry(new Color(64, 224, 208), name);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 13, 0f, 0f, 0, new Color(), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            int frameAmt = 8;
            int timeNeeded = 10;
            frameCounter++;
            if (frameCounter >= timeNeeded)
            {
                frame++;
                frameCounter = 0;
            }
            if (frame >= frameAmt)
            {
                frame = 0;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameYOffset = this.GetAnimationOffset(i, j, 8, 18, 18, 2, 2, animationFrameHeight);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.25f;
            g = 0.88f;
            b = 0.82f;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<BabyGhostBellJar>());
        }
    }
}
