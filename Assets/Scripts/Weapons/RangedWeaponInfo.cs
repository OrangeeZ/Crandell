using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;
using UnityEngine.ScriptableObjectWizard;

[Category( "Weapons" )]
public class RangedWeaponInfo : WeaponInfo {

    [CalculatorExpression]
    [SerializeField]
    private StringReactiveProperty _damageExpression;

    [SerializeField]
    private Projectile _projectilePrefab;

    [SerializeField]
    private float _projectileSpeed;

    [SerializeField]
    private int _clipSize;

    [SerializeField]
    private float _reloadDuration;

    private class RangedWeapon : Weapon<RangedWeaponInfo> {

        private float _nextAttackTime;
        private int _ammoInClip;
        private readonly ReactiveCalculator _damageCalculator;

        public RangedWeapon( RangedWeaponInfo info ) : base( info ) {

            _ammoInClip = info._clipSize;
            _damageCalculator = new ReactiveCalculator( info._damageExpression );
            _damageCalculator.SubscribeProperty( "dangerLevel", GameplayController.instance.dangerLevel );
        }

        public override void Attack( Character target ) {

            if ( target == null || Time.timeSinceLevelLoad < _nextAttackTime ) {

                return;
            }

            if ( attackCallback != null ) {

                attackCallback();
            }

            var projectile = Instantiate( typedInfo._projectilePrefab );
            var targetDirection = character.pawn.planetTransform.GetDirectionTo( target.pawn.planetTransform );

            projectile.Launch( character, targetDirection, typedInfo._projectileSpeed, (int) _damageCalculator.Result.Value );

            if ( character.pawn.turret != null ) {

                character.pawn.turret.transform.localRotation = Quaternion.FromToRotation( Vector3.forward, targetDirection );
            }

            UpdateClipAndAttackTime();
        }

        public override void Attack( Vector3 direction ) {

            if ( Time.timeSinceLevelLoad < _nextAttackTime ) {

                return;
            }

            if ( attackCallback != null ) {

                attackCallback();
            }

            var projectile = Instantiate( typedInfo._projectilePrefab );
            projectile.Launch( character, direction, typedInfo._projectileSpeed, (int) _damageCalculator.Result.Value );

            UpdateClipAndAttackTime();
        }

        public override bool CanAttack( Character target ) {

            return target.pawn.planetTransform.GetDistanceTo( character.pawn.position ) <= typedInfo.attackRange;
        }

        private void UpdateClipAndAttackTime() {

            _ammoInClip--;

            if ( _ammoInClip == 0 ) {

                _ammoInClip = typedInfo._clipSize;

                _nextAttackTime = Time.timeSinceLevelLoad + typedInfo._reloadDuration;
            } else {

                _nextAttackTime = Time.timeSinceLevelLoad + typedInfo.baseAttackSpeed;
            }
        }

    }

    public override Item GetItem() {

        return new RangedWeapon( this );
    }

}