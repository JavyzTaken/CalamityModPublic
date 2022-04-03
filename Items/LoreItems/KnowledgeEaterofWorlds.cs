using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.LoreItems
{
    public class KnowledgeEaterofWorlds : LoreItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Eater of Worlds");
            Tooltip.SetDefault("Perhaps it was just a giant worm infected by the microbe, given centuries to feed and grow its festering body.\n" +
                "Seems likely, given the origins of this place.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddTile(TileID.Bookcases).AddIngredient(ItemID.EaterofWorldsTrophy).AddIngredient(ModContent.ItemType<VictoryShard>(), 10).Register();
        }
    }
}
