﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class StellarKnifeProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellar Knife");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
			//synthesized timeLeft
			projectile.localAI[1]++;
			if (projectile.localAI[1] > 600f)
				projectile.Kill();

            if (projectile.ai[0] == 1f)
            {
                projectile.ai[0] = 0f;
                projectile.damage = (int)((double) projectile.damage * (projectile.ai[1] == 1f ? 0.9f : 0.75f));
                projectile.ai[1] = 0f;
            }
            projectile.ai[1] += 0.75f;
            if (projectile.ai[1] <= 60f)
            {
                projectile.rotation += 1f;
                projectile.velocity.X *= 0.985f;
                projectile.velocity.Y *= 0.985f;
            }
            else
            {
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 2.355f;
                if (projectile.spriteDirection == -1)
                {
                    projectile.rotation -= 1.57f;
                }
				projectile.localAI[0]++;

				Vector2 center = projectile.Center;
				float maxDistance = 600f;
				bool homeIn = false;

				if (projectile.localAI[0] >= 20f && !homeIn) //shorten knife lifespan if it hasn't found a target
					if (projectile.timeLeft >= 60)
						projectile.timeLeft = 60;

				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
					{
						float extraDistance = (float)(Main.npc[i].width / 2) + (float)(Main.npc[i].height / 2);

						if (Vector2.Distance(Main.npc[i].Center, projectile.Center) < (maxDistance + extraDistance))
						{
							center = Main.npc[i].Center;
							homeIn = true;
							break;
						}
					}
				}

				if (homeIn)
				{
					projectile.timeLeft = 600; //when homing in, the projectile cannot run out of timeLeft, but synthesized timeLeft still runs

					Vector2 homeInVector = projectile.DirectionTo(center);
					if (homeInVector.HasNaNs())
						homeInVector = Vector2.UnitY;

					projectile.velocity = (projectile.velocity * 10f + homeInVector * 30f) / (10f + 1f);
				}
                else
                {
                    projectile.velocity.X *= 0.92f;
                    projectile.velocity.Y *= 0.92f;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(projectile, lightColor, ProjectileID.Sets.TrailingMode[projectile.type], 2);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModContent.DustType<AstralBlue>(), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
