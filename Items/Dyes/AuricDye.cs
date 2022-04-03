using CalamityMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Dyes
{
    public class AuricDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/Dyes/AuricDyeShader").Value), "DyePass").
            UseColor(new Color(170, 96, 60)).UseSecondaryColor(new Color(226, 196, 106)).SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/SharpNoise"));
        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Auric Dye");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.Violet;
            Item.value = Item.sellPrice(0, 9, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BottledWater).AddIngredient(ModContent.ItemType<AuricOre>(), 5).AddTile(TileID.DyeVat).Register();
        }
    }
}
