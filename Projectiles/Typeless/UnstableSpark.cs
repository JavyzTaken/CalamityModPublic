using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Typeless
{
    public class UnstableSpark : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unstable Spark");
        }

        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (projectile.velocity.X != projectile.velocity.X)
            {
                projectile.velocity.X = projectile.velocity.X * -0.1f;
            }
            if (projectile.velocity.X != projectile.velocity.X)
            {
                projectile.velocity.X = projectile.velocity.X * -0.5f;
            }
            if (projectile.velocity.Y != projectile.velocity.Y && projectile.velocity.Y > 1f)
            {
                projectile.velocity.Y = projectile.velocity.Y * -0.5f;
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 5f)
            {
                projectile.ai[0] = 5f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X = projectile.velocity.X * 0.97f;
                    if (projectile.velocity.X > -0.01 && projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.1f;
            int num199 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, 0f, 0f, 100, default, 0.5f);
            Dust dust = Main.dust[num199];
            dust.position.X -= 2f;
            dust.position.Y += 2f;
            dust.scale += Main.rand.Next(50) * 0.01f;
            dust.noGravity = true;
            dust.velocity.Y -= 2f;
            if (Main.rand.NextBool(2))
            {
                int num200 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, 0f, 0f, 100, default, 0.5f);
                Dust dust2 = Main.dust[num200];
                dust2.position.X -= 2f;
                dust2.position.Y += 2f;
                dust2.scale += 0.3f + Main.rand.Next(50) * 0.01f;
                dust2.noGravity = true;
                dust2.velocity *= 0.1f;
            }
            if (projectile.velocity.Y < 0.25 && projectile.velocity.Y > 0.15)
            {
                projectile.velocity.X = projectile.velocity.X * 0.8f;
            }
            projectile.rotation = -projectile.velocity.X * 0.05f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
        }

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}
