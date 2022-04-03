using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.BaseProjectiles
{
    public abstract class BaseIdleHoldoutProjectile : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];

        public abstract int AssociatedItemID { get; }

        // The projectile type cannot be directly discerned at load time (Using Activator.CreateInstance will create an associated projectile with an ID of 0) and as
        // such this property exists as a direct means of retrieving it. Not ideal, but it works.
        public abstract int IntendedProjectileType { get; }
        public static Dictionary<int, int> ItemProjectileRelationship = new Dictionary<int, int>();

        public static void Load()
        {
            ItemProjectileRelationship = new Dictionary<int, int>();

            // Look through every type in the mod, and check if it's derived from BaseIdleHoldoutProjectile.
            // If it is, cache it into the item/projectile relationship cache.
            foreach (Type type in typeof(CalamityMod).Assembly.GetTypes())
            {
                // Don't load abstract classes; they cannot have instances.
                if (type.IsAbstract)
                    continue;

                if (type.IsSubclassOf(typeof(BaseIdleHoldoutProjectile)))
                {
                    BaseIdleHoldoutProjectile instance = Activator.CreateInstance(type) as BaseIdleHoldoutProjectile;
                    ItemProjectileRelationship[instance.AssociatedItemID] = instance.IntendedProjectileType;
                }
            }
        }

        public static void CheckForEveryHoldout(Player player)
        {
            foreach (int itemID in ItemProjectileRelationship.Keys)
            {
                Item heldItem = player.ActiveItem();
                if (heldItem.type != itemID)
                    continue;

                bool bladeIsPresent = false;
                int holdoutType = ItemProjectileRelationship[itemID];
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type != holdoutType || Main.projectile[i].owner != player.whoAmI || !Main.projectile[i].active)
                        continue;

                    bladeIsPresent = true;
                    break;
                }

                if (Main.myPlayer == player.whoAmI && !bladeIsPresent)
                {
                    int damage = heldItem.damage;

                    // TODO - This can probably be simplified a good bit in 1.4.
                    if (heldItem.melee)
                        damage = (int)(damage * player.MeleeDamage());
                    if (heldItem.ranged)
                        damage = (int)(damage * player.RangedDamage());
                    if (heldItem.magic)
                        damage = (int)(damage * player.MagicDamage());
                    if (heldItem.summon)
                        damage = (int)(damage * player.MinionDamage());
                    if (heldItem.Calamity().rogue)
                        damage = (int)(damage * player.RogueDamage());

                    float kb = player.GetWeaponKnockback(heldItem, heldItem.knockBack);
                    Projectile.NewProjectile(player.Center, Vector2.Zero, holdoutType, damage, kb, player.whoAmI);
                }
            }
        }

        public sealed override void AI()
        {
            CheckForEveryHoldout(Owner);
            if (Owner.ActiveItem().type != AssociatedItemID || Owner.CCed || !Owner.active || Owner.dead)
            {
                Projectile.Kill();
                return;
            }
            SafeAI();
        }

        public virtual void SafeAI() { }
    }
}
