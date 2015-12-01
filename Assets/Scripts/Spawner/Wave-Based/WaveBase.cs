using System;
using UnityEngine;
using System.Collections;

public class WaveBase : MonoBehaviour {

	public GameObject[] Spawners;
	public float Frequency;
	public CharacterInfo characterInfo;
	public WeaponInfo startingWeapon;
	public WaveTriggerBase WaveTrigger;

	// Use this for initialization
	void Start () {
		WaveTrigger.OnTrigger += WaveTriggerOnOnTrigger;
	}
	private void WaveTriggerOnOnTrigger() {
		Spawn();
	}

	// Update is called once per frame
	void Update () {
	
	}

	private void Spawn() {
		Debug.Log( 123124 );
		foreach ( GameObject spawner in Spawners  ) {
			var character = characterInfo.GetCharacter( startingPosition: spawner.transform.position );
			character.itemToDrop = default ( ItemInfo ); /* fixit */
			if ( startingWeapon != null ) {
				var weapon = startingWeapon.GetItem();
				character.inventory.AddItem( weapon );
				weapon.Apply();

				//if ( characterInfo.applyColor ) {

				character.pawn.SetColor( startingWeapon.color );
				//}
			}
		}
	}
}
