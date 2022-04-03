using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class SunGodStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun God Staff");
            Tooltip.SetDefault("Summons a solar god spirit to protect you\n" +
                "There can only be one spirit");
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.mana = 10;
            Item.width = 72;
            Item.height = 72;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.25f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<SolarGod>();
            Item.DamageType = DamageClass.Summon;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SunSpiritStaff>()).AddIngredient(ModContent.ItemType<EssenceofCinder>(), 5).AddIngredient(ItemID.SoulofMight, 3).AddIngredient(ItemID.SoulofSight, 3).AddIngredient(ItemID.SoulofFright, 3).AddTile(TileID.MythrilAnvil).Register();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            CalamityUtils.KillShootProjectiles(true, type, player);
            Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}
