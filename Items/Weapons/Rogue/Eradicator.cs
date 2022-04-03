using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class Eradicator : RogueWeapon
    {
        public static float Speed = 10.5f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eradicator");
            Tooltip.SetDefault("Throws a disk that fires lasers at nearby enemies\n" +
            "Stealth strikes stick to enemies and unleash a barrage of lasers in all directions");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.damage = 563;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.height = 54;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.shoot = ModContent.ProjectileType<EradicatorProjectile>();
            Item.shootSpeed = Speed;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.Calamity().StealthStrikeAvailable())
                damage = (int)(damage * 0.5f);

            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && proj.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[proj].timeLeft += EradicatorProjectile.StealthExtraLifetime;
                Main.projectile[proj].Calamity().stealthStrike = true;
            }
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Rogue/EradicatorGlow"));
        }
    }
}
