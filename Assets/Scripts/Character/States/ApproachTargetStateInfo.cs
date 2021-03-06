﻿using System;
using UnityEngine;
using UniRx;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;
using Utility;

[Category( "Character states" )]
public class ApproachTargetStateInfo : CharacterStateInfo {

    [Header( "Settings" )]
    public float minRange = 1.5f;

    public float maxRange = 4f;

    public bool autoActivate = true;

    public bool clearTargetOnReach = false;

    [Serializable]
    public class State : CharacterState<ApproachTargetStateInfo> {

        private TargetPosition destination;

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public override void Initialize( CharacterStateController stateController ) {

            base.Initialize( stateController );

            stateController.character.inputSource.targets.Subscribe( SetDestination );
        }

        public override bool CanBeSet() {

            return destination.HasValue
                   && character.pawn.planetTransform.GetDistanceTo( destination.Value ) >= typedInfo.minRange
                   && character.pawn.planetTransform.GetDistanceTo( destination.Value ) <= typedInfo.maxRange;
        }

        public override IEnumerable GetEvaluationBlock() {

            var pawn = character.pawn;

            pawn.canFollowDestination = true;

            do {

                yield return null;

                pawn.SetDestination( destination.Value );

            } while ( pawn.GetDistanceToDestination() > typedInfo.minRange && pawn.GetDistanceToDestination() < typedInfo.maxRange );

            pawn.canFollowDestination = false;

            if ( typedInfo.clearTargetOnReach ) {

                destination = null;
            }
        }

        public void SetDestination( object target ) {

            if ( target is Vector3 ) {

                destination = (Vector3) target;
            } else if ( target is Character ) {

                destination = ( target as Character ).pawn.transform;
            } else if ( target is ItemView ) {

                destination = ( target as ItemView ).transform;
            }

            if ( typedInfo.autoActivate ) {

                stateController.TrySetState( this );
            }
        }

        private void OnDestinationUpdate( Vector3 destination ) {

            this.destination = destination;

            stateController.TrySetState( this, allowEnterSameState: true );
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}