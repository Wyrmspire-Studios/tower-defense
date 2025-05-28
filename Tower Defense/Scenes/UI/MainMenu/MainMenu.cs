using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] public PackedScene TutorialMap;
    [Export] public Container DefaultButtonSelection;

    private bool _eventsConnected;
    private AnimationPlayer _uiAnimationPlayer;
    private Button _playButton;
    private Button _exitButton;

    public override void _Ready()
    {
        _uiAnimationPlayer = GetNode<AnimationPlayer>("CanvasLayer/UIAnimationPlayer");
        _playButton = DefaultButtonSelection.GetNode<Button>("PlayMenuButton");
        _exitButton = DefaultButtonSelection.GetNode<Button>("ExitMenuButton");
        
        if (_eventsConnected) return;
        _playButton.Pressed += OnPlayButtonPressed;
        _exitButton.Pressed += OnExitButtonPressed;
        _eventsConnected = true;
    }

    public override void _ExitTree()
    {
        if (!_eventsConnected) return;
        _playButton.Pressed -= OnPlayButtonPressed;
        _exitButton.Pressed -= OnExitButtonPressed;
        _eventsConnected = false;
    }

    private void OnPlayButtonPressed()
    {
        _uiAnimationPlayer.Play("Hide");
        GetTree().CreateTimer(0.5).Timeout += () =>
        {
            GetTree().ChangeSceneToPacked(TutorialMap);
        };
    }
    
    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}