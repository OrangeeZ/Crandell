﻿using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float lifetime = 3f;

    private AutoTimer timer;

    private PlanetSurfaceTransform _planetTransform;

    protected Vector3 direction;

    protected Character owner;

    protected float speed;

    private void Awake() {

        enabled = false;
    }

    private void Update() {

        if ( timer.ValueNormalized >= 1f ) {

            OnLifetimeExpire();
        }

        _planetTransform.Move( transform, direction, speed * Time.deltaTime );
    }

    public void Launch( Character owner, Vector3 direction, float speed ) {

        this.owner = owner;
        this.speed = speed;
        this.direction = direction;

        _planetTransform = new PlanetSurfaceTransform( Planet.instance );
        _planetTransform.SetHeight( owner.pawn.planetTransform.height );

        transform.position = this.owner.pawn.position;
        transform.rotation = this.owner.pawn.rotation;

        timer = new AutoTimer( lifetime );

        enabled = true;
    }

    public virtual void OnHit() {

        Destroy( gameObject );
    }

    public virtual void OnLifetimeExpire() {

        Destroy( gameObject );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.GetComponent<CharacterPlanetPawn>();

        if ( otherPawn != null && otherPawn != owner.pawn && otherPawn.character.teamId != owner.teamId ) {

            otherPawn.character.health.Value -= 1;

            OnHit();
        }
    }

}