using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.CalPlayer;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class PoisonPack : RogueWeapon
    {
        private static int baseDamage = 20;
        private static float baseKnockback = 1.8f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poison Pack");
            Tooltip.SetDefault("Throws a poisonous spiky ball. Stacks up to 3.\n" +
                "Stealth strikes cause the balls to release spore clouds\n" +
                "Right click to delete all existing spiky balls");
        }

        public override void SafeSetDefaults()
        {
            Item.damage = baseDamage;
            Item.Calamity().rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 14;
            Item.height = 14;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = baseKnockback;
            Item.value = Item.buyPrice(0, 0, 33, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 3;

            Item.shootSpeed = 7f;
            Item.shoot = ModContent.ProjectileType<PoisonBol>();
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void GetWeaponCrit(Player player, ref int crit) => crit += 4;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileID.None;
                Item.shootSpeed = 0f;
                return player.ownedProjectileCounts[ModContent.ProjectileType<PoisonBol>()] > 0;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<PoisonBol>();
                Item.shootSpeed = 7f;
                int UseMax = Item.stack;
                return player.ownedProjectileCounts[ModContent.ProjectileType<PoisonBol>()] < UseMax;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.killSpikyBalls = false;
            if (modPlayer.StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.killSpikyBalls = true;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SpikyBall, 50).AddIngredient(ItemID.JungleSpores, 10).AddTile(TileID.Anvils).Register();
        }
    }
}
