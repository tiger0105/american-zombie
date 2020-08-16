//using Assets.SimpleAndroidNotifications;
using Assets.SimpleAndroidNotifications;
using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine;
public enum IntroWindow{
	TITLE_SHOW,
	SOAF_FIND,
	MIRROR_FIND,
	STAR_DRAW,
	REDRUM_DRAW,
	PHONE_HOLD,
	GHOST_INTRO
}	

public class UIManager : MonoBehaviour {
	static public UIManager Instance;
	public GameObject guiCamera;
	public GameObject gyroCamera;
	public GameObject streetCamera;
	public GameObject overGuiCamera;
	public GameObject canvasUI;

	public GameObject mainMenuWindow;
	public GameObject settingWindow;
	public GameObject itemBuyWindow;
	public GameObject itemUpgradeWindow;
	public GameObject missionWindow;
	public GameObject informDlg;
	public GameObject coinBuyWindow;
    public GameObject mediaPlayerCtrl;

	public float appearDuration = 1f;

    // Add By HKC
    public GameObject dailyGiftWindow;
    public GameObject loginWindow;
    public GameObject CameraWindow;

	AudioSource audioSrc;

	/////////////////////////////////************user interface ************/////////////////////////

	public void HideAllWindows(){
		mainMenuWindow.SetActive (false);
		itemBuyWindow.SetActive (false);
		itemUpgradeWindow.SetActive (false);
		informDlg.SetActive (false);
		settingWindow.SetActive (false);
		coinBuyWindow.SetActive (false);
		missionWindow.SetActive (false);
	}
	public void HideCanvas(){
		canvasUI.SetActive (false);
	}
	public void ShowMainMenuWindow(){
		if (audioSrc == null)
			audioSrc = GetComponent<AudioSource> ();
		
		overGuiCamera.SetActive (false);
		HideAllWindows ();
		audioSrc.Pause ();
		mainMenuWindow.SetActive (true);
	}
	public void ShowSettingWindow(){
		settingWindow.SetActive (true);
		settingWindow.GetComponent<SettingWindow> ().UpdateData ();
	}
	public void ShowItemStateWindow(){
		HideAllWindows ();
		//itemStateWindow.SetActive (true);
	}
	public void ShowItemBuyWindow(){
		HideAllWindows ();

		if(!audioSrc.isPlaying)
			audioSrc.Play ();
		
		ItemModelBase.Instance.DestroyCurModel ();
		itemBuyWindow.SetActive (true);
		itemBuyWindow.GetComponent<ItemBuyWindow> ().ShowElement ("w0");
		overGuiCamera.SetActive (true);
	}
	public void ShowItemUpgradeWindow(){
		HideAllWindows ();

		if (audioSrc == null)
			audioSrc = GetComponent<AudioSource> ();

		if(!audioSrc.isPlaying)
			audioSrc.Play ();
		
		ItemModelBase.Instance.DestroyCurModel ();
		overGuiCamera.SetActive (true);
		itemUpgradeWindow.SetActive(true);
		ItemUpgradeWindow.Instance.LoadInventory ();
	}
	public void ShowCoinBuyWindow(){
		//HideAllWindows ();
		//ItemModelBase.Instance.DestroyCurModel ();
		informDlg.SetActive(false);
		coinBuyWindow.SetActive (true);
		coinBuyWindow.GetComponent<CoinBuyWindow> ().Init ();
	}
	public void ShowMisionWindow(){
		HideAllWindows ();

		if(!audioSrc.isPlaying)
			audioSrc.Play ();
		
		missionWindow.SetActive (true);
	}
	public void Inform(InformTypes type, string title, string content, string image){
		overGuiCamera.SetActive (false);
		informDlg.SetActive (true);
		informDlg.GetComponent<InformDlg> ().ModalDlg(type, title, content, image);
	}

	public void OnInformAccept(){
		InformDlg dlg = informDlg.GetComponent<InformDlg> ();
		switch(dlg.Type){
		case InformTypes.NOT_ENOUGH_COIN:			
			GameManager.Instance.ProcEventMessages (EventMessages.ENTER_COINBUY_WINDOW);
			break;
		default:
			informDlg.SetActive (false);
			overGuiCamera.SetActive (true);
			break;
		}
	}
	public void OnInformCancel(){
		InformDlg dlg = informDlg.GetComponent<InformDlg> ();
		switch(dlg.Type){
		case InformTypes.NOT_ENOUGH_COIN:
		default:
			informDlg.SetActive (false);
			overGuiCamera.SetActive (true);
			break;
		}
	}
	void Start() {		
		Instance = this;

        if (GameManager.Instance == null)
            return;

        switch (GameManager.Instance.LastEventMessage)
        {
            case EventMessages.NONE:
                GameManager.Instance.ProcEventMessages(EventMessages.ENTER_MAINMENU_WINDOW);
                break;
            default:
                GameManager.Instance.ProcEventMessages(EventMessages.ENTER_ITEMUPGRADE_WINDOW);
                break;

        }

        if (DateTime.Now.Day % 3 == 2)
        {
            string dataAsJson = FileTool.ReadFile("Data", false);
            GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
            if (GameManager.Instance.persistantData.pushNotify == 0)
            {
                GameManager.Instance.persistantData.pushNotify = 1;
                NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(5), "Notification", "Zombies are coming from the East. Join the fight today.  Play AMERICAN ZOMBIE", new Color(0, 0.6f, 1), NotificationIcon.Message);
                string data = JsonUtility.ToJson(GameManager.Instance.persistantData);
                FileTool.createORwriteFile("Data", data);
            }
        }
        else
        {
            string dataAsJson = FileTool.ReadFile("Data", false);
            GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
            if (GameManager.Instance.persistantData.pushNotify != 0)
            {
                GameManager.Instance.persistantData.pushNotify = 0;
                NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(5), "Notification", "Zombies are coming from the East. Join the fight today.  Play AMERICAN ZOMBIE", new Color(0, 0.6f, 1), NotificationIcon.Message);
                string data = JsonUtility.ToJson(GameManager.Instance.persistantData);
                FileTool.createORwriteFile("Data", data);
            }
        }

        if (GameSetting.musicOn == false)
        {
            GetComponent<AudioSource>().mute = true;
            mediaPlayerCtrl.GetComponent<MediaPlayer>().m_Muted = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
            mediaPlayerCtrl.GetComponent<MediaPlayer>().m_Muted = false;
        }

        // Add BY HKC
        CameraWindow.SetActive(false);
        int firstPlay = PlayerPrefs.GetInt("FirstPlay", 0);

        if (firstPlay == 0)
            loginWindow.SetActive(true);
        else
            loginWindow.SetActive(false);
    }

    public void OnClickStart()
    {
        mainMenuWindow.SetActive(true);
        loginWindow.SetActive(false);
        CameraWindow.SetActive(true);
    }

    public void OnCloseCamera()
    {
        CameraWindow.SetActive(false);
        mainMenuWindow.SetActive(true);
        PlayerPrefs.SetInt("FirstPlay", 1);
    }
}