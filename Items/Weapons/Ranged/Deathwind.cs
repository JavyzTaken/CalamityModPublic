using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Deathwind : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deathwind");
            Tooltip.SetDefault("Fires a spread of 4 arrows\n" +
                "Wooden arrows are converted into nebula arrows");
        }

        public override void SetDefaults()
        {
            Item.damage = 248;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 82;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DWArrow>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/DeathwindGlow"));
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int index = 0; index < 4; ++index)
            {
                float SpeedX = speedX + Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = speedY + Main.rand.Next(-20, 21) * 0.05f;
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<DWArrow>(), damage, knockBack, player.whoAmI);
                }
                else
                {
                    int num121 = Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, (int)(damage * 0.8), knockBack, player.whoAmI);
                    Main.projectile[num121].noDropItem = true;
                }
            }
            return false;
        }
    }
}
