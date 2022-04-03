﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Melee
{
    public class SanguineFuryWheel : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Melee/TrueBiomeBlade_SanguineFuryExtra";
        private bool initialized = false;
        Vector2 direction = Vector2.Zero;
        public ref float Shred => ref Projectile.ai[0];
        public float ShredRatio => MathHelper.Clamp(Shred / (SanguineFury.maxShred * 0.5f), 0f, 1f);
        public Player Owner => Main.player[Projectile.owner];

        public float Timer => MaxTime - Projectile.timeLeft;

        public const float MaxTime = 120;



        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine Bladewheel");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 45;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;


            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.95f;
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            float bladeLenght = 84 * Projectile.scale;
            float bladeWidth = 76 * Projectile.scale;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - direction * bladeLenght / 2, Projectile.Center + direction * bladeLenght / 2, bladeWidth, ref collisionPoint);
        }

        public override void AI()
        {
            if (!initialized)
            {
                SoundEngine.PlaySound(SoundID.Item90, Projectile.Center);
                initialized = true;

                direction = Owner.SafeDirectionTo(Owner.Calamity().mouseWorld, Vector2.Zero);
                direction.Normalize();
                Projectile.rotation = direction.ToRotation();

                Projectile.timeLeft = (int)MaxTime;
                Projectile.velocity = direction * 16f;

                Projectile.scale = 1f + ShredRatio; //SWAGGER
                Projectile.netUpdate = true;

            }

            Projectile.velocity *= 0.96f;
            Projectile.position += Projectile.velocity;

        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Owner.HeldItem.modItem is OmegaBiomeBlade sword && Main.rand.NextFloat() <= OmegaBiomeBlade.SuperPogoAttunement_WheelProc)
                sword.OnHitProc = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++) //Draw extra copies
            {
                var tex = GetTexture("CalamityMod/Projectiles/Melee/TrueBiomeBlade_SanguineFuryExtra");

                float drawAngleWheel = MathHelper.WrapAngle(i * MathHelper.PiOver2 + (Main.GlobalTime * 6));
                Vector2 drawOrigin = new Vector2(0f, tex.Height);
                Vector2 drawPositionWheel = Projectile.Center - drawAngleWheel.ToRotationVector2() * 20f - Main.screenPosition;

                //Vector2 drawPosition = Vector2.Lerp(drawPositionShot, drawPositionWheel, transition);

                float opacityFade = Projectile.timeLeft > 15 ? 1 : Projectile.timeLeft / 15f;

                spriteBatch.Draw(tex, drawPositionWheel, null, Color.Lerp(Color.White, lightColor, 0.5f) * 0.8f * opacityFade, drawAngleWheel, drawOrigin, Projectile.scale * 0.85f, 0f, 0f);
            }

            //Back to normal
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Particle Sparkle = new CritSpark(Projectile.Center, Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat(17.5f, 25f) * Projectile.scale, Color.White, Main.rand.NextBool() ? Color.Crimson : Color.DarkRed, 0.4f + Main.rand.NextFloat(0f, 3.5f), 20 + Main.rand.Next(30), 1, 3f);
                GeneralParticleHandler.SpawnParticle(Sparkle);
            }
        }

    }
}
