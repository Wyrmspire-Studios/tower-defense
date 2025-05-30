using Godot;
using System;
using WyrmspireStudios.Data;

public partial class StatusPanel : PanelContainer
{
    private Label _healthLabel;
    private Label _goldLabel;
    
    public override void _Ready()
    {
        _healthLabel = GetNode<Label>("MarginContainer/VBoxContainer/HealthLabel");
        _goldLabel = GetNode<Label>("MarginContainer/VBoxContainer/GoldLabel");
        UpdateHealthLabel(0, LevelData.GetHealth());
        UpdateGoldLabel(0, LevelData.GetGold());
        LevelData.HealthChanged += UpdateHealthLabel;
        LevelData.GoldChanged += UpdateGoldLabel;
    }

    public override void _ExitTree()
    {
        LevelData.HealthChanged -= UpdateHealthLabel;
        LevelData.GoldChanged -= UpdateGoldLabel;
    }

    private void UpdateHealthLabel(int oldValue, int newValue)
    {
        _healthLabel.Text = $"Health: {newValue}";
    }
    
    private void UpdateGoldLabel(int oldValue, int newValue)
    {
        _goldLabel.Text = $"Gold: {newValue}";
    }
}
