using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float speed = 10f;

    public float lifetime = 3f;

    private Planet planet;

    private AutoTimer timer;

    private Vector3 direction;

    private void Awake() {

        planet = FindObjectOfType<Planet>();

        timer = new AutoTimer( lifetime );

        enabled = false;
    }

    private void Update() {

        if ( timer.ValueNormalized >= 1f ) {

            Destroy( gameObject );
        }

        var distance = ( transform.position - planet.transform.position ).magnitude;

        var circleLength = 2f * Mathf.PI * distance;
        var angularMoveSpeed = speed / circleLength;
        angularMoveSpeed = angularMoveSpeed * 360f;

        var zAngle = direction.z * Time.deltaTime * angularMoveSpeed;
        var xAngle = direction.x * Time.deltaTime * angularMoveSpeed;

        //transform.position += direction * speed * Time.deltaTime;

        transform.rotation *= Quaternion.AngleAxis( zAngle, Vector3.right ) *
                              Quaternion.AngleAxis( -xAngle, Vector3.forward );

        distance = distance.Clamped( planet.radius, Mathf.Infinity );
        transform.position = transform.rotation * Vector3.up * distance;
    }

    public void Launch( Transform sourceTransform, Vector3 direction ) {

        transform.position = sourceTransform.position;
        transform.rotation = sourceTransform.rotation;

        this.direction = direction.normalized;

        enabled = true;
    }

    private void OnTriggerEnter( Collider other ) {

        var otherCharacter = other.GetComponent<Crandell>();

        //if ( otherCharacter != null ) {

        //    Destroy( gameObject );
        //}
    }

}