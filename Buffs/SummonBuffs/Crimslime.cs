﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;

namespace CalamityMod.Buffs.SummonBuffs
{
	public class Crimslime : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Crimslime");
			Description.SetDefault("The crimslime will protect you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			CalamityPlayer modPlayer = player.Calamity();
			if (player.ownedProjectileCounts[mod.ProjectileType("Crimslime")] > 0)
			{
				modPlayer.cSlime2 = true;
			}
			if (!modPlayer.cSlime2)
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
