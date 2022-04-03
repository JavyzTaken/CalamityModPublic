using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class SolsticeClaymore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solstice Claymore");
            Tooltip.SetDefault("Changes projectile color based on the time of year\n" +
                               "Inflicts daybroken during the day and nightwither during the night");
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.damage = 300;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.useTurn = true;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 86;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<SolsticeBeam>();
            Item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BeamSword).AddIngredient(ModContent.ItemType<AstralBar>(), 20).AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5).AddIngredient(ItemID.LunarBar, 5).AddTile(TileID.LunarCraftingStation).Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dustType = Main.dayTime ?
            Utils.SelectRandom(Main.rand, new int[]
            {
            6,
            259,
            158
            }) :
            Utils.SelectRandom(Main.rand, new int[]
            {
            173,
            27,
            234
            });
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.dayTime)
            {
                target.AddBuff(BuffID.Daybreak, 300);
            }
            else
            {
                target.AddBuff(ModContent.BuffType<Nightwither>(), 300);
            }
        }

        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            if (!Main.dayTime)
            {
                target.AddBuff(ModContent.BuffType<Nightwither>(), 300);
            }
        }
    }
}
