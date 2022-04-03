using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Melee
{
    public class DragonRageFireball : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Rogue/DragonShit";
        public NPC target;
        private int lifeTime = 420;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 66;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = lifeTime;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = Main.rand.NextFloat(40f, 70f);
                Projectile.ai[1] = Main.rand.NextFloat(35f, 55f);
            }
            target = Projectile.Center.ClosestNPCAt(1200f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

            Projectile.velocity *= 1.01f;

            if (target != null && Projectile.timeLeft < lifeTime - 10)
            {
                float inertia = Projectile.ai[0];
                float speed = Projectile.ai[1];
                Vector2 moveDirection = Projectile.SafeDirectionTo(target.Center, Vector2.UnitY);
                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + moveDirection * speed) / inertia;
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, Projectile.alpha);

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D projectileTexture = Main.projectileTexture[Projectile.type];
            int frameHeight = Main.projectileTexture[Projectile.type].Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            Main.spriteBatch.Draw(projectileTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, frameY, projectileTexture.Width, frameHeight)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)projectileTexture.Width / 2f, (float)frameHeight / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 80);
            for (int d = 0; d < 5; d++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1f);
                Main.dust[idx].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[idx].scale = 0.5f;
                    Main.dust[idx].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 8; d++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 2f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 5f;
                idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1f);
                Main.dust[idx].velocity *= 2f;
            }
            CalamityUtils.ExplosionGores(Projectile.Center, 3);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 120);
        }
    }
}
