using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class DeificThunderbolt : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deific Thunderbolt");
            Tooltip.SetDefault(@"Fires a lightning bolt to electrocute enemies
The lightning bolt travels faster while it is raining
Summons lightning from the sky on impact
Stealth strikes summon more lightning and travel faster");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 999;
            item.knockBack = 10f;
            item.crit += 8;

            item.width = 56;
            item.height = 56;
            item.useStyle = 1;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.value = Item.buyPrice(1, 40, 0, 0);
            item.useTime = 21;
            item.useAnimation = 21;
            item.rare = 10;
            item.Calamity().postMoonLordRarity = 13;
            item.Calamity().rogue = true;

            item.autoReuse = true;
            item.shootSpeed = 13.69f;
            item.shoot = ModContent.ProjectileType<DeificThunderboltProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			float stealthSpeedMult = 1f;
			if (player.Calamity().StealthStrikeAvailable())
				stealthSpeedMult = 1.5f;
			float rainSpeedMult = 1f;
			if (Main.raining)
				rainSpeedMult = 1.5f;

			int thunder = Projectile.NewProjectile(position.X, position.Y, speedX * rainSpeedMult * stealthSpeedMult, speedY * rainSpeedMult * stealthSpeedMult, type, damage, knockBack, player.whoAmI, 0f, 0f);
			if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
			{
				Main.projectile[thunder].Calamity().stealthStrike = true;
			}
            return false;
        }

        public override void AddRecipes()
        {

            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 8);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 15);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
