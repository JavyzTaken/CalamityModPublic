using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace CalamityMod
{
	public static partial class CalamityUtils
	{

		internal static readonly List<Vector2> Directions = new List<Vector2>()
		{
			new Vector2(-1f, -1f),
			new Vector2(1f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0f, -1f),
			new Vector2(-1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
		};

		/// <summary>
		/// Computes 2-dimensional Perlin Noise, which gives "random" but continuous values.
		/// </summary>
		/// <param name="x">The X position on the map.</param>
		/// <param name="y">The Y position on the map.</param>
		/// <param name="octaves">A metric of "instability" of the noise. The higher this is, the more unstable. Lower of bounds of 2-3 are preferable.</param>
		/// <param name="seed">The seed for the noise.</param>
		/// <returns></returns>
		public static float PerlinNoise2D(float x, float y, int octaves, int seed)
		{
			float SmoothFunction(float n) => 3f * n * n - 2f * n * n * n;
			float NoiseGradient(int s, int noiseX, int noiseY, float xd, float yd)
			{
				int hash = s;
				hash ^= 1619 * noiseX;
				hash ^= 31337 * noiseY;

				hash = hash * hash * hash * 60493;
				hash = (hash >> 13) ^ hash;

				Vector2 g = Directions[hash & 7];

				return xd * g.X + yd * g.Y;
			}

			int frequency = (int)Math.Pow(2D, octaves);
			x *= frequency;
			y *= frequency;

			int flooredX = (int)x;
			int flooredY = (int)y;
			int ceilingX = flooredX + 1;
			int ceilingY = flooredY + 1;
			float interpolatedX = x - flooredX;
			float interpolatedY = y - flooredY;
			float interpolatedX2 = interpolatedX - 1;
			float interpolatedY2 = interpolatedY - 1;

			float fadeX = SmoothFunction(interpolatedX);
			float fadeY = SmoothFunction(interpolatedY);

			float smoothX = MathHelper.Lerp(NoiseGradient(seed, flooredX, flooredY, interpolatedX, interpolatedY), NoiseGradient(seed, ceilingX, flooredY, interpolatedX2, interpolatedY), fadeX);
			float smoothY = MathHelper.Lerp(NoiseGradient(seed, flooredX, ceilingY, interpolatedX, interpolatedY2), NoiseGradient(seed, ceilingX, ceilingY, interpolatedX2, interpolatedY2), fadeX);
			return MathHelper.Lerp(smoothX, smoothY, fadeY);
		}

		/// <summary>
		/// Computes the Manhattan Distance between two points. This is typically used as a cheaper alternative to Euclidean Distance.
		/// </summary>
		/// <param name="a">The first point.</param>
		/// <param name="b">The second point.</param>
		public static float ManhattanDistance(this Vector2 a, Vector2 b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

		/// <summary>
		/// Wraps an angle between -90 and 90 degrees. If an angle goes past this rangle it'll loop back to the other end.
		/// </summary>
		/// <param name="theta"></param>
		/// <returns></returns>
		public static float WrapAngle90Degrees(float theta)
        {
			// Ensure that the angle starts off in the -180 to 180 degree range instead of the 0 to 360 degree range.
			if (theta > MathHelper.Pi)
				theta -= MathHelper.Pi;

			if (theta > MathHelper.PiOver2)
				theta -= MathHelper.Pi;
			if (theta < -MathHelper.PiOver2)
				theta += MathHelper.Pi;

			return theta;
		}
		
		/// <summary>
		/// Determines the angular distance between two vectors based on dot product comparisons. This method ensures underlying normalization is performed safely.
		/// </summary>
		/// <param name="v1">The first vector.</param>
		/// <param name="v2">The second vector.</param>
		public static float AngleBetween(this Vector2 v1, Vector2 v2) => (float)Math.Acos(Vector2.Dot(v1.SafeNormalize(Vector2.Zero), v2.SafeNormalize(Vector2.Zero)));

		// NOTE: A similar function to this one exists in 1.4, but it is not the same underlying function. Check Turn01ToCyclic010 in Utils.cs to see the effect.

		/// <summary>
		/// Converts a 0-1 bound to a 0-1-0 bump. This function automatically clamps the value to the necessary 0-1 range.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static float Convert01To010(float value) => (float)Math.Sin(MathHelper.Pi * MathHelper.Clamp(value, 0f, 1f));

		/// <summary>
		/// Uses a rewritten horizontal range formula to determine the direction to fire a projectile in order for it to hit a destination. Falls back on a certain value if no such direction can exist. If no fallback is provided, a clamp is used.
		/// </summary>
		/// <param name="shootingPosition">The starting position of the projectile.</param>
		/// <param name="destination">The destination for the projectile to land at.</param>
		/// <param name="gravity">The gravity of the projectile.</param>
		/// <param name="shootSpeed">The magnitude </param>
		/// <param name="nanFallback">The direction to fall back to if the calculations result in any NaNs. If nothing is specified, a clamp is performed to prevent any chance of NaNs at all.</param>
		public static Vector2 GetProjectilePhysicsFiringVelocity(Vector2 shootingPosition, Vector2 destination, float gravity, float shootSpeed, Vector2? nanFallback = null)
		{
			// Ensure that the gravity has the right sign for Terraria's coordinate system.
			gravity = -Math.Abs(gravity);

			float horizontalRange = MathHelper.Distance(shootingPosition.X, destination.X);
			float fireAngleSine = gravity * horizontalRange / (float)Math.Pow(shootSpeed, 2);

			// Clamp the sine if no fallback is provided.
			if (nanFallback is null)
				fireAngleSine = MathHelper.Clamp(fireAngleSine, -1f, 1f);

			float fireAngle = (float)Math.Asin(fireAngleSine) * 0.5f;

			// Get out of here if no valid firing angle exists. This can only happen if a fallback does indeed exist.
			if (float.IsNaN(fireAngle))
				return nanFallback.Value * shootSpeed;

			Vector2 fireVelocity = new Vector2(0f, -shootSpeed).RotatedBy(fireAngle);
			fireVelocity.X *= (destination.X - shootingPosition.X < 0).ToDirectionInt();
			return fireVelocity;
		}

		// REMOVE THIS IN CALAMITY 1.4, it's a 1.4 World.cs function.
		// Due to its temporary state, this method will not receive an XML documentation comment.
		public static Rectangle ClampToWorld(Rectangle tileRectangle)
		{
			int num = Math.Max(0, Math.Min(tileRectangle.Left, Main.maxTilesX));
			int num2 = Math.Max(0, Math.Min(tileRectangle.Top, Main.maxTilesY));
			int num3 = Math.Max(0, Math.Min(tileRectangle.Right, Main.maxTilesX));
			int num4 = Math.Max(0, Math.Min(tileRectangle.Bottom, Main.maxTilesY));
			return new Rectangle(num, num2, num3 - num, num4 - num2);
		}

		// REMOVE THIS IN CALAMITY 1.4, it's a 1.4 Utils.cs function.
		// Due to its temporary state, this method will not receive an XML documentation comment.
		public static Vector2 MoveTowards(this Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
		{
			Vector2 v = targetPosition - currentPosition;
			if (v.Length() < maxAmountAllowedToMove)
				return targetPosition;

			return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
		}
	}
}
