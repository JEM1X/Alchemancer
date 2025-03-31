using UnityEngine;
using UnityEngine.UIElements;

public class UITK
{
    public static VisualElement AddElement(VisualElement parent, params string[] classNames)
    {
        return AddElement<VisualElement>(parent, classNames);
    }

    public static T AddElement<T>(VisualElement parent, params string[] classNames) where T : VisualElement, new()
    {
        var element = CreateElement<T>(classNames);
        parent.Add(element);
        return element;
    }

    public static VisualElement CreateElement(params string[] classNames)
    {
        return CreateElement<VisualElement>(classNames);
    }

    public static T CreateElement<T>(params string[] classNames) where T : VisualElement, new()
    {
        var element = new T();
        foreach (var className in classNames)
            element.AddToClassList(className);

        return element;
    }
}
