using Microsoft.Xna.Framework;
//using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Systems
{
    public abstract class BaseMusicSceneEffect : ModSceneEffect
    {
		// Scene Effect Priorities
		// Setting it to SceneEffectPriority.BossLow on bosses makes Boss 1 play for half a second so don't do that
		// ??? (Anahita Lure): SceneEffectPriority.BossLow
		// Most Bosses: SceneEffectPriority.BossMedium
		// Major PML Bosses: SceneEffectPriority.BossHigh

        #region Overridable Properties
        public abstract int NPCType { get; }
        public abstract int? MusicModMusic { get; }
        public abstract int VanillaMusic { get; }
        public abstract int OtherworldMusic { get; }
        public virtual int MusicDistance => 5000;
        public virtual int[] AdditionalNPCs => new int[] { };
        #endregion

		#region Overridable Methods
        public virtual bool AdditionalCheck() => true;
		#endregion

        public virtual int SetMusic()
		{
			if (MusicModMusic is not null)
				return (int)MusicModMusic;

			return VanillaMusic;

			// You can't even edit music if the game has Otherworld toggle on lol.  Have fun with Boss 1
			/*
			FieldInfo swapMusicField = typeof(Main).GetField("swapMusic", BindingFlags.Static | BindingFlags.NonPublic);
			bool musicSwapped = (bool)swapMusicField.GetValue(null);
			bool playingOtherworld = (!Main.drunkWorld && musicSwapped) || (Main.drunkWorld && !musicSwapped);
			// Main.swapMusic is private.  Todo: bug tmod devs to avoid reflection
			return playingOtherworld ? OtherworldMusic : VanillaMusic;
			*/
		}

        public virtual bool SetSceneEffect(Player player)
		{
			if (!AdditionalCheck())
				return false;

			if (MusicModMusic is null && VanillaMusic == -1)
				return false;

			/*
			// You can't even edit music if the game has Otherworld toggle on lol.
			// Theoretically, reflection would only occur if the music mod is disabled
			if (MusicModMusic is null)
			{
				FieldInfo swapMusicField = typeof(Main).GetField("swapMusic", BindingFlags.Static | BindingFlags.NonPublic);
				bool musicSwapped = (bool)swapMusicField.GetValue(null);
				bool playingOtherworld = (!Main.drunkWorld && musicSwapped) || (Main.drunkWorld && !musicSwapped);
				if (VanillaMusic == -1 && !playingOtherworld)
					return false;
				if (OtherworldMusic == -1 && playingOtherworld)
					return false;
			}
			*/

			for (int j = 0; j < Main.maxNPCs; j++)
			{
				NPC npc = Main.npc[j];
				if (!npc.active)
					continue;

				bool inList = false;
				if (npc.type == NPCType)
					inList = true;
				else
				{
					for (int i = 0; i < AdditionalNPCs.Length; i++)
					{
						if (npc.type == AdditionalNPCs[i])
						{
							inList = true;
							break;
						}
					}
				}
				if (!inList)
					continue;

				Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
				Rectangle npcBox = new Rectangle((int)npc.Center.X - MusicDistance, (int)npc.Center.Y - MusicDistance, MusicDistance * 2, MusicDistance * 2);
				if (screenRect.Intersects(npcBox))
					return true;
			}
			return false;
		}

        public override int Music => SetMusic();

        public override bool IsSceneEffectActive(Player player) => SetSceneEffect(player);
    }
}
