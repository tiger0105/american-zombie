using UnityEngine;

public class ManiacoAds : MonoBehaviour
{
    public static ManiacoAds m_ManiacoAdsInstance = null;

    private void Awake()
    {
        if (m_ManiacoAdsInstance == null)
        {
            m_ManiacoAdsInstance = this;

            Advertisements.Instance.SetUserConsent(false);
            Advertisements.Instance.Initialize();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void ShowRewardedVideo()
    {
        if (Advertisements.Instance.IsInterstitialAvailable())
        {
            Advertisements.Instance.ShowRewardedVideo(RewardedVideoClosed);
        }
    }

    public void ShowInterstitialAds()
    {
        if (Advertisements.Instance.IsInterstitialAvailable())
        {
            Advertisements.Instance.ShowInterstitial(InterstitialClosed);
        }
    }

    private void InterstitialClosed(string advertiser)
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
        }
    }

    private void RewardedVideoClosed(bool completed, string advertiser)
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
            if (completed == true)
            {
                GameInfo.playerCoin += GlobalReferences.RewardCash;
                GameInfo.SavePlayerCoin();
                GameInfo.playerGold += GlobalReferences.RewardGold;
                GameInfo.SavePlayerGold();
                GlobalReferences.RewardCash = 0;
                GlobalReferences.RewardGold = 0;
            }

            ScreenFader fader = FindObjectOfType<ScreenFader>();
            GameSetting.lastSceneIndex = 3;
            fader.EndScene(1);
        }
    }
}
