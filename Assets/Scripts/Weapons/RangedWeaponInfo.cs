using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Weapons" )]
public class RangedWeaponInfo : WeaponInfo {

    [SerializeField]
    private Projectile _projectilePrefab;

    [SerializeField]
    private float _projectileSpeed;

    private class RangedWeapon : Weapon<RangedWeaponInfo> {

        private float nextAttackTime;

        public RangedWeapon( RangedWeaponInfo info ) : base( info ) {

        }

        public override void Attack( Character target ) {

            if ( target == null || Time.timeSinceLevelLoad < nextAttackTime ) {

                return;
            }

            if ( attackCallback != null ) {

                attackCallback();
            }

            var projectile = Instantiate( typedInfo._projectilePrefab );
            projectile.Launch( character, character.pawn.planetTransform.GetDirectionTo( target.pawn.planetTransform ), typedInfo._projectileSpeed );

            //target.health.Value -= typedInfo.baseDamage;

            nextAttackTime = Time.timeSinceLevelLoad + typedInfo.baseAttackSpeed;
        }

        public override void Attack( Vector3 direction ) {

            if ( Time.timeSinceLevelLoad < nextAttackTime ) {

                return;
            }

            if ( attackCallback != null ) {

                attackCallback();
            }

            var projectile = Instantiate( typedInfo._projectilePrefab );
            projectile.Launch( character, direction, typedInfo._projectileSpeed );

            //target.health.Value -= typedInfo.baseDamage;

            nextAttackTime = Time.timeSinceLevelLoad + typedInfo.baseAttackSpeed;
        }

        public override bool CanAttack( Character target ) {

            return target.pawn.planetTransform.GetDistanceTo( character.pawn.position ) <= typedInfo.attackRange;
        }

    }

    public override Item GetItem() {

        return new RangedWeapon( this );
    }

}