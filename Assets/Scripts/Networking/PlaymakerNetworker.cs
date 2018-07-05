using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaymakerNetworker : MonoBehaviour
{

    #region Public Variables
    public PhotonView photonView;

    //[HideInInspector]
    public bool stateTrigger = false;
    public string currentStateString = "";
    #endregion

    #region Mono Methods
    private void Update() {
        if ((!photonView.isMine) && (stateTrigger)) {
            stateTrigger = false;
            Debug.Log ("State Changed!");
        }
    }
    #endregion

    #region Playmaker Methods
    public void ChangeToState(string newStateString) {
        Debug.Log ("About To Change State!");
        photonView.TransferOwnership (PhotonNetwork.player.ID);
        stateTrigger = true;
        currentStateString = newStateString;
    }
    #endregion

    #region Network Methods
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext (stateTrigger);
            stream.SendNext (currentStateString);
        } else {
            stateTrigger = (bool)stream.ReceiveNext ();
            currentStateString = (string)stream.ReceiveNext ();
        }
    }
    #endregion

}