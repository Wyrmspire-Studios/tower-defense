using Godot;

namespace WyrmspireStudios.UI;

public partial class Statistics : NinePatchRect
{
    [Export] public Button StatisticsMenuButton;

    private AnimationPlayer _animationPlayer;
    private bool _eventsConnected;
    private bool _statisticsHidden = true;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        StatisticsMenuButton.Pressed += OnButtonPressed;
        _eventsConnected = true;
    }


    public override void _ExitTree()
    {
        if (_eventsConnected) return;
        StatisticsMenuButton.Pressed -= OnButtonPressed;
        _eventsConnected = false;
    }

    private void OnButtonPressed()
    {
        _statisticsHidden = !_statisticsHidden;
        _animationPlayer.Play(_statisticsHidden ? "HideStatistics" : "ShowStatistics");
    }
}