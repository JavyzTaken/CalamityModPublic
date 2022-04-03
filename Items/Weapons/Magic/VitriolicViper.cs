using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class VitriolicViper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitriolic Viper");
            Tooltip.SetDefault("Releases a volley of venomous fangs and spit");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 93;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 60;
            Item.height = 62;
            Item.useTime = Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item46;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VitriolicViperSpit>();
            Item.shootSpeed = 20f;

            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = -4; i <= 4; i += 1)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(i));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 1.65f, perturbedSpeed.Y * 1.65f, type, (int)(damage * 0.7f), knockBack * 0.7f, player.whoAmI, 0f, 0f);
            }
            for (int j = -2; j <= 2; j += 1)
            {
                Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(j));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, ModContent.ProjectileType<VitriolicViperFang>(), damage, knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
