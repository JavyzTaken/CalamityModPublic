using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class MidnightSunBeacon : ModItem
    {
        public const float MachineGunRate = 18f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Midnight Sun Beacon");
            Tooltip.SetDefault("Summons a UFO to vaporize enemies");
        }

        public override void SetDefaults()
        {
            Item.damage = 191;
            Item.mana = 10;
            Item.width = Item.height = 32;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item90;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MidnightSunBeaconProj>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.XenoStaff).AddIngredient(ItemID.MoonlordTurretStaff).AddIngredient(ModContent.ItemType<AuricBar>(), 5).AddTile(ModContent.TileType<CosmicAnvil>()).Register();
        }
    }
}
