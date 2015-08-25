using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public PlanetSurfaceTransform planetTransform;

    [SerializeField]
    private int _damage = 1000;

    [SerializeField]
    private float _radius = 20f;

    [SerializeField]
    private float _startingHeight = 5;

    [SerializeField]
    private float _weight = 10f;

    private float _speed;

    private void Start() {

        planetTransform = new PlanetSurfaceTransform( Planet.instance );

        planetTransform.SetHeight( _startingHeight );

        planetTransform.SetPosition( transform );
    }

    private void Update() {

        _speed += Time.deltaTime * _weight;

        planetTransform.SetHeight( planetTransform.height - _speed );

        planetTransform.UpdatePosition( transform );

        if ( planetTransform.height <= 0.1f ) {

            enabled = false;

            Helpers.DoSplashDamage( transform.position, _radius, _damage );
        }
    }

}