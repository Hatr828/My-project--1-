using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public sealed class MainMenuUIToolkit : MonoBehaviour
{
    [SerializeField] private Font titleFont;
    [SerializeField] private Font buttonFont;

    private UIDocument document;
    private PanelSettings runtimePanelSettings;

    private static readonly string[] SceneNames =
    {
        "Prototype 1",
        "Prototype 2",
        "Prototype 3",
        "Prototype 4",
        "Prototype 5",
    };

    private static readonly Color[] AccentColors =
    {
        new Color(0.16f, 0.55f, 0.95f),
        new Color(0.22f, 0.76f, 0.45f),
        new Color(0.98f, 0.69f, 0.2f),
        new Color(0.96f, 0.39f, 0.39f),
        new Color(0.24f, 0.76f, 0.95f),
    };

    private void OnEnable()
    {
        EnsureDocument();
        BuildUI();
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
            }

            document.panelSettings = runtimePanelSettings;
        }
    }

    private void BuildUI()
    {
        var root = document.rootVisualElement;
        root.Clear();
        root.style.flexGrow = 1;
        root.style.backgroundColor = new Color(0.05f, 0.07f, 0.12f);
        root.style.alignItems = Align.Stretch;

        var background = new VisualElement();
        background.pickingMode = PickingMode.Ignore;
        background.style.position = Position.Absolute;
        background.style.left = 0;
        background.style.right = 0;
        background.style.top = 0;
        background.style.bottom = 0;
        root.Add(background);

        background.Add(CreateGlowBlob(new Color(0.08f, 0.42f, 0.65f), new Vector2(-120f, -180f), 520f, 0.45f));
        background.Add(CreateGlowBlob(new Color(0.92f, 0.36f, 0.28f), new Vector2(420f, 320f), 520f, 0.35f));
        background.Add(CreateGlowBlob(new Color(0.2f, 0.7f, 0.85f), new Vector2(520f, -220f), 420f, 0.2f));

        var content = new VisualElement();
        content.style.flexGrow = 1;
        content.style.justifyContent = Justify.Center;
        content.style.alignItems = Align.Center;
        content.style.paddingLeft = 24;
        content.style.paddingRight = 24;
        content.style.paddingTop = 40;
        content.style.paddingBottom = 40;
        root.Add(content);

        var card = new VisualElement();
        card.style.width = new Length(85f, LengthUnit.Percent);
        card.style.maxWidth = 520;
        card.style.minWidth = 300;
        card.style.backgroundColor = new Color(0.08f, 0.11f, 0.18f, 0.9f);
        card.style.borderTopLeftRadius = 24;
        card.style.borderTopRightRadius = 24;
        card.style.borderBottomLeftRadius = 24;
        card.style.borderBottomRightRadius = 24;
        card.style.borderLeftWidth = 1;
        card.style.borderRightWidth = 1;
        card.style.borderTopWidth = 1;
        card.style.borderBottomWidth = 1;
        card.style.borderLeftColor = new Color(1f, 1f, 1f, 0.12f);
        card.style.borderRightColor = new Color(1f, 1f, 1f, 0.12f);
        card.style.borderTopColor = new Color(1f, 1f, 1f, 0.12f);
        card.style.borderBottomColor = new Color(1f, 1f, 1f, 0.12f);
        card.style.paddingLeft = 28;
        card.style.paddingRight = 28;
        card.style.paddingTop = 28;
        card.style.paddingBottom = 20;
        content.Add(card);

        var header = new VisualElement();
        header.style.flexDirection = FlexDirection.Column;
        header.style.alignItems = Align.Center;
        header.style.marginBottom = 18;
        card.Add(header);

        var title = new Label("MAIN MENU");
        ApplyTitleStyle(title);
        header.Add(title);

        var subtitle = new Label("Select a prototype to start");
        ApplySubtitleStyle(subtitle);
        header.Add(subtitle);

        var divider = new VisualElement();
        divider.style.height = 1;
        divider.style.marginTop = 14;
        divider.style.marginBottom = 18;
        divider.style.alignSelf = Align.Stretch;
        divider.style.backgroundColor = new Color(1f, 1f, 1f, 0.08f);
        header.Add(divider);

        var buttons = new VisualElement();
        buttons.style.flexDirection = FlexDirection.Column;
        buttons.style.alignItems = Align.Stretch;
        card.Add(buttons);

        for (int i = 0; i < SceneNames.Length; i++)
        {
            var label = SceneNames[i];
            var accent = AccentColors[i % AccentColors.Length];
            var button = CreateMenuButton(label, accent);
            if (i == SceneNames.Length - 1)
            {
                button.style.marginBottom = 0;
            }

            buttons.Add(button);
        }
    }

    private void ApplyTitleStyle(Label label)
    {
        label.style.unityFont = titleFont ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.fontSize = 42;
        label.style.color = new Color(0.96f, 0.98f, 1f);
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.marginBottom = 2;
    }

    private void ApplySubtitleStyle(Label label)
    {
        label.style.unityFont = buttonFont ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.style.fontSize = 14;
        label.style.color = new Color(0.55f, 0.67f, 0.85f);
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
    }

    private Button CreateMenuButton(string label, Color accent)
    {
        var button = new Button();
        button.text = label;
        button.style.height = 56;
        button.style.marginBottom = 12;
        button.style.paddingLeft = 16;
        button.style.paddingRight = 16;
        button.style.backgroundColor = new Color(accent.r, accent.g, accent.b, 0.85f);
        button.style.borderTopLeftRadius = 16;
        button.style.borderTopRightRadius = 16;
        button.style.borderBottomLeftRadius = 16;
        button.style.borderBottomRightRadius = 16;
        button.style.borderLeftWidth = 1;
        button.style.borderRightWidth = 1;
        button.style.borderTopWidth = 1;
        button.style.borderBottomWidth = 1;
        var borderColor = new Color(1f, 1f, 1f, 0.2f);
        button.style.borderLeftColor = borderColor;
        button.style.borderRightColor = borderColor;
        button.style.borderTopColor = borderColor;
        button.style.borderBottomColor = borderColor;
        button.style.color = Color.white;
        button.style.unityFont = buttonFont ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        button.style.fontSize = 20;
        button.style.unityTextAlign = TextAnchor.MiddleCenter;
        button.style.alignItems = Align.Center;
        button.style.justifyContent = Justify.Center;

        var normal = new Color(accent.r, accent.g, accent.b, 0.85f);
        var hover = Lighten(accent, 0.12f);
        hover.a = 1f;
        var pressed = Darken(accent, 0.12f);
        pressed.a = 1f;

        button.RegisterCallback<PointerEnterEvent>(_ => button.style.backgroundColor = hover);
        button.RegisterCallback<PointerLeaveEvent>(_ => button.style.backgroundColor = normal);
        button.RegisterCallback<PointerDownEvent>(_ => button.style.backgroundColor = pressed);
        button.RegisterCallback<PointerUpEvent>(_ => button.style.backgroundColor = hover);

        button.clicked += () => SceneManager.LoadScene(label);
        return button;
    }

    private VisualElement CreateGlowBlob(Color color, Vector2 position, float size, float opacity)
    {
        var blob = new VisualElement();
        blob.pickingMode = PickingMode.Ignore;
        blob.style.position = Position.Absolute;
        blob.style.left = position.x;
        blob.style.top = position.y;
        blob.style.width = size;
        blob.style.height = size;
        blob.style.backgroundColor = color;
        blob.style.opacity = opacity;
        var radius = size * 0.5f;
        blob.style.borderTopLeftRadius = radius;
        blob.style.borderTopRightRadius = radius;
        blob.style.borderBottomLeftRadius = radius;
        blob.style.borderBottomRightRadius = radius;
        return blob;
    }

    private static Color Lighten(Color color, float amount)
    {
        return new Color(
            Mathf.Clamp01(color.r + amount),
            Mathf.Clamp01(color.g + amount),
            Mathf.Clamp01(color.b + amount),
            color.a);
    }

    private static Color Darken(Color color, float amount)
    {
        return new Color(
            Mathf.Clamp01(color.r - amount),
            Mathf.Clamp01(color.g - amount),
            Mathf.Clamp01(color.b - amount),
            color.a);
    }
}
