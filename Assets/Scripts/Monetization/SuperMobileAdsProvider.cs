using System;
using SuperMobileAds;
using UnityEngine;

public class SuperMobileAdsProvider : IAdsProvider
{
    private SuperMobileAdsBanner banner;
    private SuperMobileAdsInterstitial interstitial;
    private SuperMobileAdsRewarded rewarded;

    public void Initialize()
    {
        banner = new SuperMobileAdsBanner();
        interstitial = new SuperMobileAdsInterstitial();
        rewarded = new SuperMobileAdsRewarded();

        banner.Initialize("banner_1");
        interstitial.Initialize("interstitial_1");
        rewarded.Initialize("rewarded_1");

        banner.Load();
        interstitial.Load();
        rewarded.Load();

        // Подписки на события можно добавить здесь
        interstitial.onAdLoaded += () => Debug.Log("Interstitial loaded!");
        rewarded.onAdRewarded += () => Debug.Log("Player rewarded!");
    }

    public void ShowBanner()
    {
        banner.Show();
    }

    public void ShowInterstitial()
    {
        interstitial.Show();
    }

    public void ShowRewarded(Action onRewarded)
    {
        rewarded.onAdRewarded += onRewarded;
        rewarded.Show();
    }
}