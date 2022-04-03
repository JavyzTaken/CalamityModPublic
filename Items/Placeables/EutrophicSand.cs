using CalamityMod.Items.Placeables.Walls;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityMod.Items.Placeables
{
    public class EutrophicSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eutrophic Sand");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.SunkenSea.EutrophicSand>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EutrophicSandWallSafe>(), 4).AddTile(TileID.WorkBenches).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EutrophicSandWall>(), 4).AddTile(TileID.WorkBenches).Register();
        }
    }
}
