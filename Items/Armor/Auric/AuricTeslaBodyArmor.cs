﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Tarragon;

namespace CalamityMod.Items.Armor.Auric
{
    [AutoloadEquip(EquipType.Body)]
    public class AuricTeslaBodyArmor : ModItem
    {
        public override void Load()
        {
            // All code below runs only if we're not loading on a server
            if (Main.netMode != NetmodeID.Server)
            {
                // Add equip textures
                EquipLoader.AddEquipTexture(Mod, "CalamityMod/Items/Armor/Auric/AuricTeslaBodyArmor_Back", EquipType.Back, this);
            }
        }

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Auric Tesla Body Armor");
            Tooltip.SetDefault("+100 max life\n" +
                       "8% increased damage and 5% increased critical strike chance\n" +
                       "You will freeze enemies near you when you are struck");
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.value = Item.buyPrice(1, 44, 0, 0);
            Item.defense = 48;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            modPlayer.fBarrier = true;
            player.statLifeMax2 += 100;
            player.GetDamage<GenericDamageClass>() += 0.08f;
            player.GetCritChance<GenericDamageClass>() += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerChestplate>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<FrostBarrier>().
                AddIngredient<AuricBar>(18).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<SilvaArmor>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<FrostBarrier>().
                AddIngredient<AuricBar>(18).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}