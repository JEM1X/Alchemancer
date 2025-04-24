using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UIElements;

public class LabelAutoFit : Label
{
    [ExcludeFromDocs]
    [Serializable]
    public new class UxmlSerializedData : TextElement.UxmlSerializedData
    {
        public override object CreateInstance() => new LabelAutoFit();
    }

    public LabelAutoFit()
    {
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
    }

    private void OnAttachToPanel(AttachToPanelEvent e)
    {
        UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent e)
    {
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        var previousWidthStyle = style.width;

        try
        {
            // Set width to auto temporarily to get the actual width of the label
            style.width = StyleKeyword.Auto;
            var currentFontSize = MeasureTextSize(text, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined);

            var multiplier = resolvedStyle.width / Mathf.Max(currentFontSize.x, 1);
            var newFontSize = Mathf.RoundToInt(Mathf.Clamp(multiplier * currentFontSize.y, 1, resolvedStyle.height));

            if (Mathf.RoundToInt(currentFontSize.y) != newFontSize)
                style.fontSize = new StyleLength(new Length(newFontSize));
        }
        finally
        {
            style.width = previousWidthStyle;
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
    }
}