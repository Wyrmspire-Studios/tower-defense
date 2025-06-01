using Godot;
using System;
using WyrmspireStudios.Data;

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
        var root = GetTree().GetRoot();
        var tutorialMap = TutorialMap.Instantiate();
        _uiAnimationPlayer.Play("Hide");
        GetTree().CreateTimer(0.5).Timeout += () =>
        {
            LevelData.ResetLevelData();
            
            tutorialMap.GetNode<ColorRect>("MapUi/ColorRect").Visible = false;
            tutorialMap.GetNode<ColorRect>("MapUi/ColorRect").MouseFilter = MouseFilterEnum.Ignore;
            
            root.AddChild(tutorialMap);
            GetTree().CurrentScene = tutorialMap;
            root.RemoveChild(this);
        };
    }
    
    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}