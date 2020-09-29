using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;

public class AD : MonoBehaviour
{
    public static AD instance = null;

    public GameObject GameOver, CharSprite, Char, ProrsImage;
    public Image HealthScaleChar;
    RewardBasedVideoAd RBV_Ad;

    public void Start()
    {
        if (instance == null) { instance = this; }
        RBV_Ad = RewardBasedVideoAd.Instance;

        RBV_Ad.OnAdRewarded += HandleRewardBasedVideoRewarded;
        RBV_Ad.OnAdClosed += HandleRewardBasedVideoClosed;
        RBV_Ad.OnAdOpening += HandleRewardedAdOpening;

        RequestRewardBasedVideo();
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // id тестовой рекламы
#endif
        AdRequest request = new AdRequest.Builder().Build();
        RBV_Ad.LoadAd(request, adUnitId);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Time.timeScale = 1;
        RequestRewardBasedVideo();
    }
    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Time.timeScale = 0;
    }
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        GameOver.SetActive(false);
        CharSprite.SetActive(true);
        ProrsImage.SetActive(true);
        HealthScaleChar.fillAmount = 0.5f;
        Char.GetComponent<MoveChar>().enabled = true;
        Controller.NextMovePlayer = true;
        Healths.isGameOver = false;
    }
    public void UserOptToWatchAd()
    {
        if (RBV_Ad.IsLoaded())
        {
            RBV_Ad.Show();
        }
    }
}
