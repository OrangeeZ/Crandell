using System.Collections;
using System.Linq;
using UnityEngine.ScriptableObjectWizard;
using UniRx;

namespace AI.Gambits {

	[Category( "Gambits" )]
	public class AttackOnVisibleGambit : GambitInfo {

		private class GambitInternal : Gambit {

			private readonly GambitInfo _info;

			private Character attackTarget;

			public GambitInternal( GambitInfo info, Character character )
				: base( character ) {

				this._info = info;
				character.pawn.GetSphereSensor().Select( _ => _.character ).Subscribe( OnTargetEnter );
			}

			public override bool Execute() {

				if ( attackTarget != null ) {

					character.stateController.GetState<ApproachTargetStateInfo.State>().SetDestination( attackTarget );
					character.weaponStateController.GetState<AttackStateInfo.State>().SetTarget( attackTarget );
                    
					return true;
				}

				return false;
			}

			private void OnTargetEnter( Character target ) {

			    if ( target.teamId != character.teamId ) {

                    this.attackTarget = target;
			    }
			}
		}

		public override Gambit GetGambit( Character target ) {

			return new GambitInternal( this, target );
		}
	}
}