﻿using System.Linq;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

[Category( "Character states" )]
public class FollowThenAttackStateInfo : CharacterStateInfo {

	public class State : CharacterState<FollowThenAttackStateInfo> {

		private FollowTargetStateInfo.State followState;

		private AttackStateInfo.State attackState;

		private CharacterPawn target;

		public State( CharacterStateInfo info )
			: base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			followState = stateController.GetStateByInfo( typedInfo.followStateInfo ) as FollowTargetStateInfo.State;
			attackState = stateController.GetStateByInfo( typedInfo.attackStateInfo ) as AttackStateInfo.State;

			stateController.character.pawn.GetSphereSensor().Subscribe( OnPawnClicked );
		}

		public override bool CanBeSet() {

			return target != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			stateController.SetScheduledStates( new CharacterState[] { followState, attackState } );

			yield return null;
		}

		private void OnPawnClicked( CharacterPawn characterPawn ) {

			followState.SetTarget( characterPawn );
			attackState.SetTarget( characterPawn.character );

			stateController.TrySetState( this );
		}
	}

	[SerializeField]
	private CharacterStateInfo followStateInfo;

	[SerializeField]
	private CharacterStateInfo attackStateInfo;

	public override CharacterState GetState() {

		return new State( this );
	}
}
