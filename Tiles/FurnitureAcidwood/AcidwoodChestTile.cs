using CalamityMod.Items.Placeables.FurnitureAcidwood;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.FurnitureAcidwood
{
    public class AcidwoodChestTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            this.SetUpChest(true);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Acidwood Chest");
            AddMapEntry(new Color(191, 142, 111), name, MapChestName);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Containers };
            chest = "Acidwood Chest";
            chestDrop = ModContent.ItemType<AcidwoodChest>();
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 7, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override bool HasSmartInteract() => true;

        public string MapChestName(string name, int i, int j) => CalamityUtils.GetMapChestName(name, i, j);

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
            Chest.DestroyChest(i, j);
        }

        public override bool RightClick(int i, int j) => CalamityUtils.ChestRightClick(i, j);

        public override void MouseOver(int i, int j) => CalamityUtils.ChestMouseOver<AcidwoodChest>("Acidwood Chest", i, j);

        public override void MouseOverFar(int i, int j) => CalamityUtils.ChestMouseFar<AcidwoodChest>("Acidwood Chest", i, j);
    }
}
