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

            character.pawn.SetGravityEnabled( true );

            character.pawn.SetTurretTarget( null );

            GameplayController.instance.dangerLevel.Value += 1;

            if ( stateController == character.stateController ) {

                character.pawn.SetActive( false );

                if ( 1f.Random() <= character.info.dropProbability && character.info.itemToDrop != null ) {

                    character.info.itemToDrop.DropItem( character.pawn.transform );
                }
            }

            while ( CanBeSet() ) {

                yield return null;
            }
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}