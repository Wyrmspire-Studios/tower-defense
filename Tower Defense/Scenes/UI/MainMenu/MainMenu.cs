using Godot;
using System;
using System.Collections;
using Godot.Collections;

namespace WyrmspireStudios;

[Tool]
public partial class MainMenu : Control
{
    [Export] public Dictionary<string, PackedScene> TestingScenes;
    [Export] public PackedScene PlayScene;
    [Export] public PackedScene OptionsScene;

    [Export] public NinePatchRect TestingSelection;
    [Export] public VBoxContainer TestingButtonContainer;

    [ExportGroup("CustomTexturedButton")] [Export]
    public PackedScene CustomTexturedButton;

    [Export]
    public Texture2D BaseTexture
    {
        get => _baseTexture;
        set
        {
            _baseTexture = value;
            _Ready();
        }
    }

    [Export]
    public Texture2D HoverTexture
    {
        get => _hoverTexture;
        set
        {
            _hoverTexture = value;
            _Ready();
        }
    }

    [Export]
    public Texture2D PressedTexture
    {
        get => _pressedTexture;
        set
        {
            _pressedTexture = value;
            _Ready();
        }
    }

    [Export]
    public int FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value > 0 ? value : 8;
            _Ready();
        }
    }

    private Texture2D _baseTexture;
    private Texture2D _hoverTexture;
    private Texture2D _pressedTexture;
    private int _fontSize;

    public override void _Ready()
    {
        var buttonContainer = GetNodeOrNull("ButtonContainer");

        if (buttonContainer == null) return;

        var testingButton = buttonContainer.GetNode<CustomTexturedButton>("TestingButton");
        SetTexturesOnCustomTexturedButton(testingButton);
        testingButton.FontSize = _fontSize;
        testingButton.Pressed += TestingButton_OnPressed;

        var playButton = buttonContainer.GetNode<CustomTexturedButton>("PlayButton");
        SetTexturesOnCustomTexturedButton(playButton);
        playButton.FontSize = _fontSize;
        playButton.Pressed += PlayButton_OnPressed;

        var settingsButton = buttonContainer.GetNode<CustomTexturedButton>("SettingsButton");
        SetTexturesOnCustomTexturedButton(settingsButton);
        settingsButton.FontSize = _fontSize;
        settingsButton.Pressed += SettingsButton_OnPressed;

        var exitButton = buttonContainer.GetNode<CustomTexturedButton>("ExitButton");
        SetTexturesOnCustomTexturedButton(exitButton);
        exitButton.FontSize = _fontSize;
        exitButton.Pressed += ExitButton_OnPressed;

        CreateTestingSelection();
    }

    private void TestingButton_OnPressed()
    {
        if (!TestingSelection.Visible)
        {
            TestingSelection.Show();
        }
    }

    private void PlayButton_OnPressed()
    {
        GetTree().ChangeSceneToPacked(PlayScene);
    }

    private void SettingsButton_OnPressed()
    {
        GetTree().ChangeSceneToPacked(OptionsScene);
    }

    private void ExitButton_OnPressed()
    {
        GetTree().Quit();
    }


    private void SetTexturesOnCustomTexturedButton(CustomTexturedButton customTexturedButton)
    {
        if (customTexturedButton == null) return;
        customTexturedButton.BaseTexture = _baseTexture;
        customTexturedButton.HoverTexture = _hoverTexture;
        customTexturedButton.PressedTexture = _pressedTexture;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true } eventKey)
        {
            if (eventKey.Keycode == Key.Escape && TestingSelection.Visible)
            {
                TestingSelection.Hide();
            }
        }
    }


    private void CreateTestingSelection()
    {
        if (TestingScenes == null) {
            return; 
        }
        foreach (var scene in TestingScenes)
        {
            var button = CustomTexturedButton.Instantiate<CustomTexturedButton>();
            SetTexturesOnCustomTexturedButton(button);
            button.CustomMinimumSize = new Vector2(0, 50);
            button.FontSize = _fontSize;
            button.ButtonLabel = scene.Key;
            button.Pressed += () => { GetTree().ChangeSceneToPacked(scene.Value); };
            TestingButtonContainer.AddChild(button);
        }
    }
}