using CalamityMod.Skies;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalamityMod.Effects
{
    public class CalamityShaders
    {
        public static Effect AstralFogShader;
        public static Effect LightShader;
        public static Effect SCalMouseShader;
        public static Effect TentacleShader;
        public static Effect TeleportDisplacementShader;
        public static Effect LightDistortionShader;
        public static Effect PhaseslayerRipShader;
        public static Effect FadedUVMapStreakShader;
        public static Effect FlameStreakShader;
        public static Effect FadingSolidTrailShader;
        public static Effect ScarletDevilShader;
        public static Effect BordernadoFireShader;
        public static Effect PrismCrystalShader;
        public static Effect ImpFlameTrailShader;
        public static Effect SCalShieldShader;
        public static Effect RancorMagicCircleShader;
        public static Effect BasicTintShader;
        public static Effect CircularBarShader;
        public static Effect CircularBarSpriteShader;
        public static Effect DoGDisintegrationShader;
        public static Effect ArtAttackTrailShader;
        public static Effect CircularAoETelegraph;
        public static Effect IntersectionClipShader;
        public static Effect LocalLinearTransformationShader;

        public static Effect BaseFusableParticleEdgeShader;
        public static Effect AdditiveFusableParticleEdgeShader;

        public static Effect DoGPortalShader;

        public static void LoadShaders()
        {
            if (Main.dedServ)
                return;

            AstralFogShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/CustomShader").Value;
            LightShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/LightBurstShader").Value;
            TentacleShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/TentacleShader").Value;
            TeleportDisplacementShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/TeleportDisplacementShader").Value;
            SCalMouseShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/SCalMouseShader").Value;
            LightDistortionShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/DistortionShader").Value;
            PhaseslayerRipShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/PhaseslayerRipShader").Value;
            ScarletDevilShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/ScarletDevilStreak").Value;
            BordernadoFireShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/BordernadoFire").Value;
            PrismCrystalShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/PrismCrystalStreak").Value;
            FadedUVMapStreakShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/FadedUVMapStreak").Value;
            FlameStreakShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/Flame").Value;
            FadingSolidTrailShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/FadingSolidTrail").Value;
            ImpFlameTrailShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/ImpFlameTrail").Value;
            SCalShieldShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/SupremeShieldShader").Value;
            RancorMagicCircleShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/RancorMagicCircle").Value;
            BasicTintShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/BasicTint").Value;
            CircularBarShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/CircularBarShader").Value;
            CircularBarSpriteShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/CircularBarSpriteShader").Value;
            DoGDisintegrationShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/DoGDisintegration").Value;
            ArtAttackTrailShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/ArtAttackTrail").Value;
            CircularAoETelegraph = CalamityMod.Instance.Assets.Request<Effect>("Effects/CircularAoETelegraph").Value;
            IntersectionClipShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/IntersectionClipShader").Value;
            LocalLinearTransformationShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/LocalLinearTransformationShader").Value;

            BaseFusableParticleEdgeShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/ParticleFusion/BaseFusableParticleEdgeShader").Value;
            AdditiveFusableParticleEdgeShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/ParticleFusion/AdditiveFusableParticleEdgeShader").Value;

            DoGPortalShader = CalamityMod.Instance.Assets.Request<Effect>("Effects/ScreenShaders/DoGPortalShader").Value;

            Filters.Scene["CalamityMod:Astral"] = new Filter(new AstralScreenShaderData(new Ref<Effect>(AstralFogShader), "AstralPass").UseColor(0.18f, 0.08f, 0.24f), EffectPriority.VeryHigh);

            Filters.Scene["CalamityMod:LightBurst"] = new Filter(new ScreenShaderData(new Ref<Effect>(LightShader), "BurstPass"), EffectPriority.VeryHigh);
            Filters.Scene["CalamityMod:LightBurst"].Load();

            GameShaders.Misc["CalamityMod:FireMouse"] = new MiscShaderData(new Ref<Effect>(SCalMouseShader), "DyePass");
            GameShaders.Misc["CalamityMod:SubsumingTentacle"] = new MiscShaderData(new Ref<Effect>(TentacleShader), "BurstPass");
            GameShaders.Misc["CalamityMod:TeleportDisplacement"] = new MiscShaderData(new Ref<Effect>(TeleportDisplacementShader), "GlitchPass");
            GameShaders.Misc["CalamityMod:LightDistortion"] = new MiscShaderData(new Ref<Effect>(LightDistortionShader), "DistortionPass");
            GameShaders.Misc["CalamityMod:PhaseslayerRipEffect"] = new MiscShaderData(new Ref<Effect>(PhaseslayerRipShader), "TrailPass");
            GameShaders.Misc["CalamityMod:TrailStreak"] = new MiscShaderData(new Ref<Effect>(FadedUVMapStreakShader), "TrailPass");
            GameShaders.Misc["CalamityMod:Flame"] = new MiscShaderData(new Ref<Effect>(FlameStreakShader), "TrailPass");
            GameShaders.Misc["CalamityMod:FadingSolidTrail"] = new MiscShaderData(new Ref<Effect>(FadingSolidTrailShader), "TrailPass");
            GameShaders.Misc["CalamityMod:OverpoweredTouhouSpearShader"] = new MiscShaderData(new Ref<Effect>(ScarletDevilShader), "TrailPass");
            GameShaders.Misc["CalamityMod:Bordernado"] = new MiscShaderData(new Ref<Effect>(BordernadoFireShader), "TrailPass");
            GameShaders.Misc["CalamityMod:PrismaticStreak"] = new MiscShaderData(new Ref<Effect>(PrismCrystalShader), "TrailPass");
            GameShaders.Misc["CalamityMod:ImpFlameTrail"] = new MiscShaderData(new Ref<Effect>(ImpFlameTrailShader), "TrailPass");
            GameShaders.Misc["CalamityMod:SupremeShield"] = new MiscShaderData(new Ref<Effect>(SCalShieldShader), "ShieldPass");
            GameShaders.Misc["CalamityMod:RancorMagicCircle"] = new MiscShaderData(new Ref<Effect>(RancorMagicCircleShader), "ShieldPass");
            GameShaders.Misc["CalamityMod:BasicTint"] = new MiscShaderData(new Ref<Effect>(BasicTintShader), "TintPass");
            GameShaders.Misc["CalamityMod:CircularBarShader"] = new MiscShaderData(new Ref<Effect>(CircularBarShader), "Pass0");
            GameShaders.Misc["CalamityMod:CircularBarSpriteShader"] = new MiscShaderData(new Ref<Effect>(CircularBarSpriteShader), "Pass0");
            GameShaders.Misc["CalamityMod:DoGDisintegration"] = new MiscShaderData(new Ref<Effect>(DoGDisintegrationShader), "DisintegrationPass");
            GameShaders.Misc["CalamityMod:ArtAttack"] = new MiscShaderData(new Ref<Effect>(ArtAttackTrailShader), "TrailPass");
            GameShaders.Misc["CalamityMod:CircularAoETelegraph"] = new MiscShaderData(new Ref<Effect>(CircularAoETelegraph), "TelegraphPass");
            GameShaders.Misc["CalamityMod:IntersectionClip"] = new MiscShaderData(new Ref<Effect>(IntersectionClipShader), "ClipPass");
            GameShaders.Misc["CalamityMod:LinearTransformation"] = new MiscShaderData(new Ref<Effect>(LocalLinearTransformationShader), "TransformationPass");

            GameShaders.Misc["CalamityMod:BaseFusableParticleEdge"] = new MiscShaderData(new Ref<Effect>(BaseFusableParticleEdgeShader), "ParticlePass");
            GameShaders.Misc["CalamityMod:AdditiveFusableParticleEdge"] = new MiscShaderData(new Ref<Effect>(AdditiveFusableParticleEdgeShader), "ParticlePass");

            GameShaders.Misc["CalamityMod:DoGPortal"] = new MiscShaderData(new Ref<Effect>(DoGPortalShader), "ScreenPass");
        }
    }
}
