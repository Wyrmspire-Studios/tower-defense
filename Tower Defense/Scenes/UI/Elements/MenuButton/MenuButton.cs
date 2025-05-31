using Godot;

namespace WyrmspireStudios.UI;

[Tool]
public partial class MenuButton : Button
{
    private NinePatchRect _background;
    private NinePatchRect _backgroundShadow;
    private Label _label;
    private bool _eventsConnected;

    private int _labelFontSize = 16;
    private string _labelText = "Label";

    private Texture2D _baseTexture;
    private Texture2D _hoverTexture;
    private Texture2D _pressedTexture;

    [Export]
    public int FontSize
    {
        get => _labelFontSize;
        set => UpdateLabelFontSize(value);
    }

    public void UpdateLabelFontSize(int fontSize)
    {
        _labelFontSize = fontSize;
        Label label = GetNodeOrNull<Label>("Label");
        if (label != null)
        {
            LabelSettings labelSettings = new LabelSettings();
            labelSettings.FontSize = _labelFontSize > 0 ? _labelFontSize : 8;
            label.LabelSettings = labelSettings;
        }
    }

    [Export]
    public string Label
    {
        get => _labelText;
        set => UpdateLabelText(value);
    }

    private void UpdateLabelText(string text)
    {
        _labelText = text;
        Label label = GetNodeOrNull<Label>("Label");
        if (label != null)
        {
            label.Text = text;
        }
    }

    [Export]
    public Texture2D BaseTexture
    {
        get => _baseTexture;
        set => UpdateBaseTexture(value);
    }

    public void UpdateBaseTexture(Texture2D texture)
    {
        _baseTexture = texture;
        NinePatchRect background = GetNodeOrNull<NinePatchRect>("Background");
        NinePatchRect backgroundShadow = GetNodeOrNull<NinePatchRect>("Background/BackgroundShadow");
        if (background != null && backgroundShadow != null)
        {
            background.Texture = _baseTexture;
            backgroundShadow.Texture = _baseTexture;
        }
    }


    [Export]
    public Texture2D HoverTexture
    {
        get => _hoverTexture;
        set => UpdateHoverTexture(value);
    }

    public void UpdateHoverTexture(Texture2D texture)
    {
        _hoverTexture = texture;
    }


    [Export]
    public Texture2D PressedTexture
    {
        get => _pressedTexture;
        set => UpdatePressedTexture(value);
    }

    public void UpdatePressedTexture(Texture2D texture)
    {
        _pressedTexture = texture;
    }

    public override void _Ready()
    {
        _background = GetNode<NinePatchRect>("Background");
        _backgroundShadow = GetNode<NinePatchRect>("Background/BackgroundShadow");
        _label = GetNode<Label>("Label");

        ButtonDown += OnButtonDown;
        ButtonUp += OnButtonUp;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        _eventsConnected = true;
    }

    public override void _ExitTree()
    {
        if (_eventsConnected) return;
        ButtonDown -= OnButtonDown;
        ButtonUp -= OnButtonUp;
        MouseEntered -= OnMouseEntered;
        MouseExited -= OnMouseExited;
        _eventsConnected = false;
    }

    private void OnButtonDown()
    {
        _background.Texture = _pressedTexture;
        _backgroundShadow.Texture = _pressedTexture;
    }

    private void OnButtonUp()
    {
        var texture = GetRect().HasPoint(GetLocalMousePosition()) ? _hoverTexture : _baseTexture;
        _background.Texture = texture;
        _backgroundShadow.Texture = texture;
    }

    private void OnMouseEntered()
    {
        if (!IsPressed())
        {
            _background.Texture = _hoverTexture;
            _backgroundShadow.Texture = _hoverTexture;
        }
    }

    private void OnMouseExited()
    {
        if (!IsPressed())
        {
            _background.Texture = _baseTexture;
            _backgroundShadow.Texture = _baseTexture;
        }
    }
}