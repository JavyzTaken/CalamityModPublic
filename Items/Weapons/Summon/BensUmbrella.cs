using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class BensUmbrella : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Temporal Umbrella");
            Tooltip.SetDefault("Surprisingly sturdy, I reckon this could defeat the Mafia in a single blow\n" +
                "Summons a magic hat to hover above your head\n" +
                "The hat will release a variety of objects to assault your foes\n" +
                "Requires 5 minion slots to use and there can only be one hat");
        }

        public override void SetDefaults()
        {
            Item.damage = 193;
            Item.knockBack = 1f;
            Item.mana = 99;
            Item.useTime = Item.useAnimation = 10;
            Item.DamageType = DamageClass.Summon;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<MagicHat>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 74;
            Item.height = 72;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item68;
            Item.value = CalamityGlobalItem.Rarity16BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.HotPink;
            Item.Calamity().devItem = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && player.maxMinions >= 5;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            CalamityUtils.KillShootProjectiles(true, type, player);
            Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SpikecragStaff>()).AddIngredient(ModContent.ItemType<SarosPossession>()).AddIngredient(ItemID.Umbrella).AddIngredient(ItemID.TopHat).AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5).AddTile(ModContent.TileType<DraedonsForge>()).Register();
        }
    }
}
