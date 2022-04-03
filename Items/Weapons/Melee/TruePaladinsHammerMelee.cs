using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class TruePaladinsHammerMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fallen Paladin's Hammer");
            Tooltip.SetDefault("Explodes on enemy hits");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 80);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<FallenPaladinsHammerProj>();
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.PaladinsHammer).AddIngredient(ModContent.ItemType<PwnagehammerMelee>()).AddIngredient(ModContent.ItemType<CalamityDust>(), 5).AddIngredient(ModContent.ItemType<CoreofChaos>(), 5).AddIngredient(ModContent.ItemType<CruptixBar>(), 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
