using System;
using System.Collections.Generic;
using Expressions;
using UniRx;
using UnityEngine;

namespace Expressions {

	[Serializable]
	public class ReactiveCalculator : IObservable<double> {

		public double this[string key] {

			set {

				var oldValue = calculator.GetVariable( key );

				if ( oldValue != value ) {

					calculator.SetVariable( key, value );

					Recalculate();
				}
			}
		}

		public string Expression {

			get { return _expression; }
			set {

				if ( _expression != value ) {

					_expression = value;

					Recalculate();
				}
			}
		}

		public bool IsValid {

			get { return _isValid; }
			private set { _isValid = value; }
		}

		public DoubleReactiveProperty Result = new DoubleReactiveProperty( 0 );

		private Calculator calculator = new Calculator();

		private Dictionary<string, IDisposable> subscriptions = new Dictionary<string, IDisposable>();

		[SerializeField, HideInInspector]
		private string _expression;

		[SerializeField, HideInInspector]
		private bool _isValid;

		public ReactiveCalculator( IReactiveProperty<string> expression ) {

			subscriptions["Expression"] = expression.Subscribe( _ => Expression = _ );
		}

		~ReactiveCalculator() {

			foreach ( var each in subscriptions.Values ) {

				each.Dispose();
			}

			subscriptions = null;
		}

		public void SetExpression( string newExpression ) {

			if ( _expression != newExpression ) {

				_expression = newExpression;

				Recalculate();
			}
		}

		public void SubscribeProperty( string name, IReactiveProperty<double> property ) {

			if ( subscriptions.ContainsKey( name ) ) {

				subscriptions[name].Dispose();
			}

			subscriptions[name] = property.Subscribe( _ => this[name] = _ );
		}

		public void SubscribeProperty( string name, IReactiveProperty<int> property ) {

			if ( subscriptions.ContainsKey( name ) ) {

				subscriptions[name].Dispose();
			}

			subscriptions[name] = property.Subscribe( _ => this[name] = _ );
		}

		public IDisposable Subscribe( IObserver<double> observer ) {

			return Result.Subscribe( observer );
		}

		private void Recalculate() {

			try {

				calculator.Clear();
				Result.Value = calculator.Evaluate( _expression );

				IsValid = true;
			} catch {

				IsValid = false;
			}
		}
	}
}