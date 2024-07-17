using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public TMP_Text playerName;
    private float survivalTime = 0f;

    #region UnityMethods
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
            playerName.text = PhotonNetwork.LocalPlayer.NickName;
        }

        StartCoroutine(UpdateSurvivalTime());
    }

    void Update()
    {
        if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            survivalTime += Time.deltaTime;
        }
    }

    #endregion

    #region UtilityMethods
    private IEnumerator UpdateSurvivalTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            ExitGames.Client.Photon.Hashtable survivalTimeProp = new() { { "SurvivalTime", survivalTime } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(survivalTimeProp);
            PlayerPrefs.SetFloat("SurvivalTime", survivalTime);
        }
    }
    #endregion

    #region PunCallbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Gameover");
    }
    #endregion
}
