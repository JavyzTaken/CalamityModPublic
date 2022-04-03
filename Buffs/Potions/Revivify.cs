using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Buffs.Potions
{
    public class Revivify : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Revivify");
            Description.SetDefault("You are healed by a fraction of the damage you take");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().revivify = true;
        }
    }
}
