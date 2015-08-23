using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public int health = 5;

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