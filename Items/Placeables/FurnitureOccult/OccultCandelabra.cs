using CalamityMod.Items.Placeables.FurnitureCosmilite;
using Terraria.ModLoader;
using Terraria.ID;
namespace CalamityMod.Items.Placeables.FurnitureOccult
{
    public class OccultCandelabra : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Otherworldly Candelabra");
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureOccult.OccultCandelabra>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<OccultStone>(), 15).AddIngredient(ModContent.ItemType<CosmiliteBrick>(), 3).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}
