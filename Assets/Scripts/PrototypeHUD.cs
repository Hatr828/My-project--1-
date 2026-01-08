using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public sealed class PrototypeHUD : MonoBehaviour
{
    private const string MainMenuSceneName = "MainMenu";
    private static PrototypeHUD instance;
    private static bool registered;

    private UIDocument document;
    private PanelSettings runtimePanelSettings;
    private VisualElement overlay;
    private bool gameOverShown;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Register()
    {
        if (registered)
        {
            return;
        }

        registered = true;
        SceneManager.sceneLoaded += HandleSceneLoaded;
        HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsPrototypeScene(scene.name))
        {
            return;
        }

        if (instance != null)
        {
            instance.ResetOverlay();
            return;
        }

        var go = new GameObject("PrototypeHUD");
        instance = go.AddComponent<PrototypeHUD>();
    }

    public static void ShowGameOver()
    {
        if (instance == null)
        {
            var scene = SceneManager.GetActiveScene();
            if (!IsPrototypeScene(scene.name))
            {
                return;
            }

            var go = new GameObject("PrototypeHUD");
            instance = go.AddComponent<PrototypeHUD>();
        }

        instance.ShowGameOverInternal();
    }

    private static bool IsPrototypeScene(string sceneName)
    {
        return !string.IsNullOrWhiteSpace(sceneName) && sceneName.StartsWith("Prototype ");
    }

    private void OnEnable()
    {
        EnsureDocument();
        BuildUI();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void EnsureDocument()
    {
        if (document == null)
        {
            document = GetComponent<UIDocument>();
            if (document == null)
            {
                document = gameObject.AddComponent<UIDocument>();
            }
        }

        if (document.panelSettings == null)
        {
            if (runtimePanelSettings == null)
            {
                runtimePanelSettings = ScriptableObject.CreateInstance<PanelSettings>();
                runtimePanelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
                runtimePanelSettings.referenceResolution = new Vector2Int(1920, 1080);
                runtimePanelSettings.match = 0.5f;
                runtimePanelSettings.sortingOrder = 100;
            }

            document.panelSettings = runtimePanelSettings;
        }
    }

    private void BuildUI()
    {
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        var root = document.rootVisualElement;
        root.Clear();
        root.style.flexGrow = 1;
        root.style.backgroundColor = Color.clear;

        overlay = new VisualElement();
        overlay.pickingMode = PickingMode.Ignore;
        overlay.style.position = Position.Absolute;
        overlay.style.left = 0;
        overlay.style.right = 0;
        overlay.style.top = 0;
        overlay.style.bottom = 0;
        overlay.style.alignItems = Align.Center;
        overlay.style.justifyContent = Justify.Center;
        overlay.style.backgroundColor = new Color(0f, 0f, 0f, 0.55f);
        overlay.style.display = DisplayStyle.None;
        root.Add(overlay);

        var card = new VisualElement();
        card.style.minWidth = 280;
        card.style.maxWidth = 520;
        card.style.backgroundColor = new Color(0.08f, 0.11f, 0.18f, 0.95f);
        card.style.borderTopLeftRadius = 20;
        card.style.borderTopRightRadius = 20;
        card.style.borderBottomLeftRadius = 20;
        card.style.borderBottomRightRadius = 20;
        card.style.borderLeftWidth = 1;
        card.style.borderRightWidth = 1;
        card.style.borderTopWidth = 1;
        card.style.borderBottomWidth = 1;
        var borderColor = new Color(1f, 1f, 1f, 0.12f);
        card.style.borderLeftColor = borderColor;
        card.style.borderRightColor = borderColor;
        card.style.borderTopColor = borderColor;
        card.style.borderBottomColor = borderColor;
        card.style.paddingLeft = 24;
        card.style.paddingRight = 24;
        card.style.paddingTop = 20;
        card.style.paddingBottom = 20;
        card.style.alignItems = Align.Center;
        overlay.Add(card);

        var gameOverLabel = new Label("GAME OVER");
        gameOverLabel.style.unityFont = font;
        gameOverLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        gameOverLabel.style.fontSize = 36;
        gameOverLabel.style.color = new Color(0.98f, 0.98f, 1f);
        gameOverLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        card.Add(gameOverLabel);

        var hint = new Label("Back to the main menu");
        hint.style.unityFont = font;
        hint.style.fontSize = 14;
        hint.style.color = new Color(0.65f, 0.75f, 0.9f);
        hint.style.unityTextAlign = TextAnchor.MiddleCenter;
        hint.style.marginTop = 6;
        card.Add(hint);

        var buttonRow = new VisualElement();
        buttonRow.style.flexDirection = FlexDirection.Row;
        buttonRow.style.flexWrap = Wrap.Wrap;
        buttonRow.style.alignItems = Align.Center;
        buttonRow.style.justifyContent = Justify.Center;
        buttonRow.style.marginTop = 14;
        card.Add(buttonRow);

        var playAgainButton = CreateOverlayButton(
            "Play Again",
            font,
            new Color(0.2f, 0.6f, 0.32f, 0.95f),
            new Color(0.26f, 0.7f, 0.4f, 1f),
            new Color(0.14f, 0.46f, 0.24f, 1f));
        playAgainButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        buttonRow.Add(playAgainButton);

        var backToMenuButton = CreateOverlayButton(
            "Back",
            font,
            new Color(0.14f, 0.26f, 0.42f, 0.95f),
            new Color(0.2f, 0.32f, 0.52f, 1f),
            new Color(0.08f, 0.18f, 0.3f, 1f));
        backToMenuButton.clicked += () => SceneManager.LoadScene(MainMenuSceneName);
        buttonRow.Add(backToMenuButton);

        var backButton = CreateBackButton(font);
        root.Add(backButton);
    }

    private Button CreateBackButton(Font font)
    {
        var button = new Button();
        button.text = "Back";
        button.style.position = Position.Absolute;
        button.style.top = 20;
        button.style.right = 20;
        button.style.height = 40;
        button.style.minWidth = 120;
        button.style.paddingLeft = 16;
        button.style.paddingRight = 16;
        button.style.backgroundColor = new Color(0.08f, 0.12f, 0.2f, 0.92f);
        button.style.borderTopLeftRadius = 12;
        button.style.borderTopRightRadius = 12;
        button.style.borderBottomLeftRadius = 12;
        button.style.borderBottomRightRadius = 12;
        button.style.borderLeftWidth = 1;
        button.style.borderRightWidth = 1;
        button.style.borderTopWidth = 1;
        button.style.borderBottomWidth = 1;
        var borderColor = new Color(0.4f, 0.65f, 0.95f, 0.65f);
        button.style.borderLeftColor = borderColor;
        button.style.borderRightColor = borderColor;
        button.style.borderTopColor = borderColor;
        button.style.borderBottomColor = borderColor;
        button.style.color = new Color(0.92f, 0.96f, 1f);
        button.style.unityFont = font;
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        button.style.fontSize = 18;
        button.style.unityTextAlign = TextAnchor.MiddleCenter;
        button.style.alignItems = Align.Center;
        button.style.justifyContent = Justify.Center;

        var normal = new Color(0.08f, 0.12f, 0.2f, 0.92f);
        var hover = new Color(0.16f, 0.2f, 0.32f, 0.98f);
        var pressed = new Color(0.04f, 0.07f, 0.12f, 1f);

        button.RegisterCallback<PointerEnterEvent>(_ => button.style.backgroundColor = hover);
        button.RegisterCallback<PointerLeaveEvent>(_ => button.style.backgroundColor = normal);
        button.RegisterCallback<PointerDownEvent>(_ => button.style.backgroundColor = pressed);
        button.RegisterCallback<PointerUpEvent>(_ => button.style.backgroundColor = hover);

        button.clicked += () => SceneManager.LoadScene(MainMenuSceneName);
        return button;
    }

    private Button CreateOverlayButton(string text, Font font, Color normal, Color hover, Color pressed)
    {
        var button = new Button();
        button.text = text;
        button.style.height = 44;
        button.style.minWidth = 150;
        button.style.marginLeft = 6;
        button.style.marginRight = 6;
        button.style.marginTop = 6;
        button.style.marginBottom = 6;
        button.style.paddingLeft = 16;
        button.style.paddingRight = 16;
        button.style.backgroundColor = normal;
        button.style.borderTopLeftRadius = 14;
        button.style.borderTopRightRadius = 14;
        button.style.borderBottomLeftRadius = 14;
        button.style.borderBottomRightRadius = 14;
        button.style.borderLeftWidth = 1;
        button.style.borderRightWidth = 1;
        button.style.borderTopWidth = 1;
        button.style.borderBottomWidth = 1;
        var borderColor = new Color(1f, 1f, 1f, 0.2f);
        button.style.borderLeftColor = borderColor;
        button.style.borderRightColor = borderColor;
        button.style.borderTopColor = borderColor;
        button.style.borderBottomColor = borderColor;
        button.style.color = new Color(0.95f, 0.98f, 1f);
        button.style.unityFont = font;
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        button.style.fontSize = 18;
        button.style.unityTextAlign = TextAnchor.MiddleCenter;
        button.style.alignItems = Align.Center;
        button.style.justifyContent = Justify.Center;

        button.RegisterCallback<PointerEnterEvent>(_ => button.style.backgroundColor = hover);
        button.RegisterCallback<PointerLeaveEvent>(_ => button.style.backgroundColor = normal);
        button.RegisterCallback<PointerDownEvent>(_ => button.style.backgroundColor = pressed);
        button.RegisterCallback<PointerUpEvent>(_ => button.style.backgroundColor = hover);

        return button;
    }

    private void ShowGameOverInternal()
    {
        if (gameOverShown)
        {
            return;
        }

        gameOverShown = true;
        if (overlay != null)
        {
            overlay.style.display = DisplayStyle.Flex;
        }
    }

    private void ResetOverlay()
    {
        gameOverShown = false;
        if (overlay != null)
        {
            overlay.style.display = DisplayStyle.None;
        }
    }
}
