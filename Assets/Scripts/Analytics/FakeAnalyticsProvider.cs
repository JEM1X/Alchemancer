using System.Collections.Generic;
using FakeAnalytics;

public class FakeAnalyticsProvider : IAnalyticsProvider
{
    private FakeAnalyticsSDK _sdk = new FakeAnalyticsSDK();

    public void Initialize(string appKey, string userId)
    {
        _sdk.Initialize(appKey, userId);
    }

    public void TrackEvent(string name, IDictionary<string, string> eventParams = null)
    {
        _sdk.TrackEvent(name, eventParams);
    }

    public void TrackGameStartEvent()
    {
        _sdk.TrackGameStartEvent();
    }

    public void TrackLevelEvent(int level, IDictionary<string, string> eventParams = null)
    {
        _sdk.TrackLevelEvent(level, eventParams);
    }

    public void Flush() => _sdk.Flush();
}
