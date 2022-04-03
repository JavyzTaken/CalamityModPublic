using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class GodSlayerLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God Slayer Leggings");
            Tooltip.SetDefault("5% increased movement speed\n" +
                "10% increased damage and critical strike chance");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.defense = 35;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
            player.allDamage += 0.1f;
            player.Calamity().AllCritBoost(10);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CosmiliteBar>(), 18).AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2).AddTile(ModContent.TileType<CosmicAnvil>()).Register();
        }
    }
}
