using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.DraedonsArsenal
{
    public class PlasmaGrenadeSmallExplosion : ModProjectile
    {
        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public int frameX = 0;
        public int frameY = 0;
        private const int horizontalFrames = 2;
        private const int verticalFrames = 7;
        private const int frameLength = 5;
        private const float radius = 139.5f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 279;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % frameLength == frameLength - 1)
            {
                frameY++;
                if (frameY >= verticalFrames)
                {
                    frameX++;
                    frameY = 0;
                }
                if (frameX >= horizontalFrames)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, radius, targetHitbox);

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[Projectile.type];
            int length = texture.Width / horizontalFrames;
            int height = texture.Height / verticalFrames;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle frame = new Rectangle(frameX * length, frameY * height, length, height);
            Vector2 origin = new Vector2(length / 2f, height / 2f);
            spriteBatch.Draw(texture, drawPos, frame, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
