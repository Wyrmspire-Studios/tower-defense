using Godot;
using Godot.Collections;

namespace WyrmspireStudios.UI;

[Tool]
public partial class MenuButtonGroupController : Control
{
    private Array<MenuButton> _menuButtons;

    private int _labelFontSize = 16;

    private Texture2D _baseTexture;
    private Texture2D _hoverTexture;
    private Texture2D _pressedTexture;

    [Export]
    public Array<MenuButton> MenuButtons
    {
        get => _menuButtons;
        set => UpdateMenuButtons(value);
    }

    private void UpdateMenuButtons(Array<MenuButton> menuButtons)
    {
        _menuButtons = menuButtons;
        if (_menuButtons == null) return;
        foreach (var button in _menuButtons)
        {
            if (button == null) continue;
            button.UpdateLabelFontSize(_labelFontSize);
            button.UpdateBaseTexture(_baseTexture);
            button.UpdateHoverTexture(_hoverTexture);
            button.UpdatePressedTexture(_pressedTexture);
        }
    }

    [Export]
    public int FontSize
    {
        get => _labelFontSize;
        set => UpdateLabelFontSize(value);
    }

    private void UpdateLabelFontSize(int fontSize)
    {
        _labelFontSize = fontSize;
        if (_menuButtons == null) return;
        foreach (var button in _menuButtons)
        {
            button?.UpdateLabelFontSize(_labelFontSize);
        }
    }

    [Export]
    public Texture2D BaseTexture
    {
        get => _baseTexture;
        set => UpdateBaseTexture(value);
    }

    private void UpdateBaseTexture(Texture2D texture)
    {
        _baseTexture = texture;
        if (_menuButtons == null) return;
        foreach (var button in _menuButtons)
        {
            button?.UpdateBaseTexture(_baseTexture);
        }
    }


    [Export]
    public Texture2D HoverTexture
    {
        get => _hoverTexture;
        set => UpdateHoverTexture(value);
    }

    private void UpdateHoverTexture(Texture2D texture)
    {
        _hoverTexture = texture;
        if (_menuButtons == null) return;
        foreach (var button in _menuButtons)
        {
            button?.UpdateHoverTexture(_hoverTexture);
        }
    }


    [Export]
    public Texture2D PressedTexture
    {
        get => _pressedTexture;
        set => UpdatePressedTexture(value);
    }

    private void UpdatePressedTexture(Texture2D texture)
    {
        _pressedTexture = texture;
        if (_menuButtons == null) return;
        foreach (var button in _menuButtons)
        {
            button?.UpdatePressedTexture(_pressedTexture);
        }
    }

    public override void _Ready()
    {
        base._Ready();
    }
}