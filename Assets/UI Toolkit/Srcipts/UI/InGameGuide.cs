using UnityEngine;
using UnityEngine.UIElements;

public class InGameGuide : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement background;
    private bool isVisible = true;


    private void Awake()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");

        background = UITK.AddElement(canvas, "background");

        var guideFrame = UITK.AddElement(background, "guideFrame");

        var guidePage = UITK.AddElement(guideFrame, "guidePage");

        var pageLabel = UITK.AddElement<Label>(guidePage, "pageLabel");
        pageLabel.text = "�� � ������� - �������, �������� ��������� ��������� ������ ����� ��������." +
            "��� ������ ��� ����� ������ ��� �����, ������ �� ������� ������� �� ��� ���� ������." +
            "� ������ ����� ������ ��������� ����� �������������," +
            "� ��� ������� ������������ ��� ���� ������������ ��������, ����� ������." +
            "� ��� ��� ������� �����, ������� ������� �� ��� ����: ��������, ������� � �����." +
            "�������� ����������� �� ������, ������� ������������ �� ������ �����, � ����� �������� ���� ������ �����." +
            "������ � ������ ������� ����� � ��� �� ����� �� ������ �����.";

        var guidePage1 = UITK.AddElement(guideFrame, "guidePage");

        var page1Label = UITK.AddElement<Label>(guidePage1, "pageLabel");
        page1Label.text = "����� �� �������, ��� ����� ��������� ��������� �����������." +
            "������ ����� ����� ���������� ������, ��������� �� ��� ������ ������������." +
            "� ������ ������ ���� ��� ����� ������ ����� ���� ������������." +
            "����������� ��, ����� ��������� ����� � ��������� �� � ���." +
            "����� � ��� ���������� ����������� � �����, ������� '��������� ���' � ����� ��� ������� � ����� �����������." +
            "����� ���� ��� ��� ����� �������� ���� ����, ������� ����� �����, � ��� ����� ������� � ���." +
            "���� ���� � �������� ���� ������ � �� ��������� �� ����. �����!";

        var toggleButton = UITK.AddElement<Button>(canvas, "toggleButton");
        toggleButton.text = "����";
        toggleButton.clicked += ToggleGuide;

        ToggleGuide();
    }

    private void ToggleGuide()
    {
        if (isVisible)
        {
            background.style.display = DisplayStyle.None;
            isVisible = false;
        }
        else
        {
            background.style.display = DisplayStyle.Flex;
            isVisible = true;
        }
    }
}
