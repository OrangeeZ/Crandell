using UnityEngine;
using System.Collections;

public class Crandell : MonoBehaviour {

    public float moveSpeed = 5f;

    public CharacterController characterController;

    public Projectile projectile;

    public UIJoystick moveJoystick;
    public UIJoystick attackJoystick;

    public PlanetSurfaceTransform planetTransform;

    private Planet _planet;

    private void Start() {

        _planet = FindObjectOfType<Planet>();
        planetTransform = new PlanetSurfaceTransform( _planet );
    }

    private void Update() {

        //if ( attackJoystick.GetValue().sqrMagnitude > .1f ) {

        //    Instantiate( projectile ).Launch( transform, attackJoystick.GetValue() );
        //}

        planetTransform.Move( transform, new Vector2( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) ), moveSpeed * Time.deltaTime );
        //planetTransform.UpdateTransform( transform );

        //transform.position += planet.GetGravity( transform.position ) * Time.deltaTime;

        //var distance = ( transform.position - planet.transform.position ).magnitude;

        //var circleLength = 2f * Mathf.PI * distance;
        //var angularMoveSpeed = moveSpeed / circleLength;
        //angularMoveSpeed = angularMoveSpeed * 360f;

        //var zAngle = Input.GetAxis( "Vertical" ) * Time.deltaTime * angularMoveSpeed;
        //var xAngle = -Input.GetAxis( "Horizontal" ) * Time.deltaTime * angularMoveSpeed;

        //transform.rotation *= Quaternion.AngleAxis( zAngle, Vector3.right ) *
        //                      Quaternion.AngleAxis( xAngle, Vector3.forward );

        //distance = distance.Clamped( planet.radius, Mathf.Infinity );
        //transform.position = transform.rotation * Vector3.up * distance;
    }

    private void OnDrawGizmos() {

        Gizmos.DrawRay( transform.position, transform.forward );
        Gizmos.DrawRay( transform.position, transform.right );
    }

}