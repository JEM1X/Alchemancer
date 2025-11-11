using System;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private IAdsProvider provider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Какой сдк нам нужен
        provider = new SuperMobileAdsProvider();
        provider.Initialize();
    }

    public void ShowBanner() => provider.ShowBanner();
    public void ShowInterstitial() => provider.ShowInterstitial();
    public void ShowRewarded(Action onRewarded) => provider.ShowRewarded(onRewarded);
}