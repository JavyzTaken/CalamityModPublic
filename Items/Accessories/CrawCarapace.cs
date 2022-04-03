using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityMod.Items.Accessories
{
    public class CrawCarapace : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Craw Carapace");
            Tooltip.SetDefault("5% increased damage reduction\n" +
                "Enemies take damage when they touch you");
        }

        public override void SetDefaults()
        {
            Item.defense = 3;
            Item.width = 28;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.05f;
            player.thorns += 0.25f;
        }
    }
}
