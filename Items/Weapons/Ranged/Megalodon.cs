using CalamityMod.Projectiles.Ranged;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Megalodon : ModItem
    {
        private int shotType = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Megalodon");
            Tooltip.SetDefault("50% chance to not consume ammo\n" +
                "Fires streams of water every other shot");
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 72;
            Item.height = 32;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float SpeedX = speedX + Main.rand.Next(-10, 11) * 0.05f;
            float SpeedY = speedY + Main.rand.Next(-10, 11) * 0.05f;

            if (shotType > 2)
                shotType = 1;

            if (shotType == 1)
                Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            else
                Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<ArcherfishShot>(), damage, knockBack, player.whoAmI, 0f, 0f);

            shotType++;

            return false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            if (shotType == 2)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Megashark).AddIngredient(ModContent.ItemType<Archerfish>()).AddIngredient(ModContent.ItemType<DepthCells>(), 10).AddIngredient(ModContent.ItemType<Lumenite>(), 10).AddIngredient(ModContent.ItemType<Tenebris>(), 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
