﻿using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.Walls;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables
{
    public class UelibloomBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.UelibloomBrick>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<UelibloomOre>());
            recipe.AddIngredient(ItemID.StoneBlock);
            recipe.SetResult(this);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBrickWall>(), 4);
            recipe.SetResult(this);
            recipe.AddTile(ModContent.TileType<BotanicPlanter>());
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BotanicPlatform>(), 2);
            recipe.SetResult(this);
            recipe.AddTile(ModContent.TileType<BotanicPlanter>());
            recipe.AddRecipe();
        }
    }
}
