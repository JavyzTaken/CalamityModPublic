﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Summon
{
    class VoidConcentrationBlackhole : ModProjectile
    {
        private int damage = 0;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(damage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            damage = reader.ReadInt32();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Hole");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80; //32 base, 2.5x the normal (for hitbox purposes)
            Projectile.height = 85; //34 base, 2.5x the normal (for hitbox purposes)
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 1800;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.scale = 0.01f;
        }

        private void ApplySucc(NPC npc)
        {
            float succStrength = 4f / Projectile.scale;
            succStrength *= Projectile.scale;
            Vector2 velocity = Projectile.Center - npc.Center;
            velocity *= 2f;
            velocity.SafeNormalize(Vector2.Zero);
            float num550 = 5f * Projectile.scale;
            Vector2 vector43 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float num551 = Projectile.Center.X - vector43.X;
            float num552 = Projectile.Center.Y - vector43.Y;
            float num553 = (float)Math.Sqrt((double)(num551 * num551 + num552 * num552));
            if (num553 < 100f)
            {
                num550 = 28f; //14
            }
            num553 = num550 / num553;
            num551 *= num553;
            num552 *= num553;
            npc.velocity.X = (velocity.X * 15f + num551) / 16f;
            npc.velocity.Y = (velocity.Y * 15f + num552) / 16f;
            npc.velocity = (velocity / succStrength);
        }

        private void Death()
        {
            if (Projectile.scale >= 2.5f) //it's boom o' clock
            {
                Projectile.height *= 2;
                Projectile.width *= 2;
                Projectile.maxPenetrate = -1;
                Projectile.penetrate = -1;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.damage = damage;
                Projectile.Damage();
                Projectile.friendly = false;
                Projectile.Damage();
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 14);
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 6; d++)
            {
                int shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, new Color(0, 0, 0), 4f);
                Main.dust[shadow].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[shadow].scale = 0.5f;
                    Main.dust[shadow].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 18; d++)
            {
                int shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, new Color(0, 0, 0), 4f);
                Main.dust[shadow].noGravity = true;
                Main.dust[shadow].velocity *= 5f;
                shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, new Color(0, 0, 0), 3f);
                Main.dust[shadow].velocity *= 2f;
            }
        }

        public override bool PreAI()
        {
            if (damage == 0)
            {
                damage = Projectile.damage;
                Projectile.damage = 0;
                Projectile.position = Main.player[Projectile.owner].position;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[Projectile.type];
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            int height = texture.Height / Main.projFrames[Projectile.type];
            int frameHeight = height * Projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameHeight, texture.Width, height);
            Vector2 origin = new Vector2(texture.Width / 2f, height / 2f);
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, drawPos, new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);
            return false;
        }

        public override void AI()
        {

            Projectile.scale += 0.01f;
            if (Projectile.scale % 0.02f == 0)
            {
                Projectile.scale += 0.01f; //speeds up the projectile's growth to prevent it being too long
            }
            int radius = (int)(Projectile.scale * 100f); //0.01f; to a max of 2.5f; so radius of 1 to 250 by default
            if (Projectile.scale >= 2f)
                radius *= 2;

            int baseWidth = 32;
            int baseHeight = 34;
            int newWidth = (int)(baseWidth * Projectile.scale);
            int newHeight = (int)(baseHeight * Projectile.scale);
            CalamityGlobalProjectile.ExpandHitboxBy(Projectile, newWidth, newHeight);



            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type] - 1)
            {
                Projectile.frame = 0;
            }
            Projectile.frameCounter++;

                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly /*&& CalamityGlobalNPC.ShouldAffectNPC(Main.npc[i])*/) //TODO - REMOVE COMMENT BEFORE MERGE
                    {
                        if (Vector2.Distance(Projectile.Center, Main.npc[i].Center) <= radius)
                            ApplySucc(Main.npc[i]);
                    }
                }


            if (Projectile.scale >= 2.5f) //it's boom o' clock
            {
                Death();
            }
        }
    }
}
