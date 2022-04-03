using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Rogue
{
    public class KelvinCatalystBoomerang : ModProjectile
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/KelvinCatalyst";
        public int AIState = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelvin Catalyst");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.Calamity().rogue = true;
            Projectile.coldDamage = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(AIState);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            AIState = reader.ReadInt32();
        }

        public override void AI()
        {
            VisualAudioEffects();
            BoomerangAI();
        }

        private void BoomerangAI()
        {
            switch (AIState)
            {
                case 0:
                    Projectile.localAI[0] += 1f;
                    if (Projectile.localAI[0] >= 75f)
                    {
                        ResetStats(Projectile.Calamity().stealthStrike);
                    }
                    break;
                case 1:
                    ReturnToPlayer();
                    break;
                case 2:
                    // Will target the targetted NPC that minions use btw
                    Projectile.ChargingMinionAI(1200f, 1500f, 2200f, 150f, 0, 40f, 9f, 4f, new Vector2(0f, -60f), 40f, 9f, true, true);
                    Projectile.localAI[0] += 1f;
                    if (Projectile.localAI[0] >= 120)
                    {
                        ResetStats(false);
                    }
                    break;
            }
        }

        private void ReturnToPlayer()
        {
            Player player = Main.player[Projectile.owner];
            float returnSpeed = 20f;
            float acceleration = 2f;
            Vector2 playerVec = player.Center - Projectile.Center;
            float dist = playerVec.Length();

            // Delete the projectile if it's excessively far away.
            if (dist > 3000f)
                Projectile.Kill();

            playerVec.Normalize();
            playerVec *= returnSpeed;

            // Home back in on the player.
            if (Projectile.velocity.X < playerVec.X)
            {
                Projectile.velocity.X += acceleration;
                if (Projectile.velocity.X < 0f && playerVec.X > 0f)
                    Projectile.velocity.X += acceleration;
            }
            else if (Projectile.velocity.X > playerVec.X)
            {
                Projectile.velocity.X -= acceleration;
                if (Projectile.velocity.X > 0f && playerVec.X < 0f)
                    Projectile.velocity.X -= acceleration;
            }
            if (Projectile.velocity.Y < playerVec.Y)
            {
                Projectile.velocity.Y += acceleration;
                if (Projectile.velocity.Y < 0f && playerVec.Y > 0f)
                    Projectile.velocity.Y += acceleration;
            }
            else if (Projectile.velocity.Y > playerVec.Y)
            {
                Projectile.velocity.Y -= acceleration;
                if (Projectile.velocity.Y > 0f && playerVec.Y < 0f)
                    Projectile.velocity.Y -= acceleration;
            }
            if (Main.myPlayer == Projectile.owner)
                if (Projectile.Hitbox.Intersects(player.Hitbox))
                    Projectile.Kill();
        }

        private void VisualAudioEffects()
        {
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.3f / 255f, Main.DiscoR * 0.4f / 255f, Main.DiscoR * 0.5f / 255f);

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67, 0f, 0f, 100, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;

            Projectile.rotation += 0.25f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ResetStats(Projectile.Calamity().stealthStrike);
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }

        private void ResetStats(bool chaseEnemies)
        {
            AIState = chaseEnemies ? 2 : 1;
            Projectile.localAI[0] = 0f;
            Projectile.width = Projectile.height = 60;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            OnHitEffects();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            OnHitEffects();
        }

        private void OnHitEffects()
        {
            int projType = ModContent.ProjectileType<KelvinCatalystStar>();
            if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].ownedProjectileCounts[projType] < 25)
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                    Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 4f), (float)(Math.Cos(offsetAngle) * 4f), projType, Projectile.damage / 6, Projectile.knockBack * 0.5f, Projectile.owner);

                    Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 4f), (float)(-Math.Cos(offsetAngle) * 4f), projType, Projectile.damage / 6, Projectile.knockBack * 0.5f, Projectile.owner);
                }
            }
            SoundEngine.PlaySound(SoundID.Item30, Projectile.position);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
