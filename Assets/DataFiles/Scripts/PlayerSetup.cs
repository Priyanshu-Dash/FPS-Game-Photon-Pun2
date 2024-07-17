using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using SUPERCharacter;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject[] localPlayerItems, remotePlayerItems;
    public GameObject playerCamera;
    public Animator animator;
    public SuperAnimator superAnimator;
    private Recorder recorder;
    
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            animator = GetComponent<Animator>();
            superAnimator = GetComponent<SuperAnimator>();
            recorder = GetComponent<Recorder>();
            
            // local player
            if(photonView.IsMine)
            {
                foreach(GameObject g in localPlayerItems)
                {
                    g.SetActive(true);
                }

                foreach(GameObject g in remotePlayerItems)
                {
                    g.SetActive(false);
                }

                GetComponent<SUPERCharacterAIO>().cameraPerspective = PerspectiveModes._1stPerson;
                animator.SetBool("isRemote", false);
                playerCamera.SetActive(true);
            }
            
            // remote player
            else
            {
                foreach(GameObject g in remotePlayerItems)
                {
                    g.SetActive(true);
                }

                foreach(GameObject g in localPlayerItems)
                {
                    g.SetActive(false);
                }
                
                GetComponent<SUPERCharacterAIO>().enabled = false;
                animator.SetBool("isRemote", true);
                recorder.enabled = false;
                superAnimator.enabled = false;
                playerCamera.SetActive(false);
            }
        }
    }

    public void ToggleMute()
    {
        recorder.TransmitEnabled = !recorder.TransmitEnabled;
    }

    public void ToggleMuteAll()
    {
        FindObjectOfType<Speaker>().enabled = !FindObjectOfType<Speaker>().enabled;
    }
}
