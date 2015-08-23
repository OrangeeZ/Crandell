using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

    public static List<Building> instances  = new List<Building>();

    public int health = 5;

    public SimpleSphereCollider sphereCollider;

    void Start() {
        
        instances.Add( this );
    }

    void OnDestroy() {

        instances.Remove( this );
    }

    public virtual void Hit( Projectile projectile ) {

        health -= projectile.damage;

        if ( health < 0 ) {

            Destroy();
        }
    }

    public virtual void Destroy() {

        Destroy( gameObject );
    }

}