using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiScene : MonoBehaviour {

    private void Awake() {
        if (enabled) {
            DontDestroyOnLoad (gameObject);
        }
    }

    private void Start() {
        
    }

}
