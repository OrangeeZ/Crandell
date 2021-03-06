﻿using UnityEngine;
using System.Collections;

public class ItemPickupListener : MonoBehaviour {

    [SerializeField]
    private CharacterPawn _pawn;

    private void Reset() {

        this.GetComponent( out _pawn );
    }

    private void OnTriggerEnter( Collider other ) {

        var itemView = other.GetComponent<ItemView>();

        if ( itemView != null ) {

            _pawn.character.inventory.AddItem( itemView.item );
            itemView.item.Apply();

            itemView.NotifyPickUp( _pawn.character );
        }
    }

}