using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverSceneManager : MonoBehaviour
{
    public TMP_Text survivalTime, killCount;

    void Start()
    {
        float st = PlayerPrefs.GetFloat("SurvivalTime", 0f) / 60f;
        Debug.Log("Survival Time: " + st);
        Debug.Log("Kill Count: " + PlayerPrefs.GetInt("KillCount", 0));
        survivalTime.text = "Survival Time: " + st.ToString("F2") + " min";
        killCount.text = "Kills: " + PlayerPrefs.GetInt("KillCount", 0).ToString();
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
