using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class Vigilance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vigilance");
            Tooltip.SetDefault("Summons a soul seeker to fight for you");
        }

        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.mana = 10;
            Item.width = Item.height = 32;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.DD2_BetsySummon;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SeekerSummonProj>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                int seekerIndex = 0;
                int totalSeekers = 1;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type != type || !Main.projectile[i].active || Main.projectile[i].owner != player.whoAmI)
                        continue;

                    totalSeekers++;
                }

                Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type != type || !Main.projectile[i].active || Main.projectile[i].owner != player.whoAmI)
                        continue;
                    Main.projectile[i].ai[0] = seekerIndex / (float)totalSeekers;
                    Main.projectile[i].netUpdate = true;

                    seekerIndex++;
                }
            }
            return false;
        }
    }
}
