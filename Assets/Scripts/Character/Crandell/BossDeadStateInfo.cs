using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class BossDeadStateInfo : CharacterStateInfo {

    public struct Died : IEventBase {

    }

    [SerializeField]
    private Asteroid _asteroidPrefab;

    [SerializeField]
    private float _spinningSpeed;

    private class State : CharacterState<BossDeadStateInfo> {

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

            GameplayController.instance.dangerLevel.Value += 1;

            var spinningSpeedTimer = new AutoTimer( 1f );

            while ( character.pawn.planetTransform.height > 0 ) {

                character.pawn.rotation *= Quaternion.AngleAxis( spinningSpeedTimer.ValueNormalized * typedInfo._spinningSpeed * Time.deltaTime, Vector3.up );
                character.pawn.MoveDirection( Vector3.forward );

                yield return null;
            }

            character.pawn.SetActive( false );

            if ( 1f.Random() <= character.dropProbability && character.itemToDrop != null ) {

                character.itemToDrop.DropItem( character.pawn.transform );
            }

            character.pawn.MakeDead();

            Instantiate( typedInfo._asteroidPrefab, character.pawn.position, character.pawn.rotation );

            EventSystem.RaiseEvent( new Died() );

            while ( CanBeSet() ) {

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}