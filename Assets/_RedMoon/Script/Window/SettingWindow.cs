using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class SettingWindow : MonoBehaviour {
	public Toggle musicToggle;
	public Toggle soundToggle;

    public GameObject uiMan;
    public GameObject mediaPlayCtrl;

	public void OnExitBtnClick(){
		GameManager.Instance.ProcEventMessages (EventMessages.ENTER_MAINMENU_WINDOW);
	}
	public void UpdateData(){
		musicToggle.isOn = GameSetting.musicOn;

        if (musicToggle.isOn == false)
        {
            uiMan.GetComponent<AudioSource>().mute = true;
            mediaPlayCtrl.GetComponent<MediaPlayer>().m_Muted = true;
        }
        else
        {
            uiMan.GetComponent<AudioSource>().mute = false;
            mediaPlayCtrl.GetComponent<MediaPlayer>().m_Muted = false;
        }

        soundToggle.isOn = GameSetting.soundOn;
	}
	public void OnMusicToggleClick(Toggle tog){
		GameSetting.musicOn = tog.isOn;
        if (GameSetting.musicOn == false)
        {
            uiMan.GetComponent<AudioSource>().mute = true;
            mediaPlayCtrl.GetComponent<MediaPlayer>().m_Muted = true;
        }
        else
        {
            uiMan.GetComponent<AudioSource>().mute = false;
            mediaPlayCtrl.GetComponent<MediaPlayer>().m_Muted = false;
        }
    }
	public void OnSoundToggleClick(Toggle tog){
		GameSetting.soundOn = tog.isOn;
	}
	public void OnNameToggleClick(Toggle tog){
		GameSetting.nameOn = tog.isOn;
	}
}
