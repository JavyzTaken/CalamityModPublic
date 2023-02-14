using CalamityMod.Tiles.Crags;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityMod.Tiles.Ores
{
    [LegacyName("CharredOre")]
    public class InfernalSuevite : ModTile
    {
        private int sheetWidth = 288;
        private int sheetHeight = 270;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileOreFinderPriority[Type] = 675;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithHell(Type);

            TileID.Sets.Ore[Type] = true;

            ItemDrop = ModContent.ItemType<Items.Placeables.Ores.InfernalSuevite>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Infernal Suevite");
            AddMapEntry(new Color(17, 16, 26), name);
            MineResist = 6f;
            MinPick = 150;
            HitSound = SoundID.Tink;
            DustType = 235;
            Main.tileSpelunker[Type] = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = i % 2 * sheetWidth;
            frameYOffset = j % 2 * sheetHeight;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.50f;
            g = 0.00f;
            b = 0.00f;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.CustomMergeFrame(i, j, Type, ModContent.TileType<BrimstoneSlag>(), false, false, false, false, resetFrame);
            return false;
        }
    }
}