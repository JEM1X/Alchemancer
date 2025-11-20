using System.Collections.Generic;

public interface IAnalyticsProvider
{
    void Initialize(string appKey, string userId);
    void TrackEvent(string name, IDictionary<string, string> eventParams = null);
    void TrackGameStartEvent();
    void TrackLevelEvent(int level, IDictionary<string, string> eventParams = null);
    void Flush();
}
