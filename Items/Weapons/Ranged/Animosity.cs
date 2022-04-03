using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Animosity : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animosity");
            Tooltip.SetDefault(@"50% chance to not consume ammo
Fires a powerful sniper round
Right click to fire a burst of bullets");
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 70;
            Item.height = 18;
            Item.useTime = 4;
            Item.reuseDelay = 15;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 4;
                Item.reuseDelay = 13;
                Item.useAnimation = 12;
                Item.UseSound = SoundID.Item31;
                Item.shootSpeed = 10f;
            }
            else
            {
                Item.useTime = 35;
                Item.reuseDelay = 0;
                Item.useAnimation = 35;
                Item.UseSound = SoundID.Item40;
                Item.shootSpeed = 15f;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                float SpeedX = speedX + (float)Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = speedY + (float)Main.rand.Next(-10, 11) * 0.05f;
                Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
            else
            {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.BulletHighVelocity, (int)(damage * 5.8f), knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
        }

        public override bool ConsumeAmmo(Player player)
        {
            if (Main.rand.Next(0, 100) < 50)
                return false;
            return true;
        }
    }
}
