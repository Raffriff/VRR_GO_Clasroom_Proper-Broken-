using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshAtRuntime : MonoBehaviour {

    private MeshRenderer meshRenderer;

    void Update() {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer> ();

        if (meshRenderer != null) {
            if (meshRenderer.enabled)
                meshRenderer.enabled = false;
        }
    }

}
