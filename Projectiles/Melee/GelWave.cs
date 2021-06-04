using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee
{
    public class GelWave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wave");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.melee = true;
            projectile.penetrate = 2;
            projectile.alpha = 120;
            projectile.timeLeft = 200;
			projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Item92, projectile.position);
                projectile.localAI[0] += 1f;
            }
            Lighting.AddLight(projectile.Center, 0f, 0.2f, 0.4f);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.velocity.Y += projectile.ai[0];
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 20, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			if (projectile.timeLeft > 195)
				return false;

			CalamityUtils.DrawAfterimagesCentered(projectile, ProjectileID.Sets.TrailingMode[projectile.type], lightColor, 2);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, projectile.alpha);
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 20, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slimed, 300);
            projectile.velocity *= 0.5f;
        }
    }
}
