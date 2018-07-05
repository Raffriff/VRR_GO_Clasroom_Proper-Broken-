using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironment : MonoBehaviour {

    public float loadTime = 2f;
    public GameObject[] toEnable;
    public GameObject[] toDisable;
    public PlayMakerFSM fsm;
    public string stateName;

    private float loadAmount = 0f;
    private Renderer localRenderer;

    private void Awake() {
        localRenderer = GetComponent<Renderer> ();

        Color matColour = localRenderer.material.color;
        matColour.a = 0f;
        matColour.r = 0f;
        localRenderer.material.color = matColour;
    }

    private void Update() {
        loadAmount -= Time.deltaTime / loadTime;
        loadAmount = Mathf.Clamp01 (loadAmount);

        Color matColour = localRenderer.material.color;
        matColour.a = loadAmount * 1f;
        matColour.r = loadAmount * 1f;
        localRenderer.material.color = matColour;
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "HandCollider") {
            loadAmount += Time.deltaTime / (loadTime * 0.5f);
            if (loadAmount >= 1f) {
                loadAmount = 0f;
                SendToFSM ();
            }
            Color matColour = localRenderer.material.color;
            matColour.a = loadAmount * 1f;
            matColour.r = loadAmount * 1f;
            localRenderer.material.color = matColour;
        }
    }

    private void SendToFSM() {
        fsm.SetState (stateName);
    }

    public void Activate() {
        foreach (GameObject enabling in toEnable) {
            enabling.SetActive (true);
        }
        foreach (GameObject disabling in toDisable) {
            disabling.SetActive (false);
        }
    }

}
