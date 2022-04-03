using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class ProfanedSoulArtifact : ModItem //My precious babs <3 ~Amber
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Profaned Soul Artifact");
            Tooltip.SetDefault("Purity\n" +
                "Summons a healer guardian which heals for a certain amount of health every few seconds\n" +
                "Summons a defensive guardian if you have at least 10 minion slots, which boosts your movement speed and your damage resistance\n" +
                "Summons an offensive guardian if you are wearing the tarragon summon set (or stronger), which boosts your summon damage and your minion slots\n" +
                "If you get hit, most of their effects will disappear for 5 seconds");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 40;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.Calamity().donorItem = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return !player.Calamity().pArtifact;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.pArtifact = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 25).AddIngredient(ModContent.ItemType<Chaosplate>(), 25).AddIngredient(ModContent.ItemType<DivineGeode>(), 5).AddTile(TileID.DemonAltar).Register();
        }
    }
}
