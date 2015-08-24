using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class EnemySpawner : MonoBehaviour {

	public Action<Character> Spawned;

	public CharacterInfo characterInfo;

	public ItemInfo[] startingItems;

	public WeaponInfo startingWeapon;

	public CameraBehaviour cameraBehaviour;

    public ItemInfo itemToDrop;
    public float dropProbability = 0.15f;

    public float spawnInterval;
	public float spawnMoveSpeed;
	public Vector2 spawnMoveVector;
	private float startTime;

	private Character character;

	private Planet _planet;
	public PlanetSurfaceTransform planetTransform;

	[Expressions.CalculatorExpression]
	public StringReactiveProperty activation;
	private Expressions.ReactiveCalculator _reactCalc;

    [Expressions.CalculatorExpression]
    public StringReactiveProperty deactivation;
	private Expressions.ReactiveCalculator _reactCalcDeact;

	private void Start() {
		_planet = FindObjectOfType<Planet>();
		planetTransform = new PlanetSurfaceTransform( _planet );
		_reactCalc = new Expressions.ReactiveCalculator (activation);
		_reactCalc.SubscribeProperty( "dangerLevel", GameplayController.instance.dangerLevel );

		_reactCalcDeact = new Expressions.ReactiveCalculator (deactivation);
		_reactCalcDeact.SubscribeProperty( "dangerLevel", GameplayController.instance.dangerLevel );

		SpawnThisEnemyNow11111 ();	

	}

	private void SpawnThisEnemyNow11111() {
		startTime = 0.0f;

		if (_reactCalc.Result.Value < 0) {
			return;
		}

		if (_reactCalcDeact.Result.Value >= 0) {
			return;
		}

		character = characterInfo.GetCharacter( startingPosition: transform.position );
		
		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {
			character.inventory.AddItem( each );
		}

	    character.itemToDrop = itemToDrop;
	    character.dropProbability = dropProbability;
		
		if ( startingWeapon != null ) {
			var weapon = startingWeapon.GetItem();
			character.inventory.AddItem( weapon );
			weapon.Apply();

		    //if ( characterInfo.applyColor ) {
		        
                character.pawn.SetColor( startingWeapon.color );
		    //}
		}
		
		if ( cameraBehaviour != null ) {
			var cameraBehaviourInstance = Instantiate( cameraBehaviour );
			cameraBehaviourInstance.transform.position = transform.position;
			cameraBehaviourInstance.SetTarget( character.pawn );
		}

	}

	private void Update() {
		planetTransform.Move (transform, spawnMoveVector, spawnMoveSpeed * Time.deltaTime);

		startTime += Time.deltaTime;
		if (startTime >= spawnInterval) {
			SpawnThisEnemyNow11111 ();
		}
	}
}
