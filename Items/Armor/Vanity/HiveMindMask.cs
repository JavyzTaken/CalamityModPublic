using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityMod.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class HiveMindMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hive Mind Mask");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}
