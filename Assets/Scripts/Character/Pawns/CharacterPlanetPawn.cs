using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterPlanetPawn : CharacterPawn {

    public PlanetSurfaceTransform planetTransform;

    public bool canFollowDestination;

    public GameObject turret;

    [SerializeField]
    private float _weight = 1f;

    [SerializeField]
    private float _startingHeight = 5;

    [SerializeField]
    private float _rotationToDirectionSpeed = 100;

    [SerializeField]
    private SimpleSphereCollider _sphereCollider;

    private Vector3? _destination;
    private bool _isGravityEnabled;
    private float _ySpeed;

    private Transform _turretTarget;

    [SerializeField]
    private Transform _subTransform;

    protected override void Start() {

        base.Start();

        planetTransform = new PlanetSurfaceTransform( Planet.instance );

        planetTransform.SetHeight( _startingHeight );

        planetTransform.SetPosition( transform );
    }

    private void Update() {

        if ( _isGravityEnabled ) {

            _ySpeed += _weight * Time.deltaTime;

            planetTransform.SetHeight( planetTransform.height - _ySpeed * Time.deltaTime );
            planetTransform.UpdatePosition( transform );
        }

        if ( _destination.HasValue && canFollowDestination ) {

            var direction = planetTransform.GetDirectionTo( _destination.Value );

            planetTransform.Move( transform, Vector3.forward, speed * Time.deltaTime );

            rotation = Quaternion.RotateTowards( transform.rotation, transform.rotation * Quaternion.FromToRotation( Vector3.forward, direction.Set( y: 0 ) ), _rotationToDirectionSpeed * Time.deltaTime );

            ApplyPunishingForce();
        }

        if ( turret != null && _turretTarget != null ) {

            if ( character.pawn.turret != null ) {

                var direction = planetTransform.GetDirectionTo( _turretTarget.position );

                character.pawn.turret.transform.localRotation = Quaternion.FromToRotation( Vector3.forward, direction );
            }
        }

    }

    public override void MoveDirection( Vector3 direction ) {

        planetTransform.Move( transform, direction, speed * Time.deltaTime );

        if ( _subTransform ) {

            _subTransform.localRotation = Quaternion.RotateTowards( _subTransform.localRotation, Quaternion.LookRotation( direction.Set( y: 0 ), Vector3.up ), _rotationToDirectionSpeed * Time.deltaTime );
        }

        ApplyPunishingForce();
    }

    public override void SetDestination( Vector3 destination ) {

        _destination = destination;
    }

    public override float GetDistanceToDestination() {

        return _destination.HasValue ? planetTransform.GetDistanceTo( _destination.Value ) : float.NaN;
    }

    public override Vector3 GetDirectionTo( CharacterPawn otherPawn ) {

        return planetTransform.GetDirectionTo( otherPawn.position );
    }

    public void ClearDestination() {

        _destination = null;
    }

    public void SetTurretTarget( Transform turretTarget ) {

        _turretTarget = turretTarget;
    }

    public void SetColor( Color baseColor ) {

        var renderers = GetComponentsInChildren<Renderer>();
        foreach ( var each in renderers ) {

            each.material.SetColor( "_Color", baseColor );
        }
    }

    public void SetGravityEnabled( bool value ) {

        _isGravityEnabled = value;

        if ( !value ) {

            _ySpeed = 0;
        }
    }

    public void SetActive( bool isActive ) {

        var collider = GetComponent<Collider>();

        collider.enabled = isActive;
        enabled = isActive;
    }

    private void ApplyPunishingForce() {

        if ( _sphereCollider == null ) {

            return;
        }

        var punishingForce = Building.instances
            .Select( _ => _.sphereCollider )
            .Select( _ => _.CalculatePunishingForce( _sphereCollider ) )
            .Aggregate( Vector3.zero, ( total, each ) => each + total );

        punishingForce = Quaternion.Inverse( rotation ) * punishingForce;
        punishingForce.y = 0;

        planetTransform.Move( transform, punishingForce, 1f );
    }

}