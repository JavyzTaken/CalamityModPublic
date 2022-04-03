using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class WebBall : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Web Ball");
            Tooltip.SetDefault(@"Throws a web-covered ball that covers enemies in cobwebs to slow them down
Stealth strikes slow enemies down longer");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 20;
            Item.damage = 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 0, 30);
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<WebBallBol>();
            Item.shootSpeed = 6.5f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                if (proj.WithinBounds(Main.maxProjectiles))
                    Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(20).AddIngredient(ItemID.Cobweb, 5).Register();
        }
    }
}
