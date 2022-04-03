using CalamityMod.Items.Materials;
using CalamityMod.Tiles.DraedonStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.DraedonStructures
{
    public class AgedSecurityChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aged Security Chest");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 14;
            Item.rare = ItemRarityID.Green;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 500;
            Item.createTile = ModContent.TileType<AgedSecurityChestTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 4).AddIngredient(ModContent.ItemType<DubiousPlating>(), 4).AddIngredient(ModContent.ItemType<Items.Placeables.DraedonStructures.RustedPlating>(), 10).AddIngredient(ItemID.IronBar, 2).AddTile(TileID.Anvils).Register();
        }
    }
}
