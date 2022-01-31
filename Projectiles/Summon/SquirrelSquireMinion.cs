using CalamityMod.Buffs.Summon;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Summon
{
    public class SquirrelSquireMinion : ModProjectile
    {
        public ref float AttackTimer => ref projectile.ai[1];
        public bool Attacking
        {
            get => projectile.localAI[1] == 1f;
            set => projectile.localAI[1] = value.ToInt();
        }
        public Player Owner => Main.player[projectile.owner];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Squirrel Squire");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 17; //About half of these frames are now unused, but I'll still keep them here incase
        }

        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 64;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.minion = true;
            projectile.sentry = true;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                DoInitializationEffects();
                projectile.localAI[0] = 1f;
            }

            // Re-adjust damage as needed.
            if (Owner.MinionDamage() != projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int trueDamage = (int)(projectile.Calamity().spawnedPlayerMinionProjectileDamageValue / projectile.Calamity().spawnedPlayerMinionDamageValue * Owner.MinionDamage());
                projectile.damage = trueDamage;
            }

            projectile.velocity.Y = MathHelper.Clamp(projectile.velocity.Y + 0.4f, -12f, 12f);
            projectile.frameCounter++;

            Attacking = false;
            NPC potentialTarget = projectile.Center.MinionHoming(800f, Owner, false);
            if (potentialTarget is null)
            {
                if (projectile.frameCounter > 3)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            else
            {
                Attacking = true;
                AttackTarget(potentialTarget);
            }

            projectile.rotation = 0f;
            projectile.tileCollide = true;
            projectile.StickToTiles(false, false);
        }

        public void DoInitializationEffects()
        {
            projectile.Calamity().spawnedPlayerMinionDamageValue = Owner.MinionDamage();
            projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = projectile.damage;

            int dustQuantity = 36;
            for (int d = 0; d < dustQuantity; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 7);
                dust.scale = 1.4f;
                dust.velocity = (MathHelper.TwoPi * d / dustQuantity).ToRotationVector2() * 4f;
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public void AttackTarget(NPC target)
        {
            float horizontalDistanceFromTarget = MathHelper.Distance(projectile.Center.X, target.Center.X);
            AttackTimer++;

            // Pelt the target with acorns.
            projectile.frame = 8 + (int)(AttackTimer / 6.4) % 4;
            if (Main.myPlayer == projectile.owner && AttackTimer % 30f == 27f)
            {
                projectile.spriteDirection = (target.Center.X > projectile.Center.X).ToDirectionInt();
                Vector2 acornSpawnPosition = projectile.Center + new Vector2(projectile.spriteDirection * 6f, 10f);
                float acornShootSpeed = MathHelper.Lerp(15f, 32f, projectile.Distance(target.Center) / 800f);
                Vector2 acornShootVelocity = CalamityUtils.GetProjectilePhysicsFiringVelocity(acornSpawnPosition, target.Top + target.velocity * 25f, SquirrelSquireAcorn.Gravity, acornShootSpeed);

                if (projectile.WithinRange(target.Center, 200f))
                    acornShootVelocity = (target.Center - acornSpawnPosition).SafeNormalize(-Vector2.UnitY) * acornShootSpeed;

                Projectile.NewProjectile(acornSpawnPosition, acornShootVelocity, ModContent.ProjectileType<SquirrelSquireAcorn>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        projectile.velocity.Y = MathHelper.Clamp(projectile.velocity.Y + 0.4f, -12f, 12f);
        }

        public override bool CanDamage() => false;

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void Kill(int timeLeft)
        {
            int index = Gore.NewGore(projectile.Center, Vector2.Zero, Main.rand.Next(61, 64), projectile.scale);
            Main.gore[index].velocity *= 0.1f;
        }
    }
}
