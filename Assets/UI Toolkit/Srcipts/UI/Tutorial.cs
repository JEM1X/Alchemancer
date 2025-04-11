using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;


    private void Start()
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

        var welcomeScreen = UITK.AddElement(canvas, "welcomeScreen", "InGameFrame");

        var welcomeLabel = UITK.AddElement<Label>(welcomeScreen, "welcomeLabel", "MainText");
        welcomeLabel.text = "����� ���������� � Alchemancer." +
            "�� � �������-�������, �������� ��������� ��������� ������ ����� ��������." +
            "��� ������ ��� ����� ������ ��� �����, ������ �� ������� ������� �� ��� ���� ������." +
            "� ������ ����� ������ ��������� ����� �������������, � ��� ������� ������������" +
            "��� ���� ������������ ��������, ����� ������.";

        var welcomeButton = UITK.AddElement<Button>(welcomeScreen, "welcomeButton", "MainButton");
        welcomeButton.text = "OK";
        welcomeButton.clicked += () => welcomeScreen.style.display = DisplayStyle.None;

        var handScreen = UITK.AddElement(canvas, "handScreen", "InGameFrame");
        handScreen.style.display = DisplayStyle.None;
        welcomeButton.clicked += () => handScreen.style.display = DisplayStyle.Flex;

        var handLabel = UITK.AddElement<Label>(handScreen, "handLabel", "ClearText");
        handLabel.text = "� ��� ��� ������� �����, ������� ������� �� ��� ����: ��������, ������� � �����." +
            "�������� ����������� �� ������, ������� ������������ �� ������ �����, � ����� �������� ���� ������ �����." +
            "������ � ������ ������� ����� � ��� �� ����� �� ������ �����." +
            "����� �� �������, ��� ����� ��������� ��������� �����������." +
            "������ ����� ����� ���������� ������, ��������� �� ��� ������ ������������." +
            "� ������ ������ ���� ��� ����� ������ ����� ���� ������������." +
            "�������� ������� ��� ����� ������������";

        var handButton = UITK.AddElement<Button>(handScreen, "handButton", "MainButton");
        handButton.text = "OK";
        handButton.clicked += () => handScreen.style.display = DisplayStyle.None;


    }
}
