﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Particles
{
    public class SparkParticle : Particle
    {
        public Color InitialColor;
        public override bool SetLifetime => true;
        public override bool UseCustomDraw => true;
        public override bool UseAdditiveBlend => true;
        
        public override string Texture => "CalamityMod/Projectiles/StarProj";

        public SparkParticle(Vector2 relativePosition, Vector2 velocity, int lifetime, float scale, Color color)
        {
            Position = relativePosition;
            Velocity = velocity;
            Scale = scale;
            Lifetime = lifetime;
            Color = InitialColor = color;
        }

        public override void Update()
        {
            Scale *= 0.95f;
            Color = Color.Lerp(InitialColor, Color.Transparent, (float)Math.Pow(LifetimeCompletion, 3D));
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(0.35f, 1.6f) * Scale;
            Texture2D texture = ModContent.GetTexture(Texture);

            spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
        }
    }
}