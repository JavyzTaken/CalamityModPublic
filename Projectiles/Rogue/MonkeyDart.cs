using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Rogue
{
    public class MonkeyDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Monkey Dart");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            drawOffsetX = -10;
            drawOriginOffsetY = 0;
            drawOriginOffsetX = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.extraUpdates = 2;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            //Code to make it not shoot backwards
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            //Gravity
            Projectile.velocity.Y = Projectile.velocity.Y + 0.04f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
            //Dust trail
            if (Main.rand.Next(25) == 0)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.9f);
                Main.dust[d].position = Projectile.Center;
                Main.dust[d].noLight = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Add the venom debuff
            target.AddBuff(BuffID.Venom, 180);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            //Dust splash
            int dustsplash = 0;
            while (dustsplash < 4)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 1, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 100, default, 0.9f);
                Main.dust[d].position = Projectile.Center;
                dustsplash += 1;
            }

            //Randomly not consume item if it wasnt a stealth strike
            if (Main.rand.Next(4) == 0 && Projectile.ai[0] != 1)
            {
                Item.NewItem((int)Projectile.position.X, (int)Projectile.position.Y, 27, 27, ModContent.ItemType<MonkeyDarts>());
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //This makes the projectile only bounce once
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.ai[1] >= 1f)
                {
                    Projectile.Kill();
                }
                else
                {
                    //Code for bouncing
                    Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                    if (Projectile.velocity.X != oldVelocity.X)
                    {
                        Projectile.velocity.X = -oldVelocity.X;
                    }
                    if (Projectile.velocity.Y != oldVelocity.Y)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y;
                    }
                    Projectile.ai[1] += 1f;
                }
                return false;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Projectile.ai[0] == 1)
            {
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
    }
}
