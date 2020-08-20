using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.HiveMind
{
    public class DarkHeart : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Heart");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.damage = 0;
            npc.width = 32;
            npc.height = 32;
            npc.defense = 2;
            npc.lifeMax = 150;
            if (CalamityWorld.bossRushActive)
            {
                npc.lifeMax = 18000;
            }
            npc.aiStyle = -1;
            aiType = -1;
            npc.knockBackResist = CalamityWorld.bossRushActive ? 0f : 0.4f;
            npc.noGravity = true;
            npc.canGhostHeal = false;
            npc.chaseable = false;
            npc.HitSound = SoundID.NPCHit13;
            npc.DeathSound = SoundID.NPCDeath21;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            bool revenge = CalamityWorld.revenge;
            npc.TargetClosest(true);
            float num1164 = revenge ? 4.5f : 4f;
            float num1165 = revenge ? 0.8f : 0.75f;
            if (CalamityWorld.bossRushActive)
            {
                num1164 *= 2f;
                num1165 *= 2f;
            }

            Vector2 vector133 = new Vector2(npc.Center.X, npc.Center.Y);
            float num1166 = Main.player[npc.target].Center.X - vector133.X;
            float num1167 = Main.player[npc.target].Center.Y - vector133.Y - 400f;
            float num1168 = (float)Math.Sqrt((double)(num1166 * num1166 + num1167 * num1167));
            if (num1168 < 20f)
            {
                num1166 = npc.velocity.X;
                num1167 = npc.velocity.Y;
            }
            else
            {
                num1168 = num1164 / num1168;
                num1166 *= num1168;
                num1167 *= num1168;
            }
            if (npc.velocity.X < num1166)
            {
                npc.velocity.X = npc.velocity.X + num1165;
                if (npc.velocity.X < 0f && num1166 > 0f)
                {
                    npc.velocity.X = npc.velocity.X + num1165 * 2f;
                }
            }
            else if (npc.velocity.X > num1166)
            {
                npc.velocity.X = npc.velocity.X - num1165;
                if (npc.velocity.X > 0f && num1166 < 0f)
                {
                    npc.velocity.X = npc.velocity.X - num1165 * 2f;
                }
            }
            if (npc.velocity.Y < num1167)
            {
                npc.velocity.Y = npc.velocity.Y + num1165;
                if (npc.velocity.Y < 0f && num1167 > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y + num1165 * 2f;
                }
            }
            else if (npc.velocity.Y > num1167)
            {
                npc.velocity.Y = npc.velocity.Y - num1165;
                if (npc.velocity.Y > 0f && num1167 < 0f)
                {
                    npc.velocity.Y = npc.velocity.Y - num1165 * 2f;
                }
            }
            if (npc.position.X + (float)npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + (float)Main.player[npc.target].width && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[0] += 1f;
                if (npc.ai[0] >= 30f)
                {
                    npc.ai[0] = 0f;
                    int num1169 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
                    int num1170 = (int)(npc.position.Y + (float)npc.height + 4f);
                    int num184 = Main.expertMode ? 14 : 18;
                    Projectile.NewProjectile((float)num1169, (float)num1170, 0f, 4f, ModContent.ProjectileType<ShaderainHostile>(), num184, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 14, hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 14, hitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
