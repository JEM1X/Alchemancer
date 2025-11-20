using UnityEngine;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private readonly List<IAnalyticsProvider> _providers = new List<IAnalyticsProvider>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeProviders();
    }

    private void InitializeProviders()
    {
        // В зависимости от билда можно вкл/выкл разные SDK

#if UNITY_ANDROID
        _providers.Add(new FakeAnalyticsProvider());
#elif UNITY_STANDALONE
        _providers.Add(new FakeAnalyticsProvider());
#endif

        foreach (var provider in _providers)
        {
            provider.Initialize("APP_KEY_123", SystemInfo.deviceUniqueIdentifier);
        }

        TrackGameStart();
    }

    public void TrackGameStart()
    {
        foreach (var p in _providers) p.TrackGameStartEvent();
    }

    public void TrackEvent(string name, Dictionary<string, string> p = null)
    {
        foreach (var s in _providers) s.TrackEvent(name, p);
    }

    public void TrackLevel(int level, Dictionary<string, string> p = null)
    {
        foreach (var s in _providers) s.TrackLevelEvent(level, p);
    }

    public void Flush()
    {
        foreach (var p in _providers) p.Flush();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            Flush();
    }
}
