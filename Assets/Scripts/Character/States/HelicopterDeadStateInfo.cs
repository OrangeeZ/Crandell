using UnityEngine;
using System.Collections;
using UniRx;

public class HelicopterDeadStateInfo : CharacterStateInfo {

    [SerializeField]
    private float _spinningSpeed;
    
    [SerializeField]
    private int _impactDamage;

    private class State : CharacterState<HelicopterDeadStateInfo> {

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

            while ( true ) {

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}