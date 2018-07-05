using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class SceneSetup : Photon.PunBehaviour {

    #region Public Variables
    public static SceneSetup main;

    public Transform trainerTransform;
    public string photonVoiceString;

    [Header ("References")]
    public Camera vrCamera;
    public Transform localVRTransform;
    public Transform localVRHeadTransform;
    public Transform localVRHandRight, localVRHandLeft;
    public List<Material> altAvatarMaterials;
    #endregion

    #region Private Variables
    [SerializeField]
    [Header("Spawn Points")]
    private Transform[] spawnPoint;
    [SerializeField]
    [Header("OVR Camera Spawn")]
    private Transform ovrRig;
    bool enableRpC = false;
    int positionNumber;
    int newPos;

    #endregion

    #region Mono Methods
    private void Awake() {
        main = this;
    }

    private void Update() {
        if ((Input.GetKeyDown(KeyCode.R)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            RestartExperience();
        }

        //Setting RPC to check spawn positions and player count
        if (enableRpC == true)
        {
            if (positionNumber == 1)
            {
                photonView.RPC("ChangePos", PhotonTargets.AllBuffered, positionNumber);
                Debug.Log("rpcSent");
            }

        }
    }

    //PUN RPC Method for position spawning mechanics
    [PunRPC]
    void ChangePos(int pos)
    {
        newPos = pos;

    }

    #endregion

    #region Public Methods
    public void StartSettingUp()
    {
        if (vrCamera != null)
            vrCamera.enabled = true;
        if (LobbyManager.main.playerType == LobbyManager.LocalPlayerType.Trainee)
        {

        }
        else
        {
            localVRTransform.position = trainerTransform.position;
            localVRTransform.rotation = trainerTransform.rotation;
        }

        //if (!ovrRig){
            Debug.Log("Creating Network Player");
            PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, Quaternion.identity, 0).GetComponent<PlayerNetworkController>();
        //}
        if (ovrRig != null) {  
            enableRpC = true;
            //PhotonNetwork.Instantiate("Player", spawnPoint[Random.Range(0, spawnPoint.Length)].position, spawnPoint[Random.Range(0, spawnPoint.Length)].rotation, 0);
            //Debug.Log("Pos1 " + pos1 + " Pos2 " + pos2 + " Pos3 " + pos3 + " Pos4 " + pos4);
            if (positionNumber == 0)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[0].position, spawnPoint[0].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 1;
                ovrRig.position = spawnPoint[0].position;
            }

            else if (positionNumber == 1)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[1].position, spawnPoint[1].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 2;
                ovrRig.position = spawnPoint[1].position;
            }
            else if (positionNumber == 2)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[2].position, spawnPoint[2].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 3;
                ovrRig.position = spawnPoint[2].position;
            }
            else if (positionNumber == 3)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[3].position, spawnPoint[3].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 4;
                ovrRig.position = spawnPoint[3].position;
            }
            else if (positionNumber == 4)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[4].position, spawnPoint[4].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 5;
                ovrRig.position = spawnPoint[4].position;
            }
            else if (positionNumber == 5)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[5].position, spawnPoint[5].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 6;
                ovrRig.position = spawnPoint[5].position;
            }
            else if (positionNumber == 6)
            {
                //PlayerNetworkController networkPlayer = PhotonNetwork.Instantiate("NetworkedPlayer", spawnPoint[6].position, spawnPoint[6].rotation, 0).GetComponent<PlayerNetworkController>();
                positionNumber = 0;
                ovrRig.position = spawnPoint[6].position;
            }
        }
    }

    public void RestartExperience() {
        PhotonNetwork.LeaveRoom ();
        UnityEngine.SceneManagement.SceneManager.LoadScene (0);
    }
    #endregion

    

}
