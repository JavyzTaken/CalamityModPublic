using CalamityMod.CustomRecipes;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.DraedonsArsenal
{
    public class PulsePistol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulse Pistol");
            Tooltip.SetDefault("Fires a pulse that arcs to a new target on enemy hits");
        }

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.width = 62;
            Item.height = 22;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 26;
            Item.knockBack = 0f;
            Item.useTime = Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulseRifleFire");
            Item.noMelee = true;

            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;

            Item.shoot = ModContent.ProjectileType<PulseRifleShot>();
            Item.shootSpeed = 5.2f; // This may seem low but the shot has 10 extra updates.

            modItem.UsesCharge = true;
            modItem.MaxCharge = 50f;
            modItem.ChargePerUse = 0.05f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<PulsePistolShot>(), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(10f, 0f);

        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 1);

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 5).AddIngredient(ModContent.ItemType<DubiousPlating>(), 7).AddIngredient(ModContent.ItemType<AerialiteBar>(), 4).AddIngredient(ModContent.ItemType<SeaPrism>(), 7).AddTile(TileID.Anvils).Register();
        }
    }
}
