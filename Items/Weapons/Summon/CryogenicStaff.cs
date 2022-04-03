using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class CryogenicStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryogenic Staff");
            Tooltip.SetDefault(@"Summons an animated ice construct to protect you
Fire rate and range increase the longer it targets an enemy");
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.width = 82;
            Item.height = 84;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;

            Item.value = CalamityGlobalItem.Rarity6BuyPrice;
            Item.rare = ItemRarityID.LightPurple;
            Item.Calamity().devItem = true;

            Item.UseSound = SoundID.Item78;
            Item.shoot = ModContent.ProjectileType<IceSentry>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
            player.UpdateMaxTurrets();
            return false;
        }
    }
}
