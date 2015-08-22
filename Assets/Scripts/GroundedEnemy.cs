using UnityEngine;
using System.Collections;

public class GroundedEnemy : MonoBehaviour {

    public float moveSpeed = 5f;

    public Projectile projectile;

    public float shootInterval = 1f;

    private Crandell target;

    private Planet planet;

    private AutoTimer shootTimer;

    private PlanetSurfaceTransform _planetTransform;

    private void Start() {

        target = FindObjectOfType<Crandell>();
        planet = FindObjectOfType<Planet>();

        _planetTransform = new PlanetSurfaceTransform( planet );

        shootTimer = new AutoTimer( shootInterval );
    }

    private void Update() {

        //if ( attackJoystick.GetValue().sqrMagnitude > .1f ) {

        //    Instantiate( projectile ).Launch( transform, attackJoystick.GetValue() );
        //}

        if ( shootTimer.ValueNormalized == 1f ) {

            shootTimer.Reset();

            //Instantiate( projectile ).Launch( transform, planetTransform.GetDirectionTo( target.planetTransform ) );
        }

        _planetTransform.MoveTowards( transform, target.planetTransform, moveSpeed * Time.deltaTime );
        //planetTransform.UpdateTransform( transform );

        //transform.position += planet.GetGravity(transform.position) * Time.deltaTime;

        //var distance = (transform.position - planet.transform.position).magnitude;

        //var circleLength = 2f * Mathf.PI * distance;
        //var angularMoveSpeed = moveSpeed / circleLength;
        //angularMoveSpeed = angularMoveSpeed * 360f;

        //var zAngle = directionToTarget.z * Time.deltaTime * angularMoveSpeed;
        //var xAngle = -directionToTarget.x * Time.deltaTime * angularMoveSpeed;

        //transform.rotation *= Quaternion.AngleAxis( zAngle, Vector3.right ) *
        //                      Quaternion.AngleAxis( xAngle, Vector3.forward );

        //distance = distance.Clamped( planet.radius, Mathf.Infinity );
        //transform.position = transform.rotation * Vector3.up * distance;
    }

    private Vector3 GetDirectionToTarget() {

        var relativePosition = transform.InverseTransformPoint( target.transform.position );

        return relativePosition; //new Vector3( Mathf.Atan2( delta.y, delta.x ), 0, Mathf.Atan2( delta.z, delta.x ) );
    }

    private void OnDrawGizmos() {

        Gizmos.DrawRay( transform.position, transform.forward );
        Gizmos.DrawRay( transform.position, transform.right );
    }

}