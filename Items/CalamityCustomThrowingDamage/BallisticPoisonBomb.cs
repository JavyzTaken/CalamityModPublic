using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.CalamityCustomThrowingDamage
{
    public class BallisticPoisonBomb : CalamityDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ballistic Poison Bomb");
			Tooltip.SetDefault("Throws a sticky bomb that explodes into spikes and poison clouds");
		}

		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.damage = 55;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.useTime = 22;
			item.knockBack = 6.5f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.height = 38;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = 7;
			item.shoot = mod.ProjectileType("BallisticPoisonBomb");
			item.shootSpeed = 12f;
			item.Calamity().rogue = true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthCells", 10);
            recipe.AddIngredient(null, "SulphurousSand", 20);
            recipe.AddIngredient(null, "Tenebris", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
