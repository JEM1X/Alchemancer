using UnityEngine.Localization;
using UnityEngine.UIElements;

public static class LTK
{
    public const string MAINTABLE = "UI";

    public static LocalizedString LocalizeStringUITK(TextElement element, string table, string key)
    {
        var localString = new LocalizedString(table, key);
        localString.StringChanged += (value) => element.text = value;

        return localString;
    }

    public static LocalizedString LocalizeStringUITK(TextElement element, string table, string key, string addition)
    {
        var localString = new LocalizedString(table, key);
        localString.StringChanged += (value) => element.text = value + addition;

        return localString;
    }
}
