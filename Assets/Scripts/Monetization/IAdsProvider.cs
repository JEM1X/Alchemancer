using System;

public interface IAdsProvider
{
    void Initialize();
    void ShowBanner();
    void ShowInterstitial();
    void ShowRewarded(Action onRewarded);
}