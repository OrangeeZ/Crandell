using UnityEngine;
using System.Collections;
using System.Linq;
using Packages.EventSystem;

public static class Helpers {

    public struct SplashDamage : IEventBase {

        public Vector3 position;

    }

    public static void DoSplashDamage( Vector3 point, float radius, int amount ) {

        var affectedCharacters = Character.instances.Where( _ => ( _.pawn.position - point ).magnitude <= radius );
        foreach ( var each in affectedCharacters ) {

            each.health.Value -= amount;
        }

        EventSystem.RaiseEvent( new SplashDamage {position = point} );
    }

}