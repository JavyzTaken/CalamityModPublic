using CalamityMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Dyes
{
    public class ReaverDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/Dyes/ReaverDyeShader").Value), "DyePass").
            UseColor(new Color(54, 164, 66)).UseSecondaryColor(new Color(224, 115, 65));
        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Reaver Dye");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ItemID.BottledWater, 2).AddIngredient(ModContent.ItemType<PerennialOre>(), 4).AddTile(TileID.DyeVat).Register();
        }
    }
}
