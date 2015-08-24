using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class CrandellDeadStateInfo : CharacterStateInfo {

    [SerializeField]
    private float _delay;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _damageRadius;

    [SerializeField]
    private AudioClip _deathSound;

    private class State : CharacterState<CrandellDeadStateInfo> {

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public override void Initialize( CharacterStateController stateController ) {

            base.Initialize( stateController );

            character.health.Where( _ => _ <= 0 ).Subscribe( _ => stateController.TrySetState( this ) );
        }

        public override bool CanBeSet() {

            return character.health.Value <= 0;
        }

        public override IEnumerable GetEvaluationBlock() {

            character.pawn.ClearDestination();

            character.pawn.SetGravityEnabled( true );

            character.pawn.SetTurretTarget( null );

            GameplayController.instance.dangerLevel.Value += 1;

            if ( stateController == character.stateController ) {

                character.pawn.SetActive( false );

                if ( 1f.Random() <= character.dropProbability && character.itemToDrop != null ) {

                    character.itemToDrop.DropItem( character.pawn.transform );
                }
            }

            AudioSource.PlayClipAtPoint( typedInfo._deathSound, character.pawn.position, 0.4f );

            var timer = new AutoTimer(typedInfo._delay);

            while ( timer.ValueNormalized < 1 ) {

                yield return null;
            }

            Helpers.DoSplashDamage( character.pawn.position, typedInfo._damageRadius, typedInfo._damage );

            while ( CanBeSet() ) {

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}