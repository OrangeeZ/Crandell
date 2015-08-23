using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public float speed = 3f;

    private void Update() {

        transform.localRotation *= Quaternion.AngleAxis( speed * Time.deltaTime, Vector3.forward );
    }

}