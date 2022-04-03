using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class SandSharkToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sand Shark Tooth Necklace");
            Tooltip.SetDefault("Increases armor penetration by 10\n" + "6% increased damage");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 44;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.allDamage += 0.06f;
            player.armorPenetration += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SharkToothNecklace).AddIngredient(ItemID.AvengerEmblem).AddIngredient(ModContent.ItemType<GrandScale>()).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
