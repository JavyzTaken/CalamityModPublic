using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ReaverScaleMail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reaver Scale Mail");
            Tooltip.SetDefault("9% increased damage and 4% increased critical strike chance\n" +
                "+20 max life");
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 19;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.allDamage += 0.09f;
            player.Calamity().AllCritBoost(4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DraedonBar>(), 15).AddIngredient(ItemID.JungleSpores, 12).AddIngredient(ModContent.ItemType<EssenceofCinder>(), 3).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
