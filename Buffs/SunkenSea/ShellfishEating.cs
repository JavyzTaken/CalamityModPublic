﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;

namespace CalamityMod.Buffs.SunkenSea
{
    public class ShellfishEating : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shellfish Claps");
			Description.SetDefault("Clamfest");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.Calamity().shellfishVore = true;
		}
	}
}
