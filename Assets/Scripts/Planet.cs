using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public float gravityScalar = 5f;

    public float radius {
        get { return transform.localScale.x / 2; }
    }

    public Vector3 GetGravity( Vector3 point ) {

        return ( transform.position - point ).normalized * gravityScalar;
    }

}