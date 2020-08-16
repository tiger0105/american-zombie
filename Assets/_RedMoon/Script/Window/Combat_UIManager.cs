using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Combat_UIManager : MonoBehaviour
{
    public static Combat_UIManager Instance;
    public GameObject guiCamera;
    public GameObject radarView;
    public GameObject player;
    private GameObject headshotArt;
    public Sprite[] shotArtImages;
    public float hm_value;
    public Sprite[] m_CashTypeImages;
    public float lastTimeScale = 1;
    public WebcamControll webCamControl;

    public void ShowRadarView()
    {
        HideAllWindows();
        radarView.SetActive(true);
        radarView.GetComponent<RadarBehavior>().Init(RadarMapAlignments.SMALL_LEFT_TOP);
    }
    public void ShowStreetView(GameObject spot)
    {
        guiCamera.SetActive(false);
        player.GetComponent<PlayerCtrl>().weaponCamera.SetActive(false);
        player.GetComponentInParent<vp_SimpleCrosshair>().enabled = false;
        player.GetComponentInParent<vp_SimpleHUD>().enabled = false;
        radarView.GetComponent<RadarBehavior>().radarMap.SetActive(false);
    }
    public void ExitStreetView()
    {
        guiCamera.SetActive(true);
        player.GetComponent<PlayerCtrl>().weaponCamera.SetActive(true);
        player.GetComponentInParent<vp_SimpleCrosshair>().enabled = true;
        player.GetComponentInParent<vp_SimpleHUD>().enabled = true;
        RadarBehavior.instance.radarMap.SetActive(true);
    }

    public void PopUpHeadshotArt(Transform parent)
    {
        headshotArt = Instantiate(Resources.Load("Headshot") as GameObject, parent);
        TextMeshPro bonus = headshotArt.transform.GetChild(0).GetComponent<TextMeshPro>();
        bonus.text = "+ " + GameInfo.headshotBonus;
        headshotArt.transform.GetComponent<SpriteRenderer>().sprite = shotArtImages[0];
        headshotArt.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = m_CashTypeImages[0];
        headshotArt.SetActive(false);
        headshotArt.SetActive(true);
    }

    public void PopUpBodyshotArt(Transform parent)
    {
        headshotArt = Instantiate(Resources.Load("Headshot") as GameObject, parent);
        headshotArt.GetComponent<AudioSource>().enabled = false;
        headshotArt.GetComponent<AudioSource>().mute = true;
        TextMeshPro bonus = headshotArt.transform.GetChild(0).GetComponent<TextMeshPro>();
        bonus.text = (GameInfo.killBonus / 5).ToString();
        headshotArt.transform.GetComponent<SpriteRenderer>().sprite = shotArtImages[1];
        headshotArt.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = m_CashTypeImages[0];
        headshotArt.SetActive(false);
        headshotArt.SetActive(true);
    }

    public void OnClickPauseButton()
    {
        if (vp_Gameplay.IsPaused = !vp_Gameplay.IsPaused)
        {
            AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
            foreach (AudioSource each in audioSources)
            {
                each.Pause();
            }
        }
        else
        {
            AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
            foreach (AudioSource each in audioSources)
            {
                each.UnPause();
            }
        }

        if (vp_Gameplay.IsPaused == true)
        {
            Time.timeScale = 0;
            if (webCamControl.rearWebcamTexture != null)
                webCamControl.rearWebcamTexture.Pause();
        }
        else
        {
            Time.timeScale = 1;
            if (webCamControl.rearWebcamTexture != null)
                webCamControl.rearWebcamTexture.Play();
        }
    }

    public void OnClickQuit()
    {
        if (webCamControl.rearWebcamTexture != null)
            webCamControl.rearWebcamTexture.Stop();

        Application.Quit();
    }
    public void OnClickRestartRoundButton()
    {
        if (webCamControl.rearWebcamTexture != null)
            webCamControl.rearWebcamTexture.Stop();

        vp_Gameplay.IsPaused = false;
        SceneManager.LoadScene(2);
    }
    public void OnClickMainMenuButton()
    {
        if (webCamControl.rearWebcamTexture != null)
            webCamControl.rearWebcamTexture.Stop();

        GameManager.Instance.LastEventMessage = EventMessages.NONE;
        vp_Gameplay.IsPaused = false;
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        Instance = this;
    }

    void HideAllWindows()
    {
        radarView.SetActive(false);
    }
}