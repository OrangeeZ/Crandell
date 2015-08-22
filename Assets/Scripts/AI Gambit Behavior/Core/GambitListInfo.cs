using System.Collections.Generic;
using System.Linq;
using System.Monads;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.ScriptableObjectWizard;

namespace AI.Gambits {

	public interface IGambitList {

		void Tick();
	}

	[Category( "Gambits" )]
	public class GambitListInfo : ScriptableObject {

		[SerializeField]
		private GambitInfo[] gambitInfos;

		public class GambitList : IInputSource {

			public IObservable<Vector3> moveInput { get; private set; }

			public IReadOnlyReactiveProperty<object> targets { get; private set; }

			private IList<Gambit> gambits;

			private GambitListInfo gambitListInfo;

			public GambitList( GambitListInfo gambitListInfo ) {

				this.gambitListInfo = gambitListInfo;

				moveInput = new Subject<Vector3>();
				targets = new ReactiveProperty<object>();
			}

			public void Initialize( Character character ) {

				this.gambits = gambitListInfo.gambitInfos.Select( _ => _.GetGambit( character ) ).ToArray();
				
				Observable.EveryUpdate().Subscribe( Tick );
			}

			private void Tick( long ticks ) {

				gambits.FirstOrDefault( thatCan => thatCan.Execute() );
			}
		}

		public GambitList GetGambitList() {

			return new GambitList( this );
		}
	}
}