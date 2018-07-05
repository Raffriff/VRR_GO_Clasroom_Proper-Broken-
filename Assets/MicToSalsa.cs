using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyMinnow.SALSA;
using Photon;

[RequireComponent (typeof (Salsa3D))]
[RequireComponent (typeof (AudioSource))]
public class MicToSalsa : UnityEngine.MonoBehaviour {

    private Salsa3D salsa;
    public PhotonVoiceSpeaker voice;
    public string audioInputDevice;
    private bool isLinked = false;

    private void Awake()
    {
        salsa = GetComponent<Salsa3D> ();
    }

    private void Start()
    {
        if (voice == null)
        {
            if (audioInputDevice != "")
            {
                salsa.audioClip = Microphone.Start (audioInputDevice, true, 10, 44100);
                salsa.audioSrc.clip = salsa.audioClip;
                salsa.audioSrc.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
                while (!(Microphone.GetPosition(audioInputDevice) <= 0)) { }
                salsa.audioSrc.Play ();
                Debug.Log ("Audio Input: " + audioInputDevice);
            } else if (Microphone.devices.Length > 0)
            {
                salsa.audioClip = Microphone.Start (Microphone.devices[0], true, 10, 44100);
                salsa.audioSrc.clip = salsa.audioClip;
                salsa.audioSrc.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
                while (!(Microphone.GetPosition (Microphone.devices[0]) <= 0)) { }
                salsa.audioSrc.Play ();
                Debug.Log ("Audio Input: " + Microphone.devices[0]);
            }
            
        }
    }
    private void Update()
    {
        if ((voice != null) && (!isLinked))
        {
            if (voice.isLinked)
            {
                isLinked = true;
                Debug.LogError ("!!!!!!!!!!\n");
                Debug.LogError (voice.player.source.clip.name);
                salsa.audioSrc.clip = voice.player.source.clip;
                salsa.audioSrc.Play ();
            }
        }

        if (salsa.audioSrc != null)
        {
            
        }
    }
}
