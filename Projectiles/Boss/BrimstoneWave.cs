using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Boss
{
    public class BrimstoneWave : ModProjectile
    {
        private int x;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone Flame Skull");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.Opacity = 0f;
            cooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(x);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            x = reader.ReadInt32();
        }

        public override void AI()
        {
            x++;
            Projectile.velocity.Y = (float)(5D * Math.Sin(x / 5D));

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 12)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            if (Projectile.timeLeft < 30)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 1170) / 30f), 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0.5f * Projectile.Opacity, 0f, 0f);

            if (Projectile.velocity.X < 0f)
                Projectile.spriteDirection = 1;
            else
                Projectile.spriteDirection = -1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture2D13 = Main.projectileTexture[Projectile.type];
            int num214 = Main.projectileTexture[Projectile.type].Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            lightColor.R = (byte)(255 * Projectile.Opacity);
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), Projectile.scale, spriteEffects, 0f);
            return false;
        }

        public override bool CanHitPlayer(Player target) => Projectile.Opacity == 1f;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Projectile.Opacity != 1f)
                return;

            target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 180);
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120, true);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.Calamity().lastProjectileHit = projectile;
        }
    }
}
