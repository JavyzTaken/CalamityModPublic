﻿using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Melee
{
    public class FallenPaladinsHammerEcho : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public static readonly SoundStyle SlamHamSound = new("CalamityMod/Sounds/Item/FallenPaladinsHammerBigImpact") { Volume = 1f};
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        //This is all copied straight from PwnagehammerEcho with some minor edits.
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 48, 48, 56);
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] < 42f)
            {
                Projectile.velocity *= 0.9575f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0] * 0.5f) * Projectile.localAI[0];
            }
            else if (Projectile.ai[0] < 44f)
            {
                Projectile.extraUpdates = 1;
                if (Projectile.ai[1] < 0f)
                {
                    Projectile.Kill();
                    return;
                }

                NPC target = Main.npc[(int)Projectile.ai[1]];
                if (!target.CanBeChasedBy(Projectile, false) || !target.active)
                    Projectile.Kill();
                else
                {
                    float velConst = 12f;
                    Projectile.velocity = new Vector2((target.Center.X - Projectile.Center.X) / velConst, (target.Center.Y - Projectile.Center.Y) / velConst);
                    Projectile.rotation += MathHelper.ToRadians(48f) * Projectile.localAI[0];
                }
            }

            if (Main.rand.NextBool(2))
            {
                Vector2 offset = new Vector2(7, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(3, 0).RotatedBy(offset.ToRotation());
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.position.X, Projectile.position.Y) + offset, DustID.RedTorch, new Vector2(Projectile.velocity.X * 0.2f + velOffset.X, Projectile.velocity.Y * 0.2f + velOffset.Y), 100, new Color(255, 245, 198), 2f);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(7, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(3, 0).RotatedBy(offset.ToRotation());
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.position.X, Projectile.position.Y) + offset, DustID.RedTorch, new Vector2(Projectile.velocity.X * 0.2f + velOffset.X, Projectile.velocity.Y * 0.2f + velOffset.Y), 100, new Color(255, 245, 198), 2f);
                dust.noGravity = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] <= 42f)
                return false;
            return null;
        }

        public override bool CanHitPvp(Player target) => Projectile.ai[0] > 42f;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] = target.whoAmI;
        }

        public override bool PreKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            //This is what we call fucking IMPACT.
            SoundEngine.PlaySound(SlamHamSound, Projectile.Center);
            Main.player[Projectile.owner].Calamity().GeneralScreenShakePower = 5;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FallenBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            float distance = 248f;

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                Vector2 vec = npc.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
                if (distanceTo < distance && npc.CanBeChasedBy(Projectile, false) && k != Projectile.localAI[0])
                {
                    float alldamage = Projectile.damage * 0.5f;
                    double damage = npc.StrikeNPC(npc.CalculateHitInfo((int)alldamage, Projectile.velocity.X > 0f ? 1 : -1, true, Projectile.knockBack));
                    player.addDPS((int)damage);
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float rot = MathHelper.ToRadians(22.5f) * Math.Sign(Projectile.velocity.X);
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle?(), color, Projectile.rotation - i * rot, origin, Projectile.scale, spriteEffects, 0);
            }
            return false;
        }
    }
}