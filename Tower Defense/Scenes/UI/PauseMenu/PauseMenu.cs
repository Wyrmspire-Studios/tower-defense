using Godot;

namespace WyrmspireStudios;

public partial class PauseMenu : Control
{
	[Export]
	public VBoxContainer ButtonContainer;

	[Export] public PackedScene MainMenuScene;
	[Export] public PackedScene SettingsScene;

	public override void _Ready()
	{
		Visible = false;
		var continueButton = ButtonContainer.GetNode<CustomTexturedButton>("ContinueButton");
		continueButton.Pressed += () =>
		{
			Visible = false;
			Engine.TimeScale = 1;
		};
		var settingsButton = ButtonContainer.GetNode<CustomTexturedButton>("SettingsButton");
		settingsButton.Pressed += () =>
		{
			GetTree().ChangeSceneToPacked(SettingsScene);
			Visible = false;
			Engine.TimeScale = 1;
		};
		var exitButton = ButtonContainer.GetNode<CustomTexturedButton>("ExitButton");
		exitButton.Pressed += () =>
		{
			GetTree().ChangeSceneToPacked(MainMenuScene);
			Visible = false;
			Engine.TimeScale = 1;
		};
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
