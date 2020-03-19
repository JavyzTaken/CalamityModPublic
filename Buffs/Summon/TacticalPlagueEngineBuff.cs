using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Buffs.Summon
{
    public class TacticalPlagueEngineBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tactical Plague Engine");
            Description.SetDefault("A giant plague jet is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<TacticalPlagueEngineSummon>()] > 0)
            {
                modPlayer.plagueEngine = true;
            }
            if (!modPlayer.plagueEngine)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
