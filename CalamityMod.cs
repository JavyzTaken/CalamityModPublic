﻿using CalamityMod.Balancing;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Cooldowns;
using CalamityMod.DataStructures;
using CalamityMod.Effects;
using CalamityMod.Events;
using CalamityMod.FluidSimulation;
using CalamityMod.ILEditing;
using CalamityMod.Items;
using CalamityMod.Items.Dyes.HairDye;
using CalamityMod.Items.VanillaArmorChanges;
using CalamityMod.Localization;
using CalamityMod.NPCs.AdultEidolonWyrm;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.Calamitas;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Particles;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Schematics;
using CalamityMod.Skies;
using CalamityMod.UI;
using CalamityMod.UI.CalamitasEnchants;
using CalamityMod.Waters;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

[assembly: InternalsVisibleTo("CalTestHelpers")]
namespace CalamityMod
{
    public class CalamityMod : Mod
    {
        // CONSIDER -- I have been advised by Jopo that Mods should never contain static variables

        // Boss Spawners
        public static int ghostKillCount = 0;
        public static int sharkKillCount = 0;

        // Textures
        public static Asset<Texture2D> heartOriginal2;
        public static Asset<Texture2D> heartOriginal;
        public static Asset<Texture2D> rainOriginal;
        public static Asset<Texture2D> manaOriginal;
        public static Asset<Texture2D> carpetOriginal;
        public static Texture2D AstralSky;

        // DR data structure
        public static SortedDictionary<int, float> DRValues;

        // Boss Kill Time data structure
        public static SortedDictionary<int, int> bossKillTimes;

        // Boss velocity scaling data structure
        public static SortedDictionary<int, float> bossVelocityDamageScaleValues;
        public const float velocityScaleMin = 0.5f;
        public const float bitingEnemeyVelocityScale = 0.8f;

        // Life steal cap
        public const int lifeStealCap = 10;

        // Speedrun timer
        internal static Stopwatch SpeedrunTimer = new Stopwatch();

        // Debuff immunities, these are used in the NPCDebuffs file
        public static int[] slimeEnemyImmunities = new int[1] { BuffID.Poisoned };
        public static int[] iceEnemyImmunities = new int[3] { BuffID.Frostburn, ModContent.BuffType<GlacialState>(), ModContent.BuffType<ExoFreeze>() };
        public static int[] sulphurEnemyImmunities = new int[4] { BuffID.Poisoned, BuffID.Venom, ModContent.BuffType<SulphuricPoisoning>(), ModContent.BuffType<Irradiated>() };
        public static int[] sunkenSeaEnemyImmunities = new int[2] { ModContent.BuffType<Eutrophication>(), ModContent.BuffType<PearlAura>() };
        public static int[] abyssEnemyImmunities = new int[1] { ModContent.BuffType<CrushDepth>() };
        public static int[] cragEnemyImmunities = new int[4] { BuffID.OnFire, BuffID.OnFire3, ModContent.BuffType<AbyssalFlames>(), ModContent.BuffType<BrimstoneFlames>() };
        public static int[] astralEnemyImmunities = new int[2] { BuffID.Poisoned, ModContent.BuffType<AstralInfectionDebuff>() };
        public static int[] plagueEnemyImmunities = new int[3] { BuffID.Poisoned, BuffID.Venom, ModContent.BuffType<Plague>() };
        public static int[] holyEnemyImmunities = new int[4] { BuffID.OnFire, BuffID.OnFire3, ModContent.BuffType<HolyFlames>(), ModContent.BuffType<Nightwither>() };

        internal static CalamityMod Instance;
        internal Mod musicMod = null; // This is Calamity's official music mod, CalamityModMusic
        internal bool MusicAvailable => !(musicMod is null);
        internal Mod ancientsAwakened = null;
        internal Mod bossChecklist = null;
        internal Mod census = null;
        internal Mod crouchMod = null;
        internal Mod fargos = null;
        internal Mod overhaul = null;
        internal Mod redemption = null;
        internal Mod soa = null;
        internal Mod summonersAssociation = null;
        internal Mod thorium = null;
        internal Mod varia = null;
        internal Mod subworldLibrary = null;

        #region Load
        public override void Load()
        {
            Instance = this;

            // Save vanilla textures.
            heartOriginal2 = TextureAssets.Heart;
            heartOriginal = TextureAssets.Heart2;
            rainOriginal = TextureAssets.Rain;
            manaOriginal = TextureAssets.Mana;
            carpetOriginal = TextureAssets.FlyingCarpet;

            // Apply IL edits instantly afterwards.
            ILChanges.Load();

            // If any of these mods aren't loaded, it will simply keep them as null.
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
            ancientsAwakened = null;
            ModLoader.TryGetMod("AAMod", out ancientsAwakened);
            bossChecklist = null;
            ModLoader.TryGetMod("BossChecklist", out bossChecklist);
            census = null;
            ModLoader.TryGetMod("Census", out census);
            crouchMod = null;
            ModLoader.TryGetMod("CrouchMod", out crouchMod);
            fargos = null;
            ModLoader.TryGetMod("Fargowiltas", out fargos);
            overhaul = null;
            ModLoader.TryGetMod("TerrariaOverhaul", out overhaul);
            redemption = null;
            ModLoader.TryGetMod("Redemption", out redemption);
            soa = null;
            ModLoader.TryGetMod("SacredTools", out soa);
            summonersAssociation = null;
            ModLoader.TryGetMod("SummonersAssociation", out summonersAssociation);
            thorium = null;
            ModLoader.TryGetMod("ThoriumMod", out thorium);
            varia = null;
            ModLoader.TryGetMod("Varia", out varia);
            subworldLibrary = null;
            ModLoader.TryGetMod("SubworldLibrary", out subworldLibrary);

            // Initialize the EnemyStats struct as early as it is safe to do so
            NPCStats.Load();

            // Initialize Calamity Lists so they may be used elsewhere immediately
            CalamityLists.LoadLists();

            // Initialize Calamity Balance, since it is tightly coupled with the remaining lists
            CalamityGlobalItem.LoadTweaks();

            // Mount balancing occurs during runtime and is undone when Calamity is unloaded.
            Mount.mounts[MountID.Unicorn].dashSpeed *= CalamityPlayer.UnicornSpeedNerfPower;
            Mount.mounts[MountID.Unicorn].runSpeed *= CalamityPlayer.UnicornSpeedNerfPower;
            Mount.mounts[MountID.MinecartMech].dashSpeed *= CalamityPlayer.MechanicalCartSpeedNerfPower;
            Mount.mounts[MountID.MinecartMech].runSpeed *= CalamityPlayer.MechanicalCartSpeedNerfPower;

            if (!Main.dedServ)
            {
                LoadClient();
                GeneralParticleHandler.Load();
            }

            CooldownRegistry.Load();
            BossRushEvent.Load();
            BossHealthBarManager.Load(this);
            DraedonStructures.Load();
            EnchantmentManager.LoadAllEnchantments();
            VanillaArmorChangeManager.Load();
            SetupVanillaDR();
            SetupBossKillTimes();
            SetupBossVelocityScalingValues();
            CalamityLocalization.AddLocalizations();
            CalamityConfig.RegisterDynamicLocalization();
            SchematicManager.Load();
            CustomLavaManagement.Load();
            Attunement.Load();
            BalancingChangesManager.Load();
            BaseIdleHoldoutProjectile.LoadAll();
            PlayerDashManager.Load();
        }

        private void LoadClient()
        {
            AstralSky = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/AstralSky", AssetRequestMode.ImmediateLoad).Value;

            Filters.Scene["CalamityMod:DevourerofGodsHead"] = new Filter(new DoGScreenShaderData("FilterMiniTower").UseColor(0.4f, 0.1f, 1.0f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:DevourerofGodsHead"] = new DoGSky();

            Filters.Scene["CalamityMod:CalamitasRun3"] = new Filter(new CalScreenShaderData("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.6f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:CalamitasRun3"] = new CalSky();

            Filters.Scene["CalamityMod:PlaguebringerGoliath"] = new Filter(new PbGScreenShaderData("FilterMiniTower").UseColor(0.2f, 0.6f, 0.2f).UseOpacity(0.35f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:PlaguebringerGoliath"] = new PbGSky();

            Filters.Scene["CalamityMod:Yharon"] = new Filter(new YScreenShaderData("FilterMiniTower").UseColor(1f, 0.4f, 0f).UseOpacity(0.75f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:Yharon"] = new YSky();

            Filters.Scene["CalamityMod:Leviathan"] = new Filter(new LevScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0.5f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:Leviathan"] = new LevSky();

            Filters.Scene["CalamityMod:Providence"] = new Filter(new ProvScreenShaderData("FilterMiniTower").UseColor(0.45f, 0.4f, 0.2f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:Providence"] = new ProvSky();

            Filters.Scene["CalamityMod:SupremeCalamitas"] = new Filter(new SCalScreenShaderData("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:SupremeCalamitas"] = new SCalSky();

            Filters.Scene["CalamityMod:AdultEidolonWyrm"] = new Filter(new AEWScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0.25f).UseOpacity(0.35f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:AdultEidolonWyrm"] = new AEWSky();

            Filters.Scene["CalamityMod:Signus"] = new Filter(new SignusScreenShaderData("FilterMiniTower").UseColor(0.35f, 0.1f, 0.55f).UseOpacity(0.35f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:Signus"] = new SignusSky();

            Filters.Scene["CalamityMod:BossRush"] = new Filter(new BossRushScreenShader("FilterMiniTower").UseOpacity(0.75f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:BossRush"] = new BossRushSky();

            Filters.Scene["CalamityMod:ExoMechs"] = new Filter(new ExoMechsScreenShaderData("FilterMiniTower").UseColor(ExoMechsSky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityMod:ExoMechs"] = new ExoMechsSky();

            SkyManager.Instance["CalamityMod:Astral"] = new AstralSky();
            SkyManager.Instance["CalamityMod:Cryogen"] = new CryogenSky();
            SkyManager.Instance["CalamityMod:StormWeaverFlash"] = new StormWeaverFlashSky();

            CalamityShaders.LoadShaders();

            // This must be done separately from immediate loading, as loading is now multithreaded.
            // However, render targets and certain other graphical objects can only be created on the main thread.
            Main.QueueMainThreadAction(() =>
            {
                FusableParticleManager.LoadParticleRenderSets();
                Main.OnPreDraw += PrepareRenderTargets;
            });

            RipperUI.Load();
            AstralArcanumUI.Load(this);

            Apollo.LoadHeadIcons();
            Artemis.LoadHeadIcons();
            DevourerofGodsHead.LoadHeadIcons();
            DevourerofGodsBody.LoadHeadIcons();
            DevourerofGodsTail.LoadHeadIcons();
            HiveMind.LoadHeadIcons();
            Polterghast.LoadHeadIcons();
            StormWeaverHead.LoadHeadIcons();
            SupremeCalamitas.LoadHeadIcons();
            ThanatosHead.LoadHeadIcons();
            ThanatosBody1.LoadHeadIcons();
            ThanatosBody2.LoadHeadIcons();
            ThanatosTail.LoadHeadIcons();

            GameShaders.Hair.BindShader(ModContent.ItemType<AdrenalineHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(player.hairColor, new Color(0, 255, 171), ((float)player.Calamity().adrenaline / (float)player.Calamity().adrenalineMax))));
            GameShaders.Hair.BindShader(ModContent.ItemType<RageHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(player.hairColor, new Color(255, 83, 48), ((float)player.Calamity().rage / (float)player.Calamity().rageMax))));
            GameShaders.Hair.BindShader(ModContent.ItemType<WingTimeHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(player.hairColor, new Color(139, 205, 255), ((float)player.wingTime / (float)player.wingTimeMax))));
            GameShaders.Hair.BindShader(ModContent.ItemType<StealthHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(player.hairColor, new Color(186, 85, 211), (player.Calamity().rogueStealth / player.Calamity().rogueStealthMax))));

            PopupGUIManager.LoadGUIs();
            InvasionProgressUIManager.LoadGUIs();
        }
        #endregion

        #region Unload
        public override void Unload()
        {
            musicMod = null;
            ancientsAwakened = null;
            bossChecklist = null;
            census = null;
            crouchMod = null;
            fargos = null;
            overhaul = null;
            redemption = null;
            soa = null;
            summonersAssociation = null;
            thorium = null;
            varia = null;

            AstralSky = null;

            DRValues?.Clear();
            DRValues = null;
            bossKillTimes?.Clear();
            bossKillTimes = null;
            bossVelocityDamageScaleValues?.Clear();
            bossVelocityDamageScaleValues = null;

            BalancingChangesManager.Unload();
            Attunement.Unload();
            EnchantmentManager.UnloadAllEnchantments();
            VanillaArmorChangeManager.Unload();
            CalamityLists.UnloadLists();
            NPCStats.Unload();
            CalamityGlobalItem.UnloadTweaks();

            PopupGUIManager.UnloadGUIs();
            InvasionProgressUIManager.UnloadGUIs();
            BossRushEvent.Unload();
            SchematicManager.Unload();
            CustomLavaManagement.Unload();
            DraedonStructures.Unload();
            CooldownRegistry.Unload();
            PlayerDashManager.Unload();

            TileFraming.Unload();

            Main.QueueMainThreadAction(() =>
            {
                FusableParticleManager.UnloadParticleRenderSets();
                Main.OnPreDraw -= PrepareRenderTargets;
            });

            RipperUI.Unload();
            AstralArcanumUI.Unload();

            if (!Main.dedServ)
            {
                TextureAssets.Heart = heartOriginal2;
                TextureAssets.Heart2 = heartOriginal;
                TextureAssets.Rain = rainOriginal;
                TextureAssets.Mana = manaOriginal;
                TextureAssets.FlyingCarpet = carpetOriginal;
                GeneralParticleHandler.Unload();
            }
            Mount.mounts[MountID.Unicorn].dashSpeed /= CalamityPlayer.UnicornSpeedNerfPower;
            Mount.mounts[MountID.Unicorn].runSpeed /= CalamityPlayer.UnicornSpeedNerfPower;
            Mount.mounts[MountID.MinecartMech].dashSpeed /= CalamityPlayer.MechanicalCartSpeedNerfPower;
            Mount.mounts[MountID.MinecartMech].runSpeed /= CalamityPlayer.MechanicalCartSpeedNerfPower;

            heartOriginal2 = null;
            heartOriginal = null;
            rainOriginal = null;
            manaOriginal = null;
            carpetOriginal = null;

            ILChanges.Unload();
            Instance = null;
            base.Unload();
        }
        #endregion

        #region Late Loading
        public override void PostAddRecipes()
        {
            // This is placed here so that all tiles from all mods are guaranteed to be loaded at this point.
            TileFraming.Load();
        }
        #endregion

        #region Render Target Management

        public static void PrepareRenderTargets(GameTime gameTime)
        {
            FusableParticleManager.PrepareFusableParticleTargets();
            DeathAshParticle.PrepareRenderTargets();
            FluidFieldManager.Update();
        }
        #endregion Render Target Management

        #region ConfigCrap
        internal static void SaveConfig(CalamityConfig cfg)
        {
            // in-game ModConfig saving from mod code is not supported yet in tmodloader, and subject to change, so we need to be extra careful.
            // This code only supports client configs, and doesn't call onchanged. It also doesn't support ReloadRequired or anything else.
            MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
            if (saveMethodInfo != null)
                saveMethodInfo.Invoke(null, new object[] { cfg });
            else
                Instance.Logger.Warn("In-game SaveConfig failed, code update required");
        }
        #endregion

        #region Vanilla Enemy DR
        private void SetupVanillaDR()
        {
            DRValues = new SortedDictionary<int, float> {
                { NPCID.AngryBonesBig, 0.2f },
                { NPCID.AngryBonesBigHelmet, 0.2f },
                { NPCID.AngryBonesBigMuscle, 0.2f },
                { NPCID.AnomuraFungus, 0.1f },
                { NPCID.Antlion, 0.1f },
                { NPCID.Arapaima, 0.1f },
                { NPCID.ArmoredSkeleton, 0.15f },
                { NPCID.ArmoredViking, 0.1f },
                { NPCID.BigMimicCorruption, 0.3f },
                { NPCID.BigMimicCrimson, 0.3f },
                { NPCID.BigMimicHallow, 0.3f },
                { NPCID.BigMimicJungle, 0.3f }, // unused vanilla enemy
                { NPCID.BlueArmoredBones, 0.2f },
                { NPCID.BlueArmoredBonesMace, 0.2f },
                { NPCID.BlueArmoredBonesNoPants, 0.2f },
                { NPCID.BlueArmoredBonesSword, 0.2f },
                { NPCID.BoneLee, 0.2f },
                { NPCID.Crab, 0.05f },
                { NPCID.Crawdad, 0.2f },
                { NPCID.Crawdad2, 0.2f },
                { NPCID.CultistBoss, 0.15f },
                { NPCID.Deerclops, 0.05f },
                { NPCID.DD2Betsy, 0.1f },
                { NPCID.DD2OgreT2, 0.1f },
                { NPCID.DD2OgreT3, 0.15f },
                { NPCID.DeadlySphere, 0.4f },
                { NPCID.DiabolistRed, 0.2f },
                { NPCID.DiabolistWhite, 0.2f },
                { NPCID.DukeFishron, 0.15f },
                { NPCID.DungeonGuardian, 0.999999f },
                { NPCID.DungeonSpirit, 0.2f },
                { NPCID.ElfCopter, 0.15f },
                { NPCID.Everscream, 0.1f },
                { NPCID.FlyingAntlion, 0.05f },
                { NPCID.GiantCursedSkull, 0.2f },
                { NPCID.GiantShelly, 0.2f },
                { NPCID.GiantShelly2, 0.2f },
                { NPCID.GiantTortoise, 0.35f },
                { NPCID.Golem, 0.25f },
                { NPCID.GolemFistLeft, 0.25f },
                { NPCID.GolemFistRight, 0.25f },
                { NPCID.GolemHead, 0.25f },
                { NPCID.GolemHeadFree, 0.25f },
                { NPCID.GraniteFlyer, 0.1f },
                { NPCID.GraniteGolem, 0.15f },
                { NPCID.GreekSkeleton, 0.1f },
                { NPCID.HellArmoredBones, 0.2f },
                { NPCID.HellArmoredBonesMace, 0.2f },
                { NPCID.HellArmoredBonesSpikeShield, 0.2f },
                { NPCID.HellArmoredBonesSword, 0.2f },
                { NPCID.IceGolem, 0.1f },
                { NPCID.IceQueen, 0.1f },
                { NPCID.IceTortoise, 0.35f },
                { NPCID.HeadlessHorseman, 0.05f },
                { NPCID.MartianDrone, 0.2f },
                { NPCID.MartianSaucer, 0.2f },
                { NPCID.MartianSaucerCannon, 0.2f },
                { NPCID.MartianSaucerCore, 0.2f },
                { NPCID.MartianSaucerTurret, 0.2f },
                { NPCID.MartianTurret, 0.2f },
                { NPCID.MartianWalker, 0.35f },
                { NPCID.Mimic, 0.3f },
                { NPCID.MoonLordCore, 0.15f },
                { NPCID.MoonLordHand, 0.15f },
                { NPCID.MoonLordHead, 0.15f },
                { NPCID.Mothron, 0.2f },
                { NPCID.MothronEgg, 0.5f },
                { NPCID.MourningWood, 0.1f },
                { NPCID.Necromancer, 0.2f },
                { NPCID.NecromancerArmored, 0.2f },
                { NPCID.Paladin, 0.45f },
                { NPCID.PirateCaptain, 0.05f },
                { NPCID.PirateShipCannon, 0.15f },
                { NPCID.Plantera, 0.15f },
                { NPCID.PlanterasTentacle, 0.1f },
                { NPCID.HallowBoss, 0.15f },
                { NPCID.PossessedArmor, 0.25f },
                { NPCID.PresentMimic, 0.3f },
                { NPCID.PrimeCannon, 0.2f },
                { NPCID.PrimeLaser, 0.2f },
                { NPCID.PrimeSaw, 0.2f },
                { NPCID.PrimeVice, 0.2f },
                { NPCID.Probe, 0.2f },
                { NPCID.Pumpking, 0.1f },
                { NPCID.QueenBee, 0.05f },
                { NPCID.RaggedCaster, 0.2f },
                { NPCID.RaggedCasterOpenCoat, 0.2f },
                { NPCID.Retinazer, 0.2f },
                { NPCID.RustyArmoredBonesAxe, 0.2f },
                { NPCID.RustyArmoredBonesFlail, 0.2f },
                { NPCID.RustyArmoredBonesSword, 0.2f },
                { NPCID.RustyArmoredBonesSwordNoArmor, 0.2f },
                { NPCID.SandElemental, 0.1f },
                { NPCID.SantaNK1, 0.35f },
                { NPCID.SeaSnail, 0.05f },
                { NPCID.SkeletonArcher, 0.1f },
                { NPCID.SkeletonCommando, 0.2f },
                { NPCID.SkeletonSniper, 0.2f },
                { NPCID.SkeletronHand, 0.05f },
                { NPCID.SkeletronHead, 0.05f },
                { NPCID.SkeletronPrime, 0.2f },
                { NPCID.Spazmatism, 0.2f },
                { NPCID.TacticalSkeleton, 0.2f },
                { NPCID.TheDestroyer, 0.1f },
                { NPCID.TheDestroyerBody, 0.2f },
                { NPCID.TheDestroyerTail, 0.35f },
                { NPCID.TheHungry, 0.1f },
                { NPCID.UndeadViking, 0.1f },
                { NPCID.WalkingAntlion, 0.1f },
                { NPCID.WallofFlesh, 0.5f },
            };
        }
        #endregion

        #region Boss Kill Times
        private void SetupBossKillTimes()
        {
            // Kill times are measured exactly in frames.
            // 60   frames = 1 second
            // 3600 frames = 1 minute
            bossKillTimes = new SortedDictionary<int, int> {
                //
                // VANILLA BOSSES
                //
                { NPCID.KingSlime, 3600 }, // 1:00 (60 seconds)
                { NPCID.EyeofCthulhu, 5400 }, // 1:30 (90 seconds)
                { NPCID.EaterofWorldsHead, 7200 }, // 2:00 (120 seconds)
                { NPCID.EaterofWorldsBody, 7200 },
                { NPCID.EaterofWorldsTail, 7200 },
                { NPCID.BrainofCthulhu, 5400 }, // 1:30 (90 seconds)
                { NPCID.Creeper, 1800 }, // 0:30 (30 seconds)
                { NPCID.Deerclops, 5400 }, // 1:30 (90 seconds)
                { NPCID.QueenBee, 7200 }, // 2:00 (120 seconds)
                { NPCID.SkeletronHead, 9000 }, // 2:30 (150 seconds)
                { NPCID.WallofFlesh, 7200 }, // 2:00 (120 seconds)
                { NPCID.WallofFleshEye, 7200 },
                { NPCID.QueenSlimeBoss, 7200 }, // 2:00 (120 seconds)
                { NPCID.Spazmatism, 10800 }, // 3:00 (180 seconds)
                { NPCID.Retinazer, 10800 },
                { NPCID.TheDestroyer, 10800 }, // 3:00 (180 seconds)
                { NPCID.TheDestroyerBody, 10800 },
                { NPCID.TheDestroyerTail, 10800 },
                { NPCID.SkeletronPrime, 10800 }, // 3:00 (180 seconds)
                { NPCID.Plantera, 10800 }, // 3:00 (180 seconds)
                { NPCID.HallowBoss, 10800 }, // 3:00 (180 seconds)
                { NPCID.Golem, 9000 }, // 2:30 (150 seconds)
                { NPCID.GolemHead, 3600 }, // 1:00 (60 seconds)
                { NPCID.DukeFishron, 9000 }, // 2:30 (150 seconds)
                { NPCID.CultistBoss, 9000 }, // 2:30 (150 seconds)
                { NPCID.MoonLordCore, 14400 }, // 4:00 (240 seconds)
                { NPCID.MoonLordHand, 7200 }, // 2:00 (120 seconds)
                { NPCID.MoonLordHead, 7200 }, // 2:00 (120 seconds)

                //
                // CALAMITY BOSSES
                //
                { ModContent.NPCType<DesertScourgeHead>(), 3600 }, // 1:00 (60 seconds)
                { ModContent.NPCType<DesertScourgeBody>(), 3600 },
                { ModContent.NPCType<DesertScourgeTail>(), 3600 },
                { ModContent.NPCType<CrabulonIdle>(), 5400 }, // 1:30 (90 seconds)
                { ModContent.NPCType<HiveMind>(), 7200 }, // 2:00 (120 seconds)
                { ModContent.NPCType<PerforatorHive>(), 7200 }, // 2:00 (120 seconds)
                { ModContent.NPCType<SlimeGodCore>(), 10800 }, // 3:00 (180 seconds) -- total length of Slime God fight
                { ModContent.NPCType<SlimeGod>(), 3600 }, // 1:00 (60 seconds)
                { ModContent.NPCType<SlimeGodRun>(), 3600 }, // 1:00 (60 seconds)
                { ModContent.NPCType<SlimeGodSplit>(), 3600 }, // 1:00 (60 seconds) -- split slimes should spawn at 1:00 and die at around 2:00
                { ModContent.NPCType<SlimeGodRunSplit>(), 3600 }, // 1:00 (60 seconds)
                { ModContent.NPCType<Cryogen>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<AquaticScourgeHead>(), 7200 }, // 2:00 (120 seconds)
                { ModContent.NPCType<AquaticScourgeBody>(), 7200 },
                { ModContent.NPCType<AquaticScourgeBodyAlt>(), 7200 },
                { ModContent.NPCType<AquaticScourgeTail>(), 7200 },
                { ModContent.NPCType<BrimstoneElemental>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<CalamitasRun3>(), 14400 }, // 4:00 (240 seconds)
                { ModContent.NPCType<Siren>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<Leviathan>(), 10800 },
                { ModContent.NPCType<AstrumAureus>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<AstrumDeusHeadSpectral>(), 7200 }, // 2:00 (120 seconds) -- first phase is 1:00
                { ModContent.NPCType<AstrumDeusBodySpectral>(), 7200 },
                { ModContent.NPCType<AstrumDeusTailSpectral>(), 7200 },
                { ModContent.NPCType<PlaguebringerGoliath>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<RavagerBody>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<ProfanedGuardianBoss>(), 5400 }, // 1:30 (90 seconds)
                { ModContent.NPCType<Bumblefuck>(), 7200 }, // 2:00 (120 seconds)
                { ModContent.NPCType<Providence>(), 14400 }, // 4:00 (240 seconds)
                { ModContent.NPCType<CeaselessVoid>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<DarkEnergy>(), 1200 }, // 0:20 (20 seconds)
                { ModContent.NPCType<StormWeaverHead>(), 8100 }, // 2:15 (135 seconds)
                { ModContent.NPCType<StormWeaverBody>(), 8100 },
                { ModContent.NPCType<StormWeaverTail>(), 8100 },
                { ModContent.NPCType<Signus>(), 7200 }, // 2:00 (120 seconds)
                { ModContent.NPCType<Polterghast>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<OldDuke>(), 10800 }, // 3:00 (180 seconds)
                { ModContent.NPCType<DevourerofGodsHead>(), 14400 }, // 4:00 (240 seconds)
                { ModContent.NPCType<DevourerofGodsBody>(), 14400 }, // NOTE: Sentinels Phase takes 1:00, so with that included it's 5:00
                { ModContent.NPCType<DevourerofGodsTail>(), 14400 }, // DoG Phase 1 is 1:30, DoG Phase 2 is 2:30
                { ModContent.NPCType<Yharon>(), 14700 }, // 4:05 (245 seconds) -- he spends 5 seconds invincible where you can't do anything
                { ModContent.NPCType<Apollo>(), 21600 }, // 6:00 (360 seconds)
                { ModContent.NPCType<Artemis>(), 21600 },
                { ModContent.NPCType<AresBody>(), 21600 }, // 6:00 (360 seconds)
                { ModContent.NPCType<AresGaussNuke>(), 21600 },
                { ModContent.NPCType<AresLaserCannon>(), 21600 },
                { ModContent.NPCType<AresPlasmaFlamethrower>(), 21600 },
                { ModContent.NPCType<AresTeslaCannon>(), 21600 },
                { ModContent.NPCType<ThanatosHead>(), 21600 }, // 6:00 (360 seconds)
                { ModContent.NPCType<ThanatosBody1>(), 21600 },
                { ModContent.NPCType<ThanatosBody2>(), 21600 },
                { ModContent.NPCType<ThanatosTail>(), 21600 },
                { ModContent.NPCType<SupremeCalamitas>(), 18000 }, // 5:00 (300 seconds)
                { ModContent.NPCType<EidolonWyrmHeadHuge>(), 18000 } // 5:00 (300 seconds)
            };
        }
        #endregion

        #region Boss Velocity Contact Damage Scale Values
        private void SetupBossVelocityScalingValues()
        {
            bossVelocityDamageScaleValues = new SortedDictionary<int, float> {
                { NPCID.KingSlime, velocityScaleMin },
                { NPCID.EyeofCthulhu, velocityScaleMin }, // Increases in phase 2
                { NPCID.EaterofWorldsHead, bitingEnemeyVelocityScale },
                { NPCID.EaterofWorldsBody, velocityScaleMin },
                { NPCID.EaterofWorldsTail, velocityScaleMin },
                { NPCID.Creeper, velocityScaleMin },
                { NPCID.BrainofCthulhu, velocityScaleMin },
                { NPCID.QueenBee, velocityScaleMin },
                { NPCID.SkeletronHead, velocityScaleMin },
                { NPCID.SkeletronHand, velocityScaleMin },
                { NPCID.TheHungry, bitingEnemeyVelocityScale },
                { NPCID.TheHungryII, bitingEnemeyVelocityScale },
                { NPCID.LeechHead, bitingEnemeyVelocityScale },
                { NPCID.LeechBody, velocityScaleMin },
                { NPCID.LeechTail, velocityScaleMin },
                { NPCID.QueenSlimeBoss, velocityScaleMin },
                { NPCID.Spazmatism, velocityScaleMin }, // Increases in phase 2
                { NPCID.Retinazer, velocityScaleMin },
                { NPCID.TheDestroyer, bitingEnemeyVelocityScale },
                { NPCID.TheDestroyerBody, velocityScaleMin },
                { NPCID.TheDestroyerTail, velocityScaleMin },
                { NPCID.SkeletronPrime, velocityScaleMin },
                { NPCID.PrimeCannon, velocityScaleMin },
                { NPCID.PrimeLaser, velocityScaleMin },
                { NPCID.PrimeSaw, velocityScaleMin },
                { NPCID.PrimeVice, velocityScaleMin },
                { NPCID.Plantera, velocityScaleMin }, // Increases in phase 2
                { NPCID.PlanterasTentacle, bitingEnemeyVelocityScale },
                { NPCID.HallowBoss, velocityScaleMin },
                { NPCID.Golem, velocityScaleMin },
                { NPCID.GolemFistLeft, velocityScaleMin },
                { NPCID.GolemFistRight, velocityScaleMin },
                { NPCID.GolemHead, velocityScaleMin },
                { NPCID.DukeFishron, velocityScaleMin },
                { ModContent.NPCType<DesertScourgeHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DesertScourgeBody>(), velocityScaleMin },
                { ModContent.NPCType<DesertScourgeTail>(), velocityScaleMin },
                { ModContent.NPCType<DesertNuisanceHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DesertNuisanceBody>(), velocityScaleMin },
                { ModContent.NPCType<DesertNuisanceTail>(), velocityScaleMin },
                { ModContent.NPCType<CrabulonIdle>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<HiveMind>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHive>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHeadLarge>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PerforatorBodyLarge>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorTailLarge>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHeadMedium>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PerforatorBodyMedium>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorTailMedium>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHeadSmall>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PerforatorBodySmall>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorTailSmall>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodCore>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGod>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodRun>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodSplit>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodRunSplit>(), velocityScaleMin },
                { ModContent.NPCType<SlimeSpawnCorrupt>(), velocityScaleMin },
                { ModContent.NPCType<Cryogen>(), velocityScaleMin },
                { ModContent.NPCType<AquaticScourgeHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<AquaticScourgeBody>(), velocityScaleMin },
                { ModContent.NPCType<AquaticScourgeBodyAlt>(), velocityScaleMin },
                { ModContent.NPCType<AquaticScourgeTail>(), velocityScaleMin },
                { ModContent.NPCType<BrimstoneElemental>(), velocityScaleMin },
                { ModContent.NPCType<CalamitasRun>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<CalamitasRun2>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<CalamitasRun3>(), velocityScaleMin },
                { ModContent.NPCType<Leviathan>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<Siren>(), velocityScaleMin },
                { ModContent.NPCType<AstrumAureus>(), velocityScaleMin },
                { ModContent.NPCType<AstrumDeusHeadSpectral>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<AstrumDeusBodySpectral>(), velocityScaleMin },
                { ModContent.NPCType<AstrumDeusTailSpectral>(), velocityScaleMin },
                { ModContent.NPCType<PlaguebringerGoliath>(), velocityScaleMin },
                { ModContent.NPCType<RavagerBody>(), velocityScaleMin },
                { ModContent.NPCType<RavagerClawLeft>(), velocityScaleMin },
                { ModContent.NPCType<RavagerClawRight>(), velocityScaleMin },
                { ModContent.NPCType<RavagerLegLeft>(), velocityScaleMin },
                { ModContent.NPCType<RavagerLegRight>(), velocityScaleMin },
                { ModContent.NPCType<RockPillar>(), velocityScaleMin },
                { ModContent.NPCType<ProfanedGuardianBoss>(), velocityScaleMin },
                { ModContent.NPCType<ProfanedGuardianBoss2>(), velocityScaleMin },
                { ModContent.NPCType<ProfanedGuardianBoss3>(), velocityScaleMin },
                { ModContent.NPCType<Bumblefuck>(), velocityScaleMin },
                { ModContent.NPCType<Bumblefuck2>(), velocityScaleMin },
                { ModContent.NPCType<CeaselessVoid>(), velocityScaleMin },
                { ModContent.NPCType<DarkEnergy>(), velocityScaleMin },
                { ModContent.NPCType<StormWeaverHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<StormWeaverBody>(), velocityScaleMin },
                { ModContent.NPCType<StormWeaverTail>(), velocityScaleMin },
                { ModContent.NPCType<Signus>(), velocityScaleMin },
                { ModContent.NPCType<CosmicLantern>(), velocityScaleMin },
                { ModContent.NPCType<Polterghast>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PolterPhantom>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<OldDuke>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DevourerofGodsBody>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsTail>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsHead2>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DevourerofGodsBody2>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsTail2>(), velocityScaleMin },
                { ModContent.NPCType<Yharon>(), velocityScaleMin },
                { ModContent.NPCType<SupremeCalamitas>(), velocityScaleMin },
                { ModContent.NPCType<Apollo>(), velocityScaleMin }, // Increases in phase 2
                { ModContent.NPCType<Artemis>(), velocityScaleMin },
                { ModContent.NPCType<ThanatosHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<ThanatosBody1>(), velocityScaleMin },
                { ModContent.NPCType<ThanatosBody2>(), velocityScaleMin },
                { ModContent.NPCType<ThanatosTail>(), velocityScaleMin },
                { ModContent.NPCType<EidolonWyrmHeadHuge>(), bitingEnemeyVelocityScale }
            };
        }
        #endregion

        #region Music

        // This function returns an available Calamity Music Mod track, or null if the Calamity Music Mod is not available.
        public int? GetMusicFromMusicMod(string songFilename) => MusicAvailable ? MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/" + songFilename) : null;

        #endregion

        #region Mod Support
        public override void PostSetupContent() => WeakReferenceSupport.Setup();

        public override object Call(params object[] args) => ModCalls.Call(args);
        #endregion

        #region Recipes
        public override void AddRecipeGroups() => CalamityRecipes.AddRecipeGroups();

        public override void AddRecipes() => CalamityRecipes.AddRecipes();
        #endregion

        #region Seasons
        public static Season CurrentSeason
        {
            get
            {
                DateTime date = DateTime.Now;
                int day = date.DayOfYear - Convert.ToInt32(DateTime.IsLeapYear(date.Year) && date.DayOfYear > 59);

                if (day < 80 || day >= 355)
                {
                    return Season.Winter;
                }

                else if (day >= 80 && day < 172)
                {
                    return Season.Spring;
                }

                else if (day >= 172 && day < 266)
                {
                    return Season.Summer;
                }

                else
                {
                    return Season.Fall;
                }
            }
        }
        #endregion

        #region Stop Rain
        public static void StopRain()
        {
            if (!Main.raining)
                return;
            Main.raining = false;
            CalamityNetcode.SyncWorld();
        }
        #endregion

        #region Netcode
        public override void HandlePacket(BinaryReader reader, int whoAmI) => CalamityNetcode.HandlePacket(this, reader, whoAmI);
        #endregion
    }
}
