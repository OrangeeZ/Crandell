using UnityEngine;
using System.Collections;
using System.Linq;

public static class Helpers {

    public static void DoSplashDamage( Vector3 point, float radius, int amount ) {

        var affectedCharacters = Character.instances.Where( _ => ( _.pawn.position - point ).magnitude <= radius );
        foreach ( var each in affectedCharacters ) {

            each.health.Value -= amount;
        }
    }

}