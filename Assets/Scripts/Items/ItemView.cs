using UnityEngine;
using System.Collections;

public class ItemView : AObject {

	public Item item;

	public void NotifyPickUp( Character character ) {

		Destroy( gameObject );
	}

	//void OnTriggerEnter( Collider other ) {

	//	var pawn = other.GetComponent<CharacterPawn>();

	//	if ( pawn != null ) {

	//		targetInfo.Apply( pawn.character );
	//	}
	//}
}
