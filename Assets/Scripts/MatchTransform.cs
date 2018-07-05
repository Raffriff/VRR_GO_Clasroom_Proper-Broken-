using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTransform : MonoBehaviour {

    public Transform target;
    public Vector3 positionOffset, rotationOffset;

    public void Update() {
        transform.position = target.TransformPoint(positionOffset);
        transform.rotation = target.rotation * Quaternion.Euler (rotationOffset);
    }

}
