using CalamityMod.CustomRecipes;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.DraedonsArsenal
{
    public class PulseRifle : ModItem
    {
        private int BaseDamage = 1420;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulse Rifle");
            Tooltip.SetDefault("Draedon's former pulse rifle, used in emergencies for creations which turned against him\n" +
                "When the pulse hits a target it will arc to another nearby target");
        }

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.width = 62;
            Item.height = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = BaseDamage;
            Item.knockBack = 0f;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulseRifleFire");
            Item.noMelee = true;

            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ItemRarityID.Purple;
            modItem.customRarity = CalamityRarity.DraedonRust;

            Item.shoot = ModContent.ProjectileType<PulseRifleShot>();
            Item.shootSpeed = 5f;

            modItem.UsesCharge = true;
            modItem.MaxCharge = 250f;
            modItem.ChargePerUse = 0.24f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            if (velocity.Length() > 5f)
            {
                velocity.Normalize();
                velocity *= 5f;
            }

            float SpeedX = velocity.X + (float)Main.rand.Next(-1, 2) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-1, 2) * 0.05f;

            Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<PulseRifleShot>(), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 5);

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 20).AddIngredient(ModContent.ItemType<DubiousPlating>(), 20).AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8).AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2).AddTile(ModContent.TileType<CosmicAnvil>()).Register();
        }
    }
}
