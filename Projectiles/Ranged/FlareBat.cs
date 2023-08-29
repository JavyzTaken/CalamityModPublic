﻿using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Ranged
{
    public class FlareBat : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.light = 0.25f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            int num103 = (int)Player.FindClosest(Projectile.Center, 1, 1);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < 110f && Projectile.ai[1] > 30f)
            {
                float scaleFactor2 = Projectile.velocity.Length();
                Vector2 vector11 = Main.player[num103].Center - Projectile.Center;
                vector11.Normalize();
                vector11 *= scaleFactor2;
                Projectile.velocity = (Projectile.velocity * 24f + vector11) / 25f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor2;
            }
            if (Projectile.ai[0] < 0f)
            {
                if (Projectile.velocity.Length() < 18f)
                {
                    Projectile.velocity *= 1.02f;
                }
            }
            int num192 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 0, default, 1f);
            Main.dust[num192].noGravity = true;
            Main.dust[num192].velocity *= 0.2f;
            Main.dust[num192].position = (Main.dust[num192].position + Projectile.Center) / 2f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) * Projectile.direction;

            if (Projectile.localAI[0] > 0f)
                Projectile.localAI[0]--;
            
            // Makes the bat home onto enemies after piercing once; this is just the HomeInOnNPC util without the extra update shenanigans
            if (Projectile.penetrate == 1 && Projectile.localAI[0] <= 0f)
            {
                if (!Projectile.friendly)
                    return;

                Vector2 destination = Projectile.Center;
                float maxDistance = 300;
                bool locatedTarget = false;

                // Find a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    float extraDistance = (Main.npc[i].width / 2) + (Main.npc[i].height / 2);
                    if (!Main.npc[i].CanBeChasedBy(Projectile, false) || !Projectile.WithinRange(Main.npc[i].Center, maxDistance + extraDistance))
                        continue;

                    if (Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                    {
                        destination = Main.npc[i].Center;
                        locatedTarget = true;
                        break;
                    }
                }

                if (locatedTarget)
                {
                    Vector2 homeDirection = (destination - Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Projectile.velocity = (Projectile.velocity * 20f + homeDirection * 12f) / (20f + 1f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 480);
            Projectile.localAI[0] = 10f;
            Projectile.damage /= 2;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire3, 180);
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 6, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
