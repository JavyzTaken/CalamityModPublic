using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class Corrocloud3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrocloud");
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.99f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] % 7f == 6f)
            {
                Projectile.frame++;
            }
            if (Projectile.frame >= 8)
                Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.SulfurousSeaAcid);
            }
        }
    }
}
