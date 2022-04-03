using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class AirSpinner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Air Spinner");
            Tooltip.SetDefault("Fires feathers when enemies are near\n" +
            "A very agile yoyo");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 26;
            Item.knockBack = 4f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<AirSpinnerYoyo>();
            Item.shootSpeed = 14f;

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<AerialiteBar>(), 6).AddTile(TileID.SkyMill).Register();
        }
    }
}
