using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class AlchemicalFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alchemical Flask");
            Tooltip.SetDefault("All attacks inflict the Plague debuff\n" +
                "Projectiles spawn plague seekers on enemy hits");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.alchFlask = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BottledWater).AddIngredient(ItemID.BeeWax, 5).AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 10).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
