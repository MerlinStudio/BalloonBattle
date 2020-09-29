using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;

public class AD : MonoBehaviour
{
    public GameObject GameOver, CharSprite, Char, ProrsImage;
    public Image HealthScaleChar;
    private RewardBasedVideoAd RBV_Ad;

    public void Start()
    {
        RBV_Ad = RewardBasedVideoAd.Instance;

        RBV_Ad.OnAdRewarded += HandleRewardBasedVideoRewarded;
        RBV_Ad.OnAdClosed += HandleRewardBasedVideoClosed;

        RequestRewardBasedVideo();
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-2436211558787223/8383860702";
#endif
        AdRequest request = new AdRequest.Builder().Build();
        RBV_Ad.LoadAd(request, adUnitId);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        GameOver.SetActive(false);
        CharSprite.SetActive(true);
        ProrsImage.SetActive(true);
        HealthScaleChar.fillAmount = 0.5f;
        Char.GetComponent<MoveChar>().enabled = true;
        Controller.NextMovePlayer = true;
    }
    public void UserOptToWatchAd()
    {
        if (RBV_Ad.IsLoaded())
        {
            RBV_Ad.Show();
        }
    }
}
