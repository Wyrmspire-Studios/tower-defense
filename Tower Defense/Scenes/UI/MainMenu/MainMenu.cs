using Godot;
using System;
using Godot.Collections;

namespace WyrmspireStudios;

public partial class MainMenu : Control
{
    [Export] public Array<PackedScene> TestingScenes;
    [Export] public PackedScene PlayScene;
    [Export] public PackedScene OptionsScene;
    
    [Export] public PackedScene CustomTexturedButton;
    [Export] public NinePatchRect TestingSelection;
    public override void _Ready()
    {
        var buttonContainer = GetNode<VBoxContainer>("ButtonContainer");
        var testingButton = buttonContainer.GetNode<Button>("TestingButton");
        testingButton.Pressed += TestingButton_OnPressed;
        var playButton = buttonContainer.GetNode<Button>("PlayButton");
        playButton.Pressed += () => GetTree().ChangeSceneToPacked(PlayScene);
        var settingsButton = buttonContainer.GetNode<Button>("SettingsButton");
        var exitButton = buttonContainer.GetNode<Button>("ExitButton");
        exitButton.Pressed += () => GetTree().Quit();
        
        CreateTestingSelection();
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
    
    private void TestingButton_OnPressed()
    {
        if (!TestingSelection.Visible)
        {
            TestingSelection.Show();
        }
    }
    
    private void CreateTestingSelection()
    {
        foreach (var scene in TestingScenes)
        {
            
        }
    }
}
