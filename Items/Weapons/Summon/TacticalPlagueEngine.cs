using CalamityMod.Projectiles.Summon;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class TacticalPlagueEngine : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tactical Plague Engine");
            Tooltip.SetDefault("Summons a plague jet to pummel your enemies into submission\n" +
                               "Consumes bullets\n" +
                               "Sometimes shoots a missile instead of a bullet");
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.mana = 10;
            item.width = 28;
            item.height = 20;
            item.useTime = item.useAnimation = 15;
            item.useStyle = 4;
            item.noMelee = true;
            item.knockBack = 0.5f;
            item.value = Item.buyPrice(1, 20, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item14;
            item.autoReuse = true;
            item.summon = true;
            item.shoot = ModContent.ProjectileType<TacticalPlagueEngineSummon>();
            item.shootSpeed = 16f;
            item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BlackHawkRemote>());
            recipe.AddIngredient(ModContent.ItemType<InfectedRemote>());
            recipe.AddIngredient(ModContent.ItemType<FuelCellBundle>());
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 15);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
