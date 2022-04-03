using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Rogue
{
    public class ProfanedPartisanProj : ModProjectile
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/ProfanedPartisan";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Profaned Partisan");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 9;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 0.4f)
            {
                Projectile.ai[0] += 0.1f;
            }
            else
            {
                Projectile.tileCollide = true;
            }

            //Backwards projectile fix
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            //Rotating 45 degrees if shooting right
            if (Projectile.spriteDirection == 1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
                drawOffsetX = -26;
                drawOriginOffsetX = 13;
                drawOriginOffsetY = 2;
            }
            //Rotating 45 degrees if shooting right
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(45f);
                drawOffsetX = 2;
                drawOriginOffsetX = -13;
                drawOriginOffsetY = 2;
            }

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.ProfanedFire, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
                Main.dust[d].position = Projectile.Center;
                Main.dust[d].velocity *= 0.3f;
                Main.dust[d].velocity += Projectile.velocity * 0.85f;
            }
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.2f);

            if (Projectile.Calamity().stealthStrike) //Stealth strike
            {
                Vector2 spearPosition = new Vector2(Projectile.Center.X + Main.rand.NextFloat(-15f, 15f), Projectile.Center.Y + Main.rand.NextFloat(-15f, 15f));
                Vector2 spearSpeed = Projectile.velocity;
                if (Projectile.timeLeft % 18 == 0)
                {
                    int projID = ModContent.ProjectileType<ProfanedPartisanSpear>();
                    int spearDamage = (int)(Projectile.damage * 0.4f);
                    float spearKB = 1f;
                    Projectile.NewProjectile(spearPosition, spearSpeed, projID, spearDamage, spearKB, Projectile.owner);
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.ProfanedFire, 0f, 0f, 50, default, 2.6f);
            }
            SoundEngine.PlaySound(SoundID.Item45, Projectile.position);

            int projID = ModContent.ProjectileType<PartisanExplosion>();
            int explosionDamage = (int)(Projectile.damage * 0.8f);
            float explosionKB = 8f;
            Projectile.NewProjectile(Projectile.Center, Vector2.Zero, projID, explosionDamage, explosionKB, Projectile.owner);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
    }
}
