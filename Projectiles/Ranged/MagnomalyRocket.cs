using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
namespace CalamityMod.Projectiles.Ranged
{
    public class MagnomalyRocket : ModProjectile
    {
        private bool spawnedAura = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nuke");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            //Lighting
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.25f / 255f, Main.DiscoG * 0.25f / 255f, Main.DiscoB * 0.25f / 255f);

            //Animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 7)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * Projectile.direction;

            int dustType = Main.rand.NextBool(2) ? 107 : 234;
            if (Main.rand.NextBool(4))
            {
                dustType = 269;
            }
            if (Projectile.owner == Main.myPlayer && !spawnedAura)
            {
                Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MagnomalyAura>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.5f, Projectile.owner, Projectile.identity, 0f);
                spawnedAura = true;
            }
            float dustOffsetX = Projectile.velocity.X * 0.5f;
            float dustOffsetY = Projectile.velocity.Y * 0.5f;
            if (Main.rand.NextBool(2))
            {
                int exo = Dust.NewDust(new Vector2(Projectile.position.X + 3f + dustOffsetX, Projectile.position.Y + 3f + dustOffsetY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, dustType, 0f, 0f, 100, default, 0.5f);
                Main.dust[exo].scale *= (float)Main.rand.Next(10) * 0.1f;
                Main.dust[exo].velocity *= 0.2f;
                Main.dust[exo].noGravity = true;
                Main.dust[exo].noLight = true;
            }
            else
            {
                int exo = Dust.NewDust(new Vector2(Projectile.position.X + 3f + dustOffsetX, Projectile.position.Y + 3f + dustOffsetY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, dustType, 0f, 0f, 100, default, 0.25f);
                Main.dust[exo].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[exo].velocity *= 0.05f;
                Main.dust[exo].noGravity = true;
                Main.dust[exo].noLight = true;
            }
            CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 200f, 12f, 20f);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 192);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                //DO NOT REMOVE THIS PROJECTILE
                Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MagnomalyExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);

                int dustType = Main.rand.NextBool(2) ? 107 : 234;
                if (Main.rand.NextBool(4))
                {
                    dustType = 269;
                }
                for (int d = 0; d < 30; d++)
                {
                    int exo = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1f);
                    Main.dust[exo].velocity *= 3f;
                    Main.dust[exo].noGravity = true;
                    Main.dust[exo].noLight = true;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[exo].scale = 0.5f;
                        Main.dust[exo].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int d = 0; d < 40; d++)
                {
                    int exo = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.5f);
                    Main.dust[exo].noGravity = true;
                    Main.dust[exo].noLight = true;
                    Main.dust[exo].velocity *= 5f;
                    exo = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.75f);
                    Main.dust[exo].velocity *= 2f;
                }
                CalamityUtils.ExplosionGores(Projectile.Center, 9);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            OnHitEffects();
            target.ExoDebuffs();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            OnHitEffects();
            target.ExoDebuffs();
        }

        private void OnHitEffects()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                float random = Main.rand.Next(30, 90);
                float spread = random * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                for (int i = 0; i < 4; i++)
                {
                    double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int proj1 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<MagnomalyBeam>(), Projectile.damage / 4, Projectile.knockBack / 4, Projectile.owner, 0f, 1f);
                    int proj2 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<MagnomalyBeam>(), Projectile.damage / 4, Projectile.knockBack / 4, Projectile.owner, 0f, 1f);
                }
            }
        }
    }
}
