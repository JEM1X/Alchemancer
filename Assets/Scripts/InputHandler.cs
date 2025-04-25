using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InGameMenu inGameMenu;
    [SerializeField] private RecipeBook recipeBook;
    [SerializeField] private DebugUI debug;

    private InputController controller;


    private void Awake()
    {
        controller = new InputController();

        controller.Player.RightClick.performed += evt => recipeBook.ToggleGuide();

        controller.Player.Pause.performed += evt => inGameMenu.TogglePauseScreen();

        controller.Player.Debug.performed += evt => debug.ToggleDebug();
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }
}
