using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.UI
{
	public class ModeIndicatorUI
	{
		public static readonly Vector2 OffsetToAreaCenter = new Vector2(0f, 4f);
		public static Vector2 DifficultyIconOffset => new Vector2(0f, -13f);
		public static Vector2 ArmageddonIconOffset => new Vector2(12f, 7f);
		public static Vector2 MaliceIconOffset => new Vector2(-12f, 7f);
		public static void Draw(SpriteBatch spriteBatch)
		{
			// The mode indicator should only be displayed when the inventory is open, to prevent obstruction.
			if (!Main.playerInventory)
				return;

			Texture2D outerAreaTexture = ModContent.GetTexture("CalamityMod/ExtraTextures/UI/ModeIndicatorArea");
			Texture2D revengeanceIconTexture = ModContent.GetTexture("CalamityMod/ExtraTextures/UI/ModeIndicatorRev");
			Texture2D deathIconTexture = ModContent.GetTexture("CalamityMod/ExtraTextures/UI/ModeIndicatorDeath");
			Texture2D armageddonIconTexture = ModContent.GetTexture("CalamityMod/ExtraTextures/UI/ModeIndicatorArma");
			Texture2D maliceIconTexture = ModContent.GetTexture("CalamityMod/ExtraTextures/UI/ModeIndicatorMalice");

			Vector2 drawCenter = new Vector2(Main.screenWidth - 400f, 72f) + outerAreaTexture.Size() * 0.5f;

			spriteBatch.Draw(outerAreaTexture, drawCenter, null, Color.White, 0f, outerAreaTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

			if (CalamityWorld.revenge)
			{
				Texture2D difficultyIconToUse = CalamityWorld.death ? deathIconTexture : revengeanceIconTexture;
				spriteBatch.Draw(difficultyIconToUse, drawCenter + DifficultyIconOffset, null, Color.White, 0f, difficultyIconToUse.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			}

			if (CalamityWorld.armageddon)
				spriteBatch.Draw(armageddonIconTexture, drawCenter + ArmageddonIconOffset, null, Color.White, 0f, armageddonIconTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

			if (CalamityWorld.malice)
				spriteBatch.Draw(maliceIconTexture, drawCenter + MaliceIconOffset, null, Color.White, 0f, maliceIconTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
		}
	}
}
