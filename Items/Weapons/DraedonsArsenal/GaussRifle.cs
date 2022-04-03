using CalamityMod.CustomRecipes;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.DraedonsArsenal
{
    public class GaussRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gauss Rifle");
            Tooltip.SetDefault("A large and slow weapon, the concussive force of its projectiles do well in clearing large groups\n" +
            "Fires a devastating high velocity blast with absurd knockback");
        }

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.width = 112;
            Item.height = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 150;
            Item.knockBack = 30f;
            Item.useTime = Item.useAnimation = 32;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GaussWeaponFire");
            Item.noMelee = true;

            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;

            Item.shoot = ModContent.ProjectileType<GaussRifleBlast>();
            Item.shootSpeed = 27f;

            modItem.UsesCharge = true;
            modItem.MaxCharge = 135f;
            modItem.ChargePerUse = 0.1125f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<GaussRifleBlast>(), damage, knockBack, player.whoAmI);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 3);

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 18).AddIngredient(ModContent.ItemType<DubiousPlating>(), 12).AddIngredient(ModContent.ItemType<BarofLife>(), 5).AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
