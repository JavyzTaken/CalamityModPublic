using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class AstrealDefeat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astreal Defeat");
            Tooltip.SetDefault("Ethereal bow of the tyrant king's mother\n" +
                "The mother strongly discouraged acts of violence throughout her life\n" +
                "Though she kept this bow close, to protect her family in times of great disaster\n" +
                "All arrows are converted to Astreal Arrows that emit flames as they travel");
        }

        public override void SetDefaults()
        {
            Item.damage = 153;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 78;
            Item.useTime = 4;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CalamityGlobalItem.Rarity11BuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AstrealArrow>();
            Item.shootSpeed = 4f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            float speed = velocity.Length();
            if (speed > 8f)
                velocity *= 8f / speed;

            // Always fires Astreal Arrows, regardless of ammo chosen.
            // Normally we like to allow bows to fire normal arrows but this weapon is incredibly overpowered when that is allowed.
            type = Item.shoot;
            float aiVar = Main.rand.Next(4);

            Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI, aiVar);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SpiritFlame).AddIngredient(ItemID.ShadowFlameBow).AddIngredient(ModContent.ItemType<GreatbowofTurmoil>()).AddIngredient(ModContent.ItemType<BladedgeGreatbow>()).AddIngredient(ModContent.ItemType<DarkechoGreatbow>()).AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5).AddIngredient(ItemID.LunarBar, 5).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}
