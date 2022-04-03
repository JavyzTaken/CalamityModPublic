using CalamityMod.CustomRecipes;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.DraedonsArsenal
{
    public class SystemBane : RogueWeapon
    {
        public const int MaxDeployedProjectiles = 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("System Bane");
            Tooltip.SetDefault("Can be used to quickly send out an electromagnetic blast\n" +
                               "Hurls an unstable device which sticks to the ground and shocks nearby enemies with lightning\n" +
                               "Stealth strikes make the device emit a large, damaging EMP field");
        }

        public override void SafeSetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.damage = 45;
            modItem.rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 42;
            Item.height = 36;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 16f;
            Item.shoot = ModContent.ProjectileType<SystemBaneProjectile>();

            modItem.UsesCharge = true;
            modItem.MaxCharge = 135f;
            modItem.ChargePerUse = 0.085f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < MaxDeployedProjectiles;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            if (proj.WithinBounds(Main.maxProjectiles))
                Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void SafeModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 3);

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 20).AddIngredient(ModContent.ItemType<DubiousPlating>(), 10).AddIngredient(ModContent.ItemType<BarofLife>(), 5).AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
