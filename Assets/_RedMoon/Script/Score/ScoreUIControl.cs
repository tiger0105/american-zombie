using UnityEngine;
using UnityEngine.UI;

public class ScoreUIControl : MonoBehaviour {
	public Text titleText;
	public Text misionText;
	public Text killText;
	public Text headshotText;
	public Text hpText;
	public Text ontimeText;
	public Text totalText;
	public GameObject kills_suc;
	public GameObject kills_fail;
	public GameObject head_suc;
	public GameObject head_fail;
	public GameObject hp_suc;
	public GameObject hp_fail;
	public GameObject ontime_suc;
	public GameObject ontime_fail;

    public GameObject m_TotalCashIcon;
    public GameObject m_DoubleCashIcon;

    public ScreenFader fader;

	public void OnContinueBtnClick() {
        GameSetting.lastSceneIndex = 3;
        fader.EndScene(1);
    }

	public void OnRewardBtnClick()
	{
		ManiacoAds.m_ManiacoAdsInstance.ShowRewardedVideo();
	}

	//display score
	public void DisplayScore(MissionResults mr,int total, int mission_coin, int kills_coin, int headshot_coin, int hp_coin, int ontime_coin, 
		int kills_success, int headshot_success, int hp_success, int ontime_success){

        titleText.text = string.Format ("{0}", mr.ToString());
		misionText.text = string.Format ("{0:N0}", mission_coin);
		killText.text = string.Format ("{0:N0}", kills_coin);
		headshotText.text = string.Format ("{0:N0}", headshot_coin);
		hpText.text = string.Format ("{0:N0}", hp_coin);
		ontimeText.text = string.Format ("{0:N0}", ontime_coin);
        if (GameManager.Instance.b_DoubleCashActivated == true)
        {
            total *= 2;
            m_TotalCashIcon.SetActive(false);
            m_DoubleCashIcon.SetActive(true);
            GameManager.Instance.b_DoubleCashActivated = false;
			int doubleCash = PlayerPrefs.GetInt("DoubleCash", 0);
			if (doubleCash > 0)
				PlayerPrefs.SetInt("DoubleCash", doubleCash - 1);
		}
		GlobalReferences.RewardCash = total;
		GlobalReferences.RewardGold = headshot_coin;
		if (mr.Equals(MissionResults.MISSION_SUCCESS))
		{
			totalText.text = string.Format("{0:N0}, 1", total);
		}
		else
		{
			totalText.text = string.Format("{0:N0}, 0", total);
		}
		if (kills_success.Equals (0)) {
			kills_suc.SetActive (false);
			kills_fail.SetActive (true);
		} else {
			kills_suc.SetActive (true);
			kills_fail.SetActive (false);
		}
		if (headshot_success.Equals (0)) {
			head_suc.SetActive (false);
			head_fail.SetActive (true);
		} else {
			head_suc.SetActive (true);
			head_fail.SetActive (false);
		}
		if (ontime_success.Equals (0)) {
			ontime_suc.SetActive (false);
			ontime_fail.SetActive (true);
		} else {
			ontime_suc.SetActive (true);
			ontime_fail.SetActive (false);
		}
		if (hp_success.Equals (0)) {
			hp_suc.SetActive (false);
			hp_fail.SetActive (true);
		} else {
			hp_suc.SetActive (true);
			hp_fail.SetActive (false);
		}
	}
}
