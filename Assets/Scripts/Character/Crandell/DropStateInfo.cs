using UnityEngine;
using System.Collections;
using Screen = UnityEngine.Screen;

public class DropStateInfo : CharacterStateInfo {

    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _damageRadius;

    [SerializeField]
    private float _delay = 0.5f;

    private class State : CharacterState<DropStateInfo> {

        private bool _wasSet;
        private bool _isLocked;

        public State( CharacterStateInfo info ) : base( info ) {
        }

        public override bool CanBeSet() {

            return !_wasSet;
        }

        public override bool CanSwitchTo( CharacterState nextState ) {

            return !_isLocked && base.CanSwitchTo( nextState );
        }

        public override IEnumerable GetEvaluationBlock() {

            _wasSet = true;
            _isLocked = false;

            character.pawn.SetGravityEnabled( true );

            while ( character.pawn.planetTransform.height > 0 ) {

                yield return null;
            }

            Helpers.DoSplashDamage( character.pawn.position, typedInfo._damageRadius, typedInfo._damage );

            character.pawn.SetGravityEnabled( false );

            var timer = new AutoTimer( typedInfo._delay );

            while ( timer.ValueNormalized < 1 ) {

                yield return null;
            }

            ScreenManager.GetWindow<GameScreen>().Show();

            _isLocked = false;
        }

    }

    public override CharacterState GetState() {

        return new State( this );
    }

}