using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class DeadStateInfo : CharacterStateInfo {

    private class State : CharacterState<DeadStateInfo> {

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

            while ( CanBeSet() ) {

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}