using System;
using UniRx;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Packages.EventSystem;

public class Character {

    public struct Died : IEventBase {

        public Character character;

    }

    public static List<Character> instances = new List<Character>();

    public readonly IntReactiveProperty health;

    public readonly IInputSource inputSource;

    public readonly IInventory inventory;

    public readonly CharacterPlanetPawn pawn;

    public readonly CharacterStateController stateController;
    public readonly CharacterStateController weaponStateController;

    public readonly int teamId;
    public readonly CharacterInfo info;

    public readonly CharacterStatus status;

    private readonly StatExpressionsInfo statExpressions;

    private readonly IDisposable _healthDisposable;

    public Character( StatExpressionsInfo statExpressions, CharacterPlanetPawn pawn, IInputSource inputSource, CharacterStatus status, CharacterStateController stateController, CharacterStateController weaponStateController, int teamId, CharacterInfo info ) {

        this.statExpressions = statExpressions;
        this.status = status;
        this.health = new IntReactiveProperty( this.status.maxHealth.Value );
        this.pawn = pawn;
        this.inputSource = inputSource;
        this.stateController = stateController;
        this.weaponStateController = weaponStateController;
        this.teamId = teamId;
        this.info = info;
        this.inventory = new BasicInventory( this );

        pawn.SetCharacter( this );

        this.stateController.Initialize( this );
        this.weaponStateController.Initialize( this );

        Observable.EveryUpdate().Subscribe( OnUpdate );

        status.moveSpeed.Subscribe( UpdatePawnSpeed );

        _healthDisposable = health.Subscribe( OnHealthChange );

        instances.Add( this );
    }

    private void OnHealthChange( int health ) {

        if ( health <= 0 ) {

            instances.Remove( this );

            EventSystem.RaiseEvent( new Died {character = this} );

            _healthDisposable.Dispose();
        }
    }

    private void OnUpdate( long ticks ) {

        stateController.Tick( Time.deltaTime );
        weaponStateController.Tick( Time.deltaTime );
    }

    public void ApproachAndInteract( Character target ) {
    }

    public void ApproachAndInteract( ItemView target ) {
    }

    public void ApproachAndInteract( Vector3 target ) {
    }

    private void UpdatePawnSpeed( float speed ) {

        pawn.SetSpeed( speed );
    }

}