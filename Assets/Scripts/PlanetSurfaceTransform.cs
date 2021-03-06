﻿using UnityEngine;
using System.Collections;

public class PlanetSurfaceTransform {

    public float height { get; private set; }

    public Quaternion rotation = Quaternion.identity;

    private readonly Planet _planet;

    public PlanetSurfaceTransform( Planet planet ) {

        _planet = planet;
    }

    public void Move( Transform transform, Vector3 direction, float speed ) {

        var distance = height + _planet.radius;

        var circleLength = 2f * Mathf.PI * distance;
        var angularMoveSpeed = speed / circleLength;
        angularMoveSpeed = angularMoveSpeed * 360f;

        var zAngle = direction.z * angularMoveSpeed;
        var xAngle = -direction.x * angularMoveSpeed;

        transform.rotation *= Quaternion.AngleAxis( zAngle, Vector3.right ) *
                              Quaternion.AngleAxis( xAngle, Vector3.forward );

        rotation = transform.rotation;

        height += direction.y * speed;

        transform.position = transform.rotation * Vector3.up * distance;
    }

    public Vector3 GetDirectionTo( PlanetSurfaceTransform otherTransform ) {

        var relativePosition = ( otherTransform.rotation ) * Vector3.up * _planet.radius - rotation * Vector3.up * ( _planet.radius );
        //var result = new Vector3( relativeRotation.eulerAngles.z, 0, relativeRotation.eulerAngles.y );

        var result = Quaternion.Inverse( rotation ) * relativePosition;
        result.y = ( otherTransform.height ) - height;
        //result.y = ( ( destination - _planet.transform.transform ).magnitude - _planet.radius ) - height;

        return result.normalized;

        //var result = Quaternion.Inverse( rotation ) * ( otherTransform.rotation ) * Vector3.up;

        //result.y = otherTransform.height - height;

        //return result.normalized;
    }

    public Vector3 GetDirectionTo( Vector3 destination ) {

        var relativePosition = ( Quaternion.FromToRotation( Vector3.up, ( destination - _planet.transform.position ) ) ) * Vector3.up * _planet.radius - rotation * Vector3.up * ( _planet.radius );

        //Debug.DrawLine( rotation * Vector3.up * ( _planet.radius + height ), rotation * Vector3.up * ( _planet.radius + height ) + relativePosition );
        //Debug.DrawRay(rotation * Vector3.up * (_planet.radius + height), relativeRotation.eulerAngles);

        //var result = new Vector3( relativeRotation.eulerAngles.x - 180f, 0, relativeRotation.eulerAngles.y - 180f );

        //var result = new Vector3();
        //result.x = Vector3.Angle( Vector3.right, relativePosition.Set( y: 0 ) );
        //result.z = relativePosition.z;
        //result.z = Vector3.Angle(  );

        var result = Quaternion.Inverse( rotation ) * relativePosition;

        //Debug.DrawLine( rotation * Vector3.up * ( _planet.radius + height ), relativeRotation * Vector3.forward );

        //Debug.DrawLine( rotation * Vector3.up * ( _planet.radius + height ), ( relativeRotation ) * Vector3.up * ( _planet.radius + height ) );
        //Debug.DrawLine( rotation * Vector3.up * ( _planet.radius + height ), destination );

        result.y = ( ( destination - _planet.transform.position ).magnitude - _planet.radius ) - height;

        return result.normalized;
    }

    public void MoveTowards( Transform transform, PlanetSurfaceTransform otherTransform, float speed ) {

        Move( transform, GetDirectionTo( otherTransform ), speed );
    }

    public void MoveTowards( Transform transform, Vector3 destination, float speed ) {

        Move( transform, GetDirectionTo( destination ), speed );
    }

    public float GetDistanceTo( Vector3 destination ) {

        var result = Vector3.Angle( ( Quaternion.FromToRotation( Vector3.up, ( destination - _planet.transform.position ) ) ) * Vector3.up * _planet.radius, rotation * Vector3.up * ( _planet.radius ) ) * Mathf.Deg2Rad * _planet.radius;

        return result;
    }

    public void SetHeight( float height ) {

        this.height = height.Clamped( 0, float.MaxValue );
    }

    public void SetPosition( Transform transform ) {

        rotation = Quaternion.FromToRotation( Vector3.up, transform.position );

        transform.rotation = rotation;
        transform.position = rotation * Vector3.up * ( _planet.radius + height );
    }

    public void UpdatePosition( Transform transform ) {

        transform.position = rotation * Vector3.up * ( _planet.radius + height );
    }

}