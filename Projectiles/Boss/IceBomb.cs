using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Events;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Boss
{
    public class IceBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.Calamity().canBreakPlayerDefense = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 0.5f;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.Calamity().affectedByMaliceModeVelocityMultiplier = true;
        }

        public override bool CanHitPlayer(Player target) => Projectile.ai[0] >= 120f;

        public override void AI()
        {
            Projectile.velocity *= 0.99f;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.01f;
                Projectile.alpha -= 50;
                if (Projectile.alpha <= 0)
                {
                    Projectile.localAI[0] = 1f;
                    Projectile.alpha = 0;
                }
            }
            else
            {
                Projectile.scale -= 0.01f;
                Projectile.alpha += 50;
                if (Projectile.alpha >= 255)
                {
                    Projectile.localAI[0] = 0f;
                    Projectile.alpha = 255;
                }
            }

            if (Projectile.ai[0] < 120f)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] == 120f)
                {
                    for (int num621 = 0; num621 < 8; num621++)
                    {
                        int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[num622].velocity *= 3f;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num622].scale = 0.5f;
                            Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int num623 = 0; num623 < 14; num623++)
                    {
                        int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67, 0f, 0f, 100, default, 3f);
                        Main.dust[num624].noGravity = true;
                        Main.dust[num624].velocity *= 5f;
                        num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[num624].velocity *= 2f;
                    }

                    Projectile.scale = 1f;
                    CalamityGlobalProjectile.ExpandHitboxBy(Projectile, (int)(30f * Projectile.scale));
                    SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.dayTime ? new Color(50, 50, 255, Projectile.alpha) : new Color(255, 255, 255, Projectile.alpha);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            if (Projectile.owner == Main.myPlayer)
            {
                int totalProjectiles = (CalamityWorld.revenge || BossRushEvent.BossRushActive ? 3 : 2) + (int)Projectile.ai[1];
                float radians = MathHelper.TwoPi / totalProjectiles;
                int type = ModContent.ProjectileType<IceRain>();
                int damage = (int)Math.Round(Projectile.damage * 0.75);
                float velocity = CalamityWorld.revenge || BossRushEvent.BossRushActive ? 7f : 6f;
                Vector2 spinningPoint = new Vector2(0f, -velocity);
                for (int k = 0; k < totalProjectiles; k++)
                {
                    Vector2 vector255 = spinningPoint.RotatedBy(radians * k);
                    Projectile.NewProjectile(Projectile.Center, vector255, type, damage, 0f, Projectile.owner, 1f, 0f);
                }
            }

            for (int k = 0; k < 10; k++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Projectile.ai[0] >= 120f)
            {
                target.AddBuff(BuffID.Frostburn, 120, true);
                target.AddBuff(BuffID.Chilled, 90, true);
                target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
            }
        }
    }
}
