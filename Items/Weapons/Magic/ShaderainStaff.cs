using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class ShaderainStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shaderain Staff");
            Tooltip.SetDefault("Fires a shade storm cloud that inflicts shadowflame");
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item66;
            Item.shoot = ModContent.ProjectileType<ShadeNimbus>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float num72 = Item.shootSpeed;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (player.gravDir == -1f)
            {
                num79 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
            }
            float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
            if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
            {
                num78 = (float)player.direction;
                num79 = 0f;
                num80 = num72;
            }
            else
            {
                num80 = num72 / num80;
            }
            num78 *= num80;
            num79 *= num80;
            int num154 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, ModContent.ProjectileType<ShadeNimbusCloud>(), damage, knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[num154].ai[0] = (float)Main.mouseX + Main.screenPosition.X;
            Main.projectile[num154].ai[1] = (float)Main.mouseY + Main.screenPosition.Y;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.RottenChunk, 2).AddIngredient(ItemID.DemoniteBar, 3).AddIngredient(ModContent.ItemType<TrueShadowScale>(), 12).AddTile(TileID.DemonAltar).Register();
        }
    }
}
