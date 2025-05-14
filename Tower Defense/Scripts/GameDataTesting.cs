using Godot;
using System;
using WyrmspireStudios;

public partial class GameDataTesting : GridContainer
{
	private Label _healthLabel;
	private Label _goldLabel;
	private Label _shardsLabel;
	public override void _Ready()
	{
		_healthLabel = GetNode<Label>("HealthLabel");
		GameData.HealthChanged += UpdateHealthLabel;
		_goldLabel = GetNode<Label>("GoldLabel");
		GameData.GoldChanged += UpdateGoldLabel;
		_shardsLabel = GetNode<Label>("ShardsLabel");
		GameData.ShardsChanged += UpdateShardsLabel;
		
		var addHealthButton = GetNode<Button>("AddHealthButton");
		addHealthButton.Pressed += () => GameData.AddHealth(1);
		var removeHealthButton = GetNode<Button>("RemoveHealthButton");
		removeHealthButton.Pressed += () => GameData.RemoveHealth(1);
		var resetHealthButton = GetNode<Button>("ResetHealthButton");
		resetHealthButton.Pressed += () => GameData.SetHealth(100);

		var addGoldButton = GetNode<Button>("AddGoldButton");
		addGoldButton.Pressed += () => GameData.AddGold(1);
		var removeGoldButton = GetNode<Button>("RemoveGoldButton");
		removeGoldButton.Pressed += () => GameData.RemoveGold(1);
		var resetGoldButton = GetNode<Button>("ResetGoldButton");
		resetGoldButton.Pressed += () => GameData.SetGold(0);

		var addShardsButton = GetNode<Button>("AddShardsButton");
		addShardsButton.Pressed += () => GameData.AddShards(1);
		var removeShardsButton = GetNode<Button>("RemoveShardsButton");
		removeShardsButton.Pressed += () => GameData.RemoveShards(1);
		var resetShardsButton = GetNode<Button>("ResetShardsButton");
		resetShardsButton.Pressed += () => GameData.SetShards(0);
	}

	private void UpdateHealthLabel(int oldValue, int newValue)
	{
		_healthLabel.Text = $"Health: {newValue}";
	}
	
	private void UpdateGoldLabel(int oldValue, int newValue)
	{
		_goldLabel.Text = $"Gold: {newValue}";
	}
	
	private void UpdateShardsLabel(int oldValue, int newValue)
	{
		_shardsLabel.Text = $"Shards: {newValue}";
	}
	
}
