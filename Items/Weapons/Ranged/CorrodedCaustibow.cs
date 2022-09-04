﻿using Terraria.DataStructures;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class CorrodedCaustibow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corroded Caustibow");
            Tooltip.SetDefault("Converts wooden arrows into slow, powerful shells that trail an irradiated aura");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 88;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 38;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CalamityGlobalItem.Rarity6BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Shell>();
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 20;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityUtils.CheckWoodenAmmo(type, player))
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<CorrodedShell>(), damage, knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Shellshooter>().
                AddIngredient<Toxibow>().
                AddIngredient<CorrodedFossil>(10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
