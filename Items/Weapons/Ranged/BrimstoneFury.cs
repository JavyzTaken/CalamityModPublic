using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class BrimstoneFury : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone Fury");
            Tooltip.SetDefault("Converts wooden arrows into spreads of 3 brimstone bolts");
        }

        public override void SetDefaults()
        {
            Item.damage = 39;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 58;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.75f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BrimstoneBolt>();
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numProj = 2;
            float rotation = MathHelper.ToRadians(2);
            for (int i = 0; i < numProj + 1; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));

                if (type == ProjectileID.WoodenArrowFriendly)
                    Projectile.NewProjectile(position, perturbedSpeed, ModContent.ProjectileType<BrimstoneBolt>(), damage, knockBack, player.whoAmI);
                else
                {
                    int proj = Projectile.NewProjectile(position, perturbedSpeed, type, damage, knockBack, player.whoAmI);
                    Main.projectile[proj].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<UnholyCore>(), 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
