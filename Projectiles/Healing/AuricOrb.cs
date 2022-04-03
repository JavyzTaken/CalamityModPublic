using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Healing
{
    public class AuricOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Auric Orb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            Projectile.alpha -= 2;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.05f;
                if (Projectile.scale > 1.2f)
                {
                    Projectile.localAI[0] = 1f;
                }
            }
            else
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale < 0.8f)
                {
                    Projectile.localAI[0] = 0f;
                }
            }

            Projectile.HealingProjectile((int)Projectile.ai[1], (int)Projectile.ai[0], 6f, 15f);
            return;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, Main.DiscoG, 53, Projectile.alpha);
        }

        public override void Kill(int timeLeft)
        {
            for (int num407 = 0; num407 < 5; num407++)
            {
                int num408 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 157, 0f, 0f, 0, new Color(255, Main.DiscoG, 53), 1f);
                Main.dust[num408].noGravity = true;
                Main.dust[num408].velocity *= 1.5f;
                Main.dust[num408].scale = 1.5f;
            }
        }
    }
}
