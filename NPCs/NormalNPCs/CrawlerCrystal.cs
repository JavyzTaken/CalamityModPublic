using CalamityMod.Items.Accessories;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Weapons.Melee;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.NPCs.NormalNPCs
{
    public class CrawlerCrystal : ModNPC
    {
        private bool detected = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Crawler");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.3f;
            NPC.aiStyle = -1;
            NPC.damage = 35;
            NPC.width = 44;
            NPC.height = 34;
            NPC.defense = 12;
            NPC.lifeMax = 300;
            NPC.knockBackResist = 0.15f;
            aiType = -1;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath36;
            banner = NPC.type;
            bannerItem = ModContent.ItemType<CrystalCrawlerBanner>();
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(detected);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            detected = reader.ReadBoolean();
        }

        public override void FindFrame(int frameHeight)
        {
            if (!detected)
            {
                NPC.frame.Y = frameHeight * 4;
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter += (double)(NPC.velocity.Length() / 8f);
            if (NPC.frameCounter > 2.0)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y >= frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void AI()
        {
            if (!detected)
                NPC.TargetClosest();
            if (((Main.player[NPC.target].Center - NPC.Center).Length() < 100f && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position,
                    Main.player[NPC.target].width, Main.player[NPC.target].height)) || NPC.justHit)
                detected = true;
            if (!detected)
                return;
            CalamityAI.GemCrawlerAI(NPC, Mod, 7f, 0.07f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe || spawnInfo.Player.Calamity().ZoneAbyss || spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.EnchantedSword.Chance;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 68, hitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 68, hitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void NPCLoot()
        {
            DropHelper.DropItem(NPC, ItemID.CrystalShard, 2, 4);
            DropHelper.DropItemChance(NPC, ModContent.ItemType<CrystalBlade>(), 5);
            DropHelper.DropItemChance(NPC, ModContent.ItemType<ScuttlersJewel>(), 10);
        }
    }
}
