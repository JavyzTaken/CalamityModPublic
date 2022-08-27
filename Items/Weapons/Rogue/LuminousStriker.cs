﻿using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class LuminousStriker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminous Striker");
            Tooltip.SetDefault("Send the stars back to where they belong\n"
                              +"Throws a stardust javelin trailed by rising stardust shards\n"
                              +"Explodes into additional stardust shards upon hitting enemies\n"
                              +"Stealth strikes cause the stardust shards to fly alongside the javelin instead of rising");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 102;
            Item.damage = 149;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<LuminousStrikerProj>();
            Item.shootSpeed = 20f;
            Item.DamageType = RogueDamageClass.Instance;
        }

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
		{
			itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.RogueWeapon;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, (int)(damage * 1.25f), knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(proj))
                Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpearofPaleolith>().
                AddIngredient<ScourgeoftheSeas>().
                AddIngredient<Turbulance>().
                AddIngredient<MeldConstruct>(10).
                AddIngredient(ItemID.FragmentStardust, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
