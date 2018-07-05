using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : Photon.PunBehaviour
{

    #region Public Variables
    //Static
    public static LobbyManager main;

    [Header ("Connection")]
    public string roomName = "Exhibition";
    public int dataRate = 30;

    [Header ("Stats")]
    public LocalPlayerType playerType;

    [Header ("UI References")]
    public Text debugText;
    public Toggle isCompanion;
    public Button startButton;
    #endregion

    #region Hidden Variables
    //[HideInInspector]
    //public PhotonPlayer otherPlayer;
    #endregion

    #region Private Variables
    private bool connectedToMaster = false;
    private bool joiningRoom = false;
    #endregion

    #region Enums
    public enum LocalPlayerType { Trainee, Companion }
    #endregion

    #region Mono Methods
    private void Awake() {
        if (enabled) {
            main = this;
        }
        PhotonNetwork.sendRate = dataRate;
        PhotonNetwork.sendRateOnSerialize = dataRate;
    }

    private void Start() {
        PhotonNetwork.ConnectUsingSettings ("0.01");
        isCompanion.isOn = false;
        startButton.interactable = true;
        isCompanion.gameObject.SetActive (false);
        startButton.gameObject.SetActive (false);
        debugText.text = "Connecting To Master...";
        Debug.Log ("Connecting To Master...");
    }
    #endregion

    #region Network Methods
    public override void OnConnectedToPhoton() {
        base.OnConnectedToPhoton ();
        DebugTextAdd ("\nConnected To Photon");
        Debug.Log ("Connected To Photon");
    }

    public override void OnDisconnectedFromPhoton() {
        connectedToMaster = false;
        startButton.gameObject.SetActive (false);
        debugText.gameObject.SetActive (true);
        DebugTextAdd ("\n<color=red>Lost connection to photon...</color>"); ;
        Debug.LogWarning ("Lost connection to photon...");
        PhotonNetwork.LeaveRoom ();
        PhotonNetwork.ConnectUsingSettings ("0.01");
    }

    public override void OnConnectionFail(DisconnectCause cause) {
        base.OnConnectionFail (cause);
        debugText.gameObject.SetActive (true);
        DebugTextAdd ("\n<color=red>Lost connection to photon...</color>"); ;
        DebugTextAdd ("\n<color=red>Reason: " + cause.ToString() + "</color>"); ;
        Debug.LogWarning ("Lost connection to photon...");
        Debug.LogWarning ("Reason: " + cause.ToString ());
        connectedToMaster = false;
        startButton.gameObject.SetActive (false);
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
        base.OnFailedToConnectToPhoton (cause);
        debugText.gameObject.SetActive (true);
        DebugTextAdd ("\n<color=red>Failed connection...</color>"); ;
        DebugTextAdd ("\n<color=red>Reason: " + cause.ToString () + "</color>"); ;
        Debug.LogWarning ("Failed connection...");
        Debug.LogWarning ("Reason: " + cause.ToString ());
        connectedToMaster = false;
        startButton.gameObject.SetActive (false);
    }

    public override void OnConnectedToMaster() {
        connectedToMaster = true;
        startButton.gameObject.SetActive (true);
        DebugTextAdd ("\nConnected To Master");
        Debug.Log ("Connected To Master");
        isCompanion.gameObject.SetActive (true);
        startButton.gameObject.SetActive (true);
        Login();
    }

    public override void OnJoinedRoom() {
        joiningRoom = false;
        connectedToMaster = false;
        
        debugText.gameObject.SetActive (false);
        isCompanion.gameObject.SetActive (false);
        startButton.gameObject.SetActive (false);

        if (isCompanion.isOn)
            playerType = LocalPlayerType.Companion;
        else
            playerType = LocalPlayerType.Trainee;

        SceneSetup.main.StartSettingUp ();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        base.OnPhotonJoinRoomFailed (codeAndMsg);
        debugText.gameObject.SetActive (true);
        joiningRoom = false;
        DebugTextAdd ("\n<color=red>Failed to join room</color>");
        Debug.LogWarning ("Failed to join room");
        foreach (object code in codeAndMsg) {
            DebugTextAdd ("\n<color=yellow>" + code.ToString() + "</color>");
            Debug.LogWarning (code.ToString ());
        }
    }

    public override void OnLeftRoom() {
        debugText.gameObject.SetActive (true);
        base.OnLeftRoom ();
        DebugTextAdd ("\nLeft room");
        Debug.Log ("Left room");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        base.OnPhotonPlayerConnected (newPlayer);
        DebugTextAdd ("\nOther user logged in");
        Debug.Log ("Other user logged in");
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        base.OnPhotonPlayerDisconnected (otherPlayer);
        DebugTextAdd ("\nOther user disconnected");
        Debug.LogWarning ("Other user disconnected");
    }
    #endregion

    private void DebugTextAdd(string input) {
        debugText.text += input;
        if (debugText.text.Length > 500) {
            debugText.text.Substring (debugText.text.Length - 500, 500);
        }
    }

    public void Login() {
        if (!joiningRoom) {
            PhotonNetwork.JoinOrCreateRoom (roomName, new RoomOptions (), new TypedLobby ());
            joiningRoom = true;
        }
    }

}