using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class AuroraBlazer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aurora Blazer");
            Tooltip.SetDefault("Spews astral flames that travel in a star-shaped patterns\n" +
            "60% chance to not consume gel");
        }

        public override void SetDefaults()
        {
            Item.damage = 49;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AuroraFire>();
            Item.shootSpeed = 7.5f;
            Item.useAmmo = AmmoID.Gel;

            Item.width = 68;
            Item.height = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item34;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = ItemRarityID.Lime;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float SpeedX = speedX + Main.rand.NextFloat(-15f, 15f) * 0.05f;
            float SpeedY = speedY + Main.rand.NextFloat(-15f, 15f) * 0.05f;
            Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, damage, knockBack, player.whoAmI, Main.rand.Next(4, 11), 0f);
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/AuroraBlazerGlow"));
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool ConsumeAmmo(Player player) => Main.rand.Next(100) >= 60;
    }
}
