using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

    public static List<Building> instances = new List<Building>();

    public int health = 5;

    public SimpleSphereCollider sphereCollider;

    public EffectsBase[] effects;

    private void Start() {

        instances.Add( this );
    }

    private void OnDestroy() {

        instances.Remove( this );
    }

    public virtual void Hit( Projectile projectile ) {

        for ( var i = health - projectile.damage; i <= health; i++ ) {

            if ( i >= 0 && i < effects.Length ) {

                effects[i].Activate();
            }
        }

        health -= projectile.damage;

        if ( health < 0 ) {

            Destroy();
        }
    }

    public virtual void Destroy() {

        GetComponent<Collider>().enabled = false;
        //Destroy( gameObject );
    }

}