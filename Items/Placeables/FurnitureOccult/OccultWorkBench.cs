using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables.FurnitureOccult
{
    public class OccultWorkBench : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Otherworldly Work Bench");
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureOccult.OccultWorkBench>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<OccultStone>(), 10).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}
