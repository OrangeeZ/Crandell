using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public static Planet instance { get; private set; }
    public float gravityScalar = 5f;

    public float radius {
        get { return transform.localScale.x / 2; }
    }

    void Awake() {

        instance = this;
    }

    public Vector3 GetGravity( Vector3 point ) {

        return ( transform.position - point ).normalized * gravityScalar;
    }

}