using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.NPCs.Providence;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Boss
{
    public class HolyFlare : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Flare");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 50;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 840;
            cooldownSlot = 1;
            Projectile.Calamity().affectedByMaliceModeVelocityMultiplier = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            float velocityYCap = Projectile.position.Y + Projectile.height < player.position.Y ? 1f : 0.25f;
            Projectile.velocity.Y += 0.01f;
            if (Projectile.velocity.Y > velocityYCap)
                Projectile.velocity.Y = velocityYCap;

            float velocityX = (!Main.dayTime || CalamityWorld.malice || BossRushEvent.BossRushActive) ? 0.025f : 0.02f;
            if (Projectile.position.X + Projectile.width < player.position.X)
            {
                if (Projectile.velocity.X < 0f)
                    Projectile.velocity.X *= 0.99f;
                Projectile.velocity.X += velocityX;
            }
            else if (Projectile.position.X > player.position.X + player.width)
            {
                if (Projectile.velocity.X > 0f)
                    Projectile.velocity.X *= 0.99f;
                Projectile.velocity.X -= velocityX;
            }

            float velocityXCap = (!Main.dayTime || CalamityWorld.malice || BossRushEvent.BossRushActive) ? 10f : 8f;
            if (Projectile.velocity.X > velocityXCap || Projectile.velocity.X < -velocityXCap)
                Projectile.velocity.X *= 0.97f;

            Projectile.rotation = Projectile.velocity.X * 0.025f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return ((Main.dayTime && !CalamityWorld.malice) || !NPC.AnyNPCs(ModContent.NPCType<Providence>())) ? new Color(250, 150, 0, Projectile.alpha) : new Color(100, 200, 250, Projectile.alpha);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = ((Main.dayTime && !CalamityWorld.malice) || !NPC.AnyNPCs(ModContent.NPCType<Providence>())) ? Main.projectileTexture[Projectile.type] : ModContent.Request<Texture2D>("CalamityMod/Projectiles/Boss/HolyFlareNight");
            int num214 = Main.projectileTexture[Projectile.type].Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
            Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
            int dustType = ((Main.dayTime && !CalamityWorld.malice) || !NPC.AnyNPCs(ModContent.NPCType<Providence>())) ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;
            for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 10; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            int buffType = ((Main.dayTime && !CalamityWorld.malice) || !NPC.AnyNPCs(ModContent.NPCType<Providence>())) ? ModContent.BuffType<HolyFlames>() : ModContent.BuffType<Nightwither>();
            target.AddBuff(buffType, 120);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.Calamity().lastProjectileHit = projectile;
        }
    }
}
