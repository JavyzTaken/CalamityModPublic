using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class ForsakenSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forsaken Saber");
            Tooltip.SetDefault("Shoots three sand blades that alter their velocity as they travel");
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 56;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<SandBlade>();
            Item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int projectiles = 0; projectiles < 3; projectiles++)
            {
                float SpeedX = speedX + Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = speedY + Main.rand.Next(-40, 41) * 0.05f;
                Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, (int)(damage * 0.8), knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddRecipeGroup("AnyAdamantiteBar", 5).AddIngredient(ItemID.AncientBattleArmorMaterial, 2).AddTile(TileID.MythrilAnvil).Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 159);
            }
        }
    }
}
