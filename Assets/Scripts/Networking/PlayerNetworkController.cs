using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkController : MonoBehaviour
{

    #region Public Variables
    public Transform avatarHead, avatarHandRight, avatarHandLeft;

    public Vector3 handRightRotationOffset, handLeftRotationOffset;

    public Renderer avatarRenderer;

    [HideInInspector]
    public Transform playerGlobal, playerHead, playerHandRight, playerHandLeft;

    public PhotonView photonView;

    public GameObject[] localHidden;

    [Range (0f, 1f)]
    public float smoothingAmount = 0.75f;
    #endregion

    #region Private Variables
    private Vector3 vrPosition, headPosition, handRightPosition, handLeftPosition;
    private Quaternion vrRotation, headRotation, handRightRotation, handLeftRotation;
    private bool materialUpdated = false;
    #endregion

    #region Mono Methods
    void Start() {
        if (photonView.isMine) {
            playerGlobal = SceneSetup.main.localVRTransform;
            playerHead = SceneSetup.main.localVRHeadTransform;
            playerHandRight = SceneSetup.main.localVRHandRight;
            playerHandLeft = SceneSetup.main.localVRHandLeft;

            transform.SetParent (playerHead);
            transform.localPosition = Vector3.zero;

            foreach (GameObject toHide in localHidden) {
                toHide.SetActive (false);
            }

        } else {
            if (SceneSetup.main.altAvatarMaterials != null) {
                if (SceneSetup.main.altAvatarMaterials.Count > 0) {
                    avatarRenderer.material = SceneSetup.main.altAvatarMaterials[0];
                    SceneSetup.main.altAvatarMaterials.RemoveAt (0);
                }
            }
        }
    }

    private void Update() {
        SmoothMovement ();
    }
    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext (playerGlobal.position);
            stream.SendNext (playerGlobal.rotation);
            stream.SendNext (playerHead.position);
            stream.SendNext (playerHead.rotation);
            stream.SendNext (playerHandRight.position);
            stream.SendNext (playerHandRight.rotation);
            stream.SendNext (playerHandLeft.position);
            stream.SendNext (playerHandLeft.rotation);
        } else {
            vrPosition = (Vector3)stream.ReceiveNext ();
            vrRotation = (Quaternion)stream.ReceiveNext ();
            headPosition = (Vector3)stream.ReceiveNext ();
            headRotation = (Quaternion)stream.ReceiveNext ();
            handRightPosition = (Vector3)stream.ReceiveNext ();
            handRightRotation = (Quaternion)stream.ReceiveNext () * Quaternion.Euler (handRightRotationOffset);
            handLeftPosition = (Vector3)stream.ReceiveNext ();
            handLeftRotation = (Quaternion)stream.ReceiveNext () * Quaternion.Euler (handLeftRotationOffset);
        }
    }

    private void SmoothMovement() {
        if (!photonView.isMine) {
            float smoothnessAmount = (1f - smoothingAmount) * (Time.deltaTime / (1f / (float)PhotonNetwork.sendRate));
            transform.position = Vector3.Lerp (transform.position, vrPosition, smoothnessAmount);
            transform.rotation = Quaternion.Lerp (transform.rotation, vrRotation, smoothnessAmount);
            avatarHead.position = Vector3.Lerp (avatarHead.position, headPosition, smoothnessAmount);
            avatarHead.rotation = Quaternion.Lerp (avatarHead.rotation, headRotation, smoothnessAmount);
            avatarHandRight.position = Vector3.Lerp (avatarHandRight.position, handRightPosition, smoothnessAmount);
            avatarHandRight.rotation = Quaternion.Lerp (avatarHandRight.rotation, handRightRotation, smoothnessAmount);
            avatarHandLeft.position = Vector3.Lerp (avatarHandLeft.position, handLeftPosition, smoothnessAmount);
            avatarHandLeft.rotation = Quaternion.Lerp (avatarHandLeft.rotation, handLeftRotation, smoothnessAmount);
        }
    }
}
