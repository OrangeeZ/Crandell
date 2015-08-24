using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class SimpleSphereCollider : MonoBehaviour {

    public float radius = 1f;

    public Object userData;

    private Vector3 previousPosition;

    public Vector3 CalculatePunishingForce( SimpleSphereCollider otherCollider ) {

        var deltaVector = otherCollider.transform.position - transform.position;
        var maxDistance = otherCollider.radius + radius;

        if ( deltaVector.x >= maxDistance || deltaVector.y >= maxDistance || deltaVector.z >= maxDistance ) {

            return Vector3.zero;
        }

        var intersectionAmount = deltaVector.magnitude;

        var intersectionDirection = deltaVector / intersectionAmount;

        return intersectionDirection * ( maxDistance - intersectionAmount ).Clamped( 0, float.MaxValue );
    }

    public bool Intersects( SimpleSphereCollider otherCollider ) {

        if ( !otherCollider.enabled ) {

            return false;
        }

        return IntersectsInternal( transform.position, radius, otherCollider.transform.position, otherCollider.radius );
    }

    public SimpleSphereCollider IntersectsContinuous( IEnumerable<SimpleSphereCollider> otherColliders ) {

        var from = previousPosition;
        var to = transform.position;

        var interpolationStep = radius * 0.5f;
        var steps = Mathf.CeilToInt( ( to - from ).magnitude / interpolationStep );

        steps = Mathf.Max( steps, 1 );

        var currentPosition = from;

        for ( var i = 0; i < steps; i++ ) {

            foreach ( var each in otherColliders.Where( each => each.enabled ) ) {

                if ( IntersectsInternal( currentPosition, radius, each.transform.position, each.radius ) ) {

                    return each;
                }
            }

            currentPosition = Vector3.MoveTowards( currentPosition, to, interpolationStep );
        }

        return null;
    }

    public SimpleCapsuleCollider IntersectsContinuous( IEnumerable<SimpleCapsuleCollider> otherColliders ) {

        var from = previousPosition;
        var to = transform.position;

        var interpolationStep = radius * 0.5f;
        var steps = Mathf.CeilToInt( ( to - from ).magnitude / interpolationStep );

        steps = Mathf.Max( steps, 1 );

        var currentPosition = from;

        for ( var i = 0; i < steps; i++ ) {

            foreach ( var each in otherColliders.Where( each => each.enabled ) ) {

                if ( each.Intersects( this ) ) {

                    return each;
                }
            }

            currentPosition = Vector3.MoveTowards( currentPosition, to, interpolationStep );
        }

        return null;
    }

    public IEnumerable<SimpleSphereCollider> IntersectsMany( IEnumerable<SimpleSphereCollider> otherColliders ) {

        return otherColliders.Where( each => each != null ).Where( Intersects );
    }

    private bool IntersectsInternal( Vector3 thisPosition, float thisRadius, Vector3 otherPosition, float otherRadius ) {

        return ( thisPosition - otherPosition ).sqrMagnitude <= ( Mathf.Pow( thisRadius + otherRadius, 2 ) );
    }

    private void LateUpdate() {

        previousPosition = transform.position;
    }

    private void OnDrawGizmos() {

        var from = previousPosition;
        var to = transform.position;

        var interpolationStep = radius * 0.5f;
        var steps = Mathf.CeilToInt( ( to - from ).magnitude / interpolationStep );

        steps = Mathf.Max( steps, 1 );

        var currentPosition = from;

        for ( var i = 0; i < steps; i++ ) {

            Debug.DrawLine( currentPosition, ( currentPosition = Vector3.MoveTowards( currentPosition, to, interpolationStep ) ) * 0.9f );
        }

        Gizmos.DrawWireSphere( transform.position, radius );
        //Gizmos.DrawRay( transform.position, normal );
    }

}