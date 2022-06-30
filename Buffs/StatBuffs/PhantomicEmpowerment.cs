﻿using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Buffs.StatBuffs
{
    class PhantomicEmpowerment : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantomic Empowerment");
            Description.SetDefault("Empowering minion damage by 10%");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<SummonDamageClass>() += 0.1f;
        }
    }
}