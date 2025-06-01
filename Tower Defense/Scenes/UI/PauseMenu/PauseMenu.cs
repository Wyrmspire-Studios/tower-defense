using Godot;
using WyrmspireStudios.Data;

namespace WyrmspireStudios.UI;

public partial class PauseMenu : Control
{
	[Export]
	public VBoxContainer ButtonContainer;

	[Export] public PackedScene MainMenuScene;
	[Export] public PackedScene SettingsScene;
	
	private bool _eventsConnected;
	private Button _continueButton;
	private Button _statisticsButton;
	private Button _exitButton;
	public override void _Ready()
	{
		Visible = false;
		_continueButton = ButtonContainer.GetNode<Button>("ContinueMenuButton");
		_statisticsButton = ButtonContainer.GetNode<Button>("StatisticsMenuButton");
		_exitButton = ButtonContainer.GetNode<Button>("ExitMenuButton");
		
		if (_eventsConnected) return;
		_continueButton.Pressed += ContinueButtonPressed;
		_statisticsButton.Pressed += StatisticsButtonPressed;
		_exitButton.Pressed += ExitButtonPressed;
		_eventsConnected = true;
	}

	public override void _ExitTree()
	{
		if (!_eventsConnected) return;
		_continueButton.Pressed -= ContinueButtonPressed;
		_statisticsButton.Pressed -= StatisticsButtonPressed;
		_exitButton.Pressed -= ExitButtonPressed;
		_eventsConnected = false;
	}

	private void ContinueButtonPressed()
	{
		Visible = false;
		Engine.TimeScale = 1;
	}

	private void StatisticsButtonPressed()
	{
		Visible = false;
		Engine.TimeScale = 1;
	}

	private void ExitButtonPressed()
	{
		Visible = false;
		Engine.TimeScale = 1;
		GetTree().ChangeSceneToPacked(MainMenuScene);
		MapPicker.Reset();
		LevelData.ResetLevelData();
		GameData.ResetGameData();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey { Pressed: true, Keycode: Key.Escape })
		{
			if (!MapUi.Transitioning && GetTree().GetCurrentScene().Name != "MainMenu")
			{
				Visible = !Visible;
				Engine.TimeScale = Visible ? 0 : 1;
			}
		}
	}
}
