using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{

    public void LoadMainScene()
    {
        GameSetting.lastSceneIndex = 0;
        SceneManager.LoadScene(1);
    }

    public void Start()
    {
        //LoadMainScene();
    }
}