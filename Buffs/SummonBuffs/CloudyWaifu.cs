﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;

namespace CalamityMod.Buffs.SummonBuffs
{
	public class CloudyWaifu : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cloud Elemental");
			Description.SetDefault("The cloud elemental will protect you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			CalamityPlayer modPlayer = player.Calamity();
			if (player.ownedProjectileCounts[mod.ProjectileType("CloudyWaifu")] > 0)
			{
				modPlayer.cWaifu = true;
			}
			if (!modPlayer.cWaifu)
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
