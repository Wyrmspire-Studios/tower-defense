using Godot;

namespace WyrmspireStudios;

public partial class PauseMenu : Control
{
    [Export] public VBoxContainer ButtonContainer;

    [Export] public PackedScene MainMenuScene;
    [Export] public PackedScene SettingsScene;

    public override void _Ready()
    {
        Visible = false;
        var continueButton = ButtonContainer.GetNode<CustomTexturedButton>("ContinueButton");
        continueButton.Pressed += ContinueButton_OnPressed;
        var settingsButton = ButtonContainer.GetNode<CustomTexturedButton>("SettingsButton");
        settingsButton.Pressed += SettingsButton_OnPressed;
        var exitButton = ButtonContainer.GetNode<CustomTexturedButton>("ExitButton");
        exitButton.Pressed += ExitButton_OnPressed;
    }

    private void ContinueButton_OnPressed()
    {
        Visible = false;
        Engine.TimeScale = 1;
    }

    private void SettingsButton_OnPressed()
    {
        Visible = false;
        Engine.TimeScale = 1;
        GetTree().ChangeSceneToPacked(SettingsScene);
    }

    private void ExitButton_OnPressed()
    {
        Visible = false;
        Engine.TimeScale = 1;
        GetTree().ChangeSceneToPacked(MainMenuScene);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true, Keycode: Key.Escape })
        {
            if (GetTree().GetCurrentScene().Name != "MainMenu")
            {
                Visible = !Visible;
                Engine.TimeScale = Visible ? 0 : 1;
            }
        }
    }
}