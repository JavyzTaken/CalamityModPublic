using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class BrimflameBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimflame Boots");
            Tooltip.SetDefault("5% increased magic damage\n" +
                "5% increased movement speed");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CalamityDust>(), 5).AddIngredient(ModContent.ItemType<UnholyCore>(), 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
