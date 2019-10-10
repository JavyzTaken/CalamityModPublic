﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityMod.Items.CalamityCustomThrowingDamage
{
    public class MonkeyDarts : CalamityDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Monkey Darts");
            Tooltip.SetDefault("Throws three bouncing darts if stealth is full\n" + "Perfect for popping");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 270;
            item.knockBack = 4;
            item.crit = 18;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.consumable = true;
            item.maxStack = 999;
            item.width = 27;
            item.height = 27;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 7;
            item.value = Item.buyPrice(0, 0, 4, 0);
            item.shootSpeed = 10.5f;
            item.shoot = mod.ProjectileType("MonkeyDart");
            item.autoReuse = true;
            item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Checks if stealth is avalaible to shoot a spread of 3 darts
            if (player.Calamity().StealthStrikeAvailable())
            {
                float spread = 7;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedspeed = new Vector2(speedX * 1.4f, speedY * 1.4f).RotatedBy(MathHelper.ToRadians(spread));
                    int p = Projectile.NewProjectile(position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, ModContent.ProjectileType<MonkeyDart>(), item.damage, item.knockBack, player.whoAmI,1);
                    Main.projectile[p].Calamity().stealthStrike = true;
                    spread -= 7;
                }
                return false;
            }

            else
            {
                return true;
            }
        }

    }
}
