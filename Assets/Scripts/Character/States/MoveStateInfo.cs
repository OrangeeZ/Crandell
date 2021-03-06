﻿using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class MoveStateInfo : CharacterStateInfo {

    private class State : CharacterState<MoveStateInfo> {

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public override bool CanBeSet() {

            return GetMoveDirection().magnitude > 0;
        }

        public override IEnumerable GetEvaluationBlock() {

            while ( CanBeSet() ) {

                character.pawn.MoveDirection( GetMoveDirection() );

                yield return null;
            }
        }

        private Vector3 GetMoveDirection() {

            return GameScreen.instance.moveJoystick.GetValue();
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}