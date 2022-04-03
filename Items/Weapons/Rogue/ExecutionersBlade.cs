using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class ExecutionersBlade : RogueWeapon
    {
        private int counter = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Executioner's Blade");
            Tooltip.SetDefault("Throws a stream of homing blades\n" +
                "Stealth strikes summon a guillotine of blades on hit");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 64;
            Item.damage = 200;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 3;
            Item.useAnimation = 9;
            Item.reuseDelay = 1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.75f;
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<ExecutionersBladeProj>();
            Item.shootSpeed = 26f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Rogue/ExecutionersBladeGlow"));
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CosmiliteBar>(), 12).AddTile(ModContent.TileType<CosmicAnvil>()).Register();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            bool usingStealth = player.Calamity().StealthStrikeAvailable() && counter == 0;
            if (usingStealth)
                damage = (int)(damage * 3.61);

            int stealth = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            if (usingStealth && stealth.WithinBounds(Main.maxProjectiles))
                Main.projectile[stealth].Calamity().stealthStrike = true;

            counter++;
            if (counter >= Item.useAnimation / Item.useTime)
                counter = 0;
            return false;
        }
    }
}
