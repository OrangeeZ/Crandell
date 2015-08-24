using UnityEngine;
using System.Collections;
using System.Linq;
using Packages.EventSystem;

public static class Helpers {

    public struct SplashDamage : IEventBase {

        public Vector3 position;
        public float radius;

    }

    public static void DoSplashDamage( Vector3 point, float radius, int amount ) {

        var affectedCharacters = Character.instances.Where( _ => ( _.pawn.position - point ).magnitude <= radius );
        foreach ( var each in affectedCharacters ) {

            each.health.Value -= amount;
        }

        var affectedBuildings = Building.instances.Where( _ => _.sphereCollider.Intersects( point, radius ) );
        foreach ( var each in affectedBuildings ) {

            each.Hit( amount );
        }

        EventSystem.RaiseEvent( new SplashDamage {position = point, radius = radius * 0.5f} );
    }

}