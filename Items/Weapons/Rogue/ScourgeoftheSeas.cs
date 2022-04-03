using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class ScourgeoftheSeas : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scourge of the Seas");
            Tooltip.SetDefault("Snaps apart into a venomous cloud upon striking an enemy\n" +
            "Stealth strikes are coated with vile toxins, afflicting enemies with a powerful debuff");
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 45;
            Item.knockBack = 3.5f;
            Item.useAnimation = Item.useTime = 20;
            Item.autoReuse = true;
            Item.Calamity().rogue = true;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<ScourgeoftheSeasProjectile>();

            Item.width = 64;
            Item.height = 66;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 36, 0, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<ScourgeoftheSeasProjectile>(), damage, knockBack, player.whoAmI, 0f, 1f);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
