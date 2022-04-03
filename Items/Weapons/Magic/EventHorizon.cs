using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class EventHorizon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Event Horizon");
            Tooltip.SetDefault("Nothing, not even light, can return.\n" +
            "Fires a ring of stars to home in on nearby enemies\n" +
            "Stars spawn black holes on enemy hits");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 46;

            Item.damage = 275;
            Item.knockBack = 3.5f;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;

            Item.useTime = Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;

            Item.UseSound = SoundID.Item84;
            Item.shoot = ModContent.ProjectileType<EventHorizonStar>();
            Item.shootSpeed = 25f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (float i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i;
                Projectile.NewProjectile(player.Center, angle.ToRotationVector2() * 8f, type, damage, knockBack, player.whoAmI, angle, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Starfall>()).AddIngredient(ModContent.ItemType<NuclearFury>()).AddIngredient(ModContent.ItemType<RelicofRuin>()).AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8).AddIngredient(ModContent.ItemType<DarksunFragment>(), 8).AddTile(TileID.Bookcases).Register();
        }
    }
}
