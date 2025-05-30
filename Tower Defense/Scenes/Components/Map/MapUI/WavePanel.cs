using Godot;
using System;
using WyrmspireStudios.Data;

public partial class WavePanel : PanelContainer
{
    private Label _waveLabel;

    public override void _Ready()
    {
        _waveLabel = GetNode<Label>("MarginContainer/WaveLabel");
        UpdateWaveLabel(0, 0);
        LevelData.CurrentWaveChanged += UpdateWaveLabel;
        LevelData.MaxWaveChanged += UpdateWaveLabel;
    }

    public override void _ExitTree()
    {
        LevelData.CurrentWaveChanged -= UpdateWaveLabel;
        LevelData.MaxWaveChanged -= UpdateWaveLabel;
    }

    private void UpdateWaveLabel(int oldValue, int newValue)
    {
        _waveLabel.Text = $"Wave: {LevelData.GetCurrentWave() + 1} / {LevelData.GetMaxWave()}";
    }
}
