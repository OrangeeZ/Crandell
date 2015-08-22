using UnityEngine;
using System.Collections;
using System.Linq;

public class SphereLevelGenerator : MonoBehaviour {

    public GameObject[] prefabs;

    private void Start() {

        var planet = FindObjectOfType<Planet>();

        var localToWorldMatrix = transform.localToWorldMatrix;

        foreach ( var each in GetComponent<MeshFilter>().sharedMesh.vertices.Distinct() ) {

            var worldPos = localToWorldMatrix.MultiplyPoint3x4( each );

            Instantiate( prefabs.RandomElement(), worldPos, Quaternion.FromToRotation( Vector3.up, worldPos ) );

        }

        //for ( var i = -planet.radius; i < planet.radius; i += 1f ) {

        //    for ( var j = 0f; j < 2 * Mathf.PI * ( planet.radius - i.Abs() ); j += 10f ) {

        //        var inverseRadius = ( planet.radius - i.Abs() ) * i.Sign();

        //        var rotator = Quaternion.AngleAxis( j * Mathf.Rad2Deg / i.Abs(), Vector3.up );
        //        var position = rotator * Vector3.right * Mathf.Sin( ( planet.radius - inverseRadius ) / planet.radius * Mathf.PI * 0.5f ) * planet.radius + Vector3.up * Mathf.Sin( inverseRadius / planet.radius * Mathf.PI * 0.5f ) * planet.radius;
        //        Instantiate( prefabs.RandomElement(), position, Quaternion.FromToRotation( Vector3.up, position ) );
        //    }
        //}

    }

}