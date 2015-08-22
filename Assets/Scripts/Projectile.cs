using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float speed = 10f;

    public float lifetime = 3f;

    private AutoTimer timer;

    private Vector3 _direction;

    private PlanetSurfaceTransform _planetTransform;
    private Character _owner;

    private void Awake() {

        var planet = FindObjectOfType<Planet>();

        _planetTransform = new PlanetSurfaceTransform( planet );

        timer = new AutoTimer( lifetime );

        enabled = false;
    }

    private void Update() {

        if ( timer.ValueNormalized >= 1f ) {

            Destroy( gameObject );
        }

        _planetTransform.Move( transform, _direction, speed * Time.deltaTime );
    }

    public void Launch( Character owner, Vector3 direction ) {

        _owner = owner;

        _planetTransform.SetHeight( owner.pawn.planetTransform.height );

        transform.position = _owner.pawn.position;
        transform.rotation = _owner.pawn.rotation;

        _direction = direction;

        enabled = true;
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.GetComponent<CharacterPlanetPawn>();

        if ( otherPawn != null && otherPawn != _owner.pawn && otherPawn.character.teamId != _owner.teamId ) {

            otherPawn.character.health.Value -= 1;

            Destroy( gameObject );
        }
    }

}