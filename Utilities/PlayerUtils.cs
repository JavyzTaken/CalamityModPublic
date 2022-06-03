﻿using CalamityMod.Balancing;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod
{
    public static partial class CalamityUtils
    {
        #region Stat Retrieval
        public static int GetCurrentDefense(this Player player, bool accountForDefenseDamage = false)
        {
            CalamityPlayer mp = player.Calamity();
            return player.statDefense + (accountForDefenseDamage ? 0 : mp.CurrentDefenseDamage);
        }

        public static StatModifier GetBestClassDamage(this Player player)
        {
            StatModifier ret = new();
            StatModifier classless = player.GetDamage<GenericDamageClass>();

            // Atypical damage stats are copied from "classless", like Avenger Emblem. This prevents stacking flat damage effects repeatedly.
            ret.Base = classless.Base;
            ret *= classless.Multiplicative;
            ret.Flat = classless.Flat;

            // Check the five Calamity classes to see what the strongest one is, and use that for the typical damage stat.
            float best = 0f;

            float melee = player.GetDamage<MeleeDamageClass>().Additive;
            if (melee > best) best = melee;
            float ranged = player.GetDamage<RangedDamageClass>().Additive;
            if (ranged > best) best = ranged;
            float magic = player.GetDamage<MagicDamageClass>().Additive;
            if (magic > best) best = magic;

            // Summoner intentionally has a reduction. As the only class with no crit, it tends to have higher raw damage than other classes.
            float summon = player.GetDamage<SummonDamageClass>().Additive * BalancingConstants.SummonAllClassScalingFactor;
            if (summon > best) best = summon;
            // We intentionally don't check whip class, because it inherits 100% from Summon

            float rogue = player.GetDamage<RogueDamageClass>().Additive;
            if (rogue > best) best = rogue;

            // Add the best typical damage stat, then return the full modifier.
            ret += best;
            return ret;
        }

        public static float GetRangedAmmoCostReduction(this Player player)
        {
            // Tally up all possible vanilla effects.
            float vanillaCost = player.ammoBox ? 0.8f : 1f;
            if (player.ammoPotion)
                vanillaCost *= 0.8f;
            if (player.ammoCost80)
                vanillaCost *= 0.8f;
            if (player.ammoCost75)
                vanillaCost *= 0.75f;

            // Account for Calamity effects.
            return vanillaCost * player.Calamity().rangedAmmoCost;
        }

        public static float GetStandingStealthRegen(this Player player)
        {
            CalamityPlayer mp = player.Calamity();
            return (mp.rogueStealthMax / BalancingConstants.BaseStealthGenTime) * mp.stealthGenStandstill;
        }

        public static float GetMovingStealthRegen(this Player player)
        {
            CalamityPlayer mp = player.Calamity();
            return (mp.rogueStealthMax / BalancingConstants.BaseStealthGenTime) * BalancingConstants.MovingStealthGenRatio * mp.stealthGenMoving * mp.stealthAcceleration;
        }

        public static float GetJumpBoost(this Player player) => player.jumpSpeedBoost + (player.wereWolf ? 0.2f : 0f) + (player.jumpBoost ? BalancingConstants.BalloonJumpSpeedBoost : 0f);

        /// <summary>
        /// Calculates and returns the player's total light strength. This is used for Abyss darkness, among other things.<br/>
        /// The Stat Meter also reports this stat.
        /// </summary>
        /// <returns>The player's total light strength.</returns>
        public static int GetCurrentAbyssLightLevel(this Player player)
        {
            CalamityPlayer mp = player.Calamity();
            int light = mp.externalAbyssLight;
            bool underwater = player.IsUnderwater();
            bool miningHelmet = player.head == ArmorIDs.Head.MiningHelmet;

            // The campfire bonus does not apply while in the Abyss.
            if (!mp.ZoneAbyss && (player.HasBuff(BuffID.Campfire) || Main.SceneMetrics.HasCampfire))
                light += 1;
            if (mp.camper) // inherits Campfire so it is +2 in practice
                light += 1;
            if (miningHelmet)
                light += 1;
            if (player.lightOrb)
                light += 1;
            if (player.crimsonHeart)
                light += 1;
            if (player.magicLantern)
                light += 1;
            if (mp.giantPearl)
                light += 1;
            if (mp.radiator)
                light += 1;
            if (mp.bendyPet)
                light += 1;
            if (mp.sparks)
                light += 1;
            if (mp.fathomSwarmerVisage)
                light += 1;
            if (mp.aquaticHeart)
                light += 1;
            if (mp.aAmpoule)
                light += 1;
            else if (mp.rOoze && !Main.dayTime) // radiant ooze and ampoule/higher don't stack
                light += 1;
            if (mp.aquaticEmblem && underwater)
                light += 1;
            if (player.arcticDivingGear && underwater) // inherited by abyssal diving gear/suit. jellyfish necklace is inherited so arctic diving gear is really +2
                light += 1;
            if (mp.jellyfishNecklace && underwater) // inherited by jellyfish diving gear and higher
                light += 1;
            if (mp.lumenousAmulet && underwater)
                light += 2;
            if (mp.shine)
                light += 2;
            if (mp.blazingCore)
                light += 2;
            if (player.redFairy || player.greenFairy || player.blueFairy)
                light += 2;
            if (mp.babyGhostBell)
                light += underwater ? 2 : 1;
            if (player.petFlagDD2Ghost)
                light += 2;
            if (mp.sirenPet)
                light += underwater ? 3 : 1;
            if (player.wisp)
                light += 3;
            if (player.suspiciouslookingTentacle)
                light += 3;
            if (mp.littleLightPet)
                light += 3;
            if (mp.profanedCrystalBuffs && !mp.ZoneAbyss)
                light += Main.dayTime || player.lavaWet ? 2 : 1; // not sure how you'd be in lava in the abyss but go ham I guess
            return light;
        }
        #endregion

        #region Movement and Controls
        public static bool ControlsEnabled(this Player player, bool allowWoFTongue = false)
        {
            if (player.CCed) // Covers frozen (player.frozen), webs (player.webbed), and Medusa (player.stoned)
                return false;
            if (player.tongued && !allowWoFTongue)
                return false;
            return true;
        }
        
        public static bool StandingStill(this Player player, float velocity = 0.05f) => player.velocity.Length() < velocity;

        /// <summary>
        /// Checks if the player is ontop of solid ground. May also check for solid ground for X tiles in front of them
        /// </summary>
        /// <param name="player">The Player whose position is being checked</param>
        /// <param name="solidGroundAhead">How many tiles in front of the player to check</param>
        /// <param name="airExposureNeeded">How many tiles above every checked tile are checked for non-solid ground</param>
        public static bool CheckSolidGround(this Player player, int solidGroundAhead = 0, int airExposureNeeded = 0)
        {
            if (player.velocity.Y != 0) //Player gotta be standing still in any case
                return false;

            Tile checkedTile;
            bool ConditionMet = true;

            for (int i = 0; i <= solidGroundAhead; i++) //Check i tiles in front of the player
            {
                ConditionMet = Main.tile[(int)player.Center.X / 16 + player.direction * i, (int)(player.position.Y + (float)player.height - 1f) / 16 + 1].IsTileSolidGround();
                if (!ConditionMet)
                    return ConditionMet;

                for (int j = 1; j <= airExposureNeeded; j++) //Check j tiles ontop of each checked tiles for non-solid ground
                {
                    checkedTile = Main.tile[(int)player.Center.X / 16 + player.direction * i, (int)(player.position.Y + (float)player.height - 1f) / 16 + 1 - j];

                    ConditionMet = !(checkedTile != null && checkedTile.HasUnactuatedTile && Main.tileSolid[checkedTile.TileType]); //IsTileSolidGround minus the ground part, to avoid platforms and other half solid tiles messing it up
                    if (!ConditionMet)
                        return ConditionMet;
                }
            }
            return ConditionMet;
        }
        #endregion

        #region Location and Biomes
        public static bool IsUnderwater(this Player player) => Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
        public static bool InSpace(this Player player)
        {
            float x = Main.maxTilesX / 4200f;
            x *= x;
            float spaceGravityMult = (float)((player.position.Y / 16f - (60f + 10f * x)) / (Main.worldSurface / 6.0));
            return spaceGravityMult < 1f;
        }
        public static bool PillarZone(this Player player) => player.ZoneTowerStardust || player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula;
        public static bool InCalamity(this Player player) => player.Calamity().ZoneCalamity;
        public static bool InSunkenSea(this Player player) => player.Calamity().ZoneSunkenSea;
        public static bool InSulphur(this Player player) => player.Calamity().ZoneSulphur;
        public static bool InAstral(this Player player, int biome = 0) //1 is above ground, 2 is underground, 3 is desert
        {
            switch (biome)
            {
                case 1:
                    return player.Calamity().ZoneAstral && (player.ZoneOverworldHeight || player.ZoneSkyHeight);

                case 2:
                    return player.Calamity().ZoneAstral && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight);

                case 3:
                    return player.Calamity().ZoneAstral && player.ZoneDesert;

                default:
                    return player.Calamity().ZoneAstral;
            }
        }
        public static bool InAbyss(this Player player, int layer = 0)
        {
            switch (layer)
            {
                case 1:
                    return player.Calamity().ZoneAbyssLayer1;

                case 2:
                    return player.Calamity().ZoneAbyssLayer2;

                case 3:
                    return player.Calamity().ZoneAbyssLayer3;

                case 4:
                    return player.Calamity().ZoneAbyssLayer4;

                default:
                    return player.Calamity().ZoneAbyss;
            }
        }
        #endregion

        #region Inventory Checks
        // TODO -- Wrong. This should return false for weapons which emit true melee projectiles e.g. Arkhalis
        public static bool HoldingProjectileMeleeWeapon(this Player player)
        {
            Item item = player.ActiveItem();
            return item.CountsAsClass<MeleeDamageClass>() && item.shoot != ProjectileID.None;
        }

        public static bool HoldingTrueMeleeWeapon(this Player player) => player.ActiveItem().IsTrueMelee();

        public static bool InventoryHas(this Player player, params int[] items)
        {
            return player.inventory.Any(item => items.Contains(item.type));
        }

        public static bool PortableStorageHas(this Player player, params int[] items)
        {
            bool hasItem = false;
            if (player.bank.item.Any(item => items.Contains(item.type)))
                hasItem = true;
            if (player.bank2.item.Any(item => items.Contains(item.type)))
                hasItem = true;
            if (player.bank3.item.Any(item => items.Contains(item.type)))
                hasItem = true;
            return hasItem;
        }
        #endregion

        #region Immunity Frames
        /// <summary>
        /// Gives the player the specified number of immunity frames (or "iframes" for short).<br />If the player already has more iframes than you want to give them, this function does nothing.
        /// </summary>
        /// <param name="player">The player who should be given immunity frames.</param>
        /// <param name="frames">The number of immunity frames to give.</param>
        /// <param name="blink">Whether or not the player should be blinking during this time.</param>
        /// <returns>Whether or not any immunity frames were given.</returns>
        public static bool GiveIFrames(this Player player, int frames, bool blink = false)
        {
            // Check to see if there is any way for the player to get iframes from this operation.
            bool anyIFramesWouldBeGiven = false;
            for (int i = 0; i < player.hurtCooldowns.Length; ++i)
                if (player.hurtCooldowns[i] < frames)
                    anyIFramesWouldBeGiven = true;

            // If they would get nothing, don't do it.
            if (!anyIFramesWouldBeGiven)
                return false;

            // Apply iframes thoroughly.
            player.immune = true;
            player.immuneNoBlink = !blink;
            player.immuneTime = frames;
            for (int i = 0; i < player.hurtCooldowns.Length; ++i)
                if (player.hurtCooldowns[i] < frames)
                    player.hurtCooldowns[i] = frames;
            return true;
        }

        /// <summary>
        /// Removes all immunity frames (or "iframes" for short) from the specified player immediately.
        /// </summary>
        /// <param name="player">The player whose iframes should be removed.</param>
        public static void RemoveAllIFrames(this Player player)
        {
            player.immune = false;
            player.immuneNoBlink = false;
            player.immuneTime = 0;
            for (int i = 0; i < player.hurtCooldowns.Length; ++i)
                player.hurtCooldowns[i] = 0;
        }
        #endregion

        #region Rage and Adrenaline
        /// <summary>
        /// Returns the damage multiplier Adrenaline Mode provides for the given player.
        /// </summary>
        /// <param name="mp">The player whose Adrenaline damage should be calculated.</param>
        /// <returns>Adrenaline damage multiplier. 1.0 would be no change.</returns>
        public static float GetAdrenalineDamage(this CalamityPlayer mp)
        {
            float adrenalineBoost = CalamityPlayer.AdrenalineDamageBoost;
            if (mp.adrenalineBoostOne)
                adrenalineBoost += CalamityPlayer.AdrenalineDamagePerBooster;
            if (mp.adrenalineBoostTwo)
                adrenalineBoost += CalamityPlayer.AdrenalineDamagePerBooster;
            if (mp.adrenalineBoostThree)
                adrenalineBoost += CalamityPlayer.AdrenalineDamagePerBooster;

            return adrenalineBoost;
        }

        /// <summary>
        /// Applies Rage and Adrenaline to the given damage multiplier. The values controlling the so-called "Rippers" can be found in CalamityPlayer.
        /// </summary>
        /// <param name="mp">The CalamityPlayer who may or may not be using Rage or Adrenaline.</param>
        /// <param name="damageMult">A reference to the current in-use damage multiplier. This will be increased in-place.</param>
        public static void ApplyRippersToDamage(CalamityPlayer mp, bool trueMelee, ref double damageMult)
        {
            // Reduce how much true melee benefits from Rage and Adrenaline.
            double rageAndAdrenalineTrueMeleeDamageMult = 0.5;

            // Rage and Adrenaline now stack additively with no special cases.
            if (mp.rageModeActive)
                damageMult += trueMelee ? mp.RageDamageBoost * rageAndAdrenalineTrueMeleeDamageMult : mp.RageDamageBoost;
            // Draedon's Heart disables Adrenaline damage.
            if (mp.adrenalineModeActive && !mp.draedonsHeart)
                damageMult += trueMelee ? mp.GetAdrenalineDamage() * rageAndAdrenalineTrueMeleeDamageMult : mp.GetAdrenalineDamage();
        }
        #endregion

        #region Cooldowns
        public static bool HasCooldown(this Player p, string id)
        {
            if (p is null)
                return false;
            CalamityPlayer modPlayer = p.Calamity();
            return !(modPlayer is null) && modPlayer.cooldowns.ContainsKey(id);
        }

        /// <summary>
        /// Applies the specified cooldown to the player, creating a new instance automatically.<br/>
        /// By default, overwrites existing instances of this cooldown, but this behavior can be disabled.
        /// </summary>
        /// <param name="p">The player to whom the cooldown should be applied.</param>
        /// <param name="id">The string ID of the cooldown to apply. This is referenced against the Cooldown Registry.</param>
        /// <param name="duration">The duration, in frames, of this instance of the cooldown.</param>
        /// <param name="overwrite">Whether or not to overwrite any existing instances of this cooldown. Defaults to true.</param>
        /// <returns>The cooldown instance which was created. <b>Note the cooldown is always created, but may not be necessarily applied to the player.</b></returns>
        public static CooldownInstance AddCooldown(this Player p, string id, int duration, bool overwrite = true)
        {
            var cd = CooldownRegistry.Get(id);
            CooldownInstance instance = new CooldownInstance(p, cd, duration);

            bool alreadyHasCooldown = p.HasCooldown(id);
            if (!alreadyHasCooldown || overwrite)
            {
                CalamityPlayer mp = p.Calamity();
                mp.cooldowns[id] = instance;
                mp.SyncCooldownAddition(Main.netMode == NetmodeID.Server, instance);
            }

            return instance;
        }

        /// <summary>
        /// Applies the specified cooldown to the player, creating a new instance automatically.<br/>
        /// By default, overwrites existing instances of this cooldown, but this behavior can be disabled.
        /// </summary>
        /// <param name="p">The player to whom the cooldown should be applied.</param>
        /// <param name="id">The string ID of the cooldown to apply. This is referenced against the Cooldown Registry.</param>
        /// <param name="duration">The duration, in frames, of this instance of the cooldown.</param>
        /// <param name="overwrite">Whether or not to overwrite any existing instances of this cooldown. Defaults to true.</param>
        /// <param name="handlerArgs">Arbitrary extra arguments to pass to the CooldownHandler constructor via reflection.</param>
        /// <returns>The cooldown instance which was created. <b>Note the cooldown is always created, but may not be necessarily applied to the player.</b></returns>
        public static CooldownInstance AddCooldown(this Player p, string id, int duration, bool overwrite = true, params object[] handlerArgs)
        {
            var cd = CooldownRegistry.Get(id);
            CooldownInstance instance = new CooldownInstance(p, cd, duration, handlerArgs);

            bool alreadyHasCooldown = p.HasCooldown(id);
            if (!alreadyHasCooldown || overwrite)
                p.Calamity().cooldowns[id] = instance;

            return instance;
        }

        public static IList<CooldownInstance> GetDisplayedCooldowns(this Player p)
        {
            List<CooldownInstance> ret = new List<CooldownInstance>(16);
            if (p is null || p.Calamity() is null)
                return ret;

            foreach (CooldownInstance instance in p.Calamity().cooldowns.Values)
                if (instance.handler.ShouldDisplay)
                    ret.Add(instance);
            return ret;
        }
        #endregion

        #region Debuffs
        public static void Inflict246DebuffsPvp(Player target, int buff, float timeBase = 2f)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(buff, SecondsToFrames(timeBase * 3f), false);
            }
            else if (Main.rand.NextBool(2))
            {
                target.AddBuff(buff, SecondsToFrames(timeBase * 2f), false);
            }
            else
            {
                target.AddBuff(buff, SecondsToFrames(timeBase), false);
            }
        }

        /// <summary>
        /// Inflict typical exo weapon debuffs in pvp.
        /// </summary>
        /// <param name="target">The Player attacked.</param>
        /// <param name="multiplier">Debuff time multiplier if needed.</param>
        /// <returns>Inflicts debuffs if the target isn't immune.</returns>
        public static void ExoDebuffs(this Player target, float multiplier = 1f)
        {
            target.AddBuff(BuffType<ExoFreeze>(), (int)(30 * multiplier));
            target.AddBuff(BuffType<HolyFlames>(), (int)(120 * multiplier));
            target.AddBuff(BuffID.Frostburn, (int)(150 * multiplier));
            target.AddBuff(BuffID.OnFire, (int)(180 * multiplier));
        }
        #endregion

        /// <summary>
        /// Makes the given player send the given packet to all appropriate receivers.<br />
        /// If server is false, the packet is sent only to the multiplayer host.<br />
        /// If server is true, the packet is sent to all clients except the player it pertains to.
        /// </summary>
        /// <param name="player">The player to whom the packet's data pertains.</param>
        /// <param name="packet">The packet to send with certain parameters.</param>
        /// <param name="server">True if a dedicated server is broadcasting information to all players.</param>
        public static void SendPacket(this Player player, ModPacket packet, bool server)
        {
            // Client: Send the packet only to the host.
            if (!server)
                packet.Send();

            // Server: Send the packet to every OTHER client.
            else
                packet.Send(-1, player.whoAmI);
        }
    }
}
