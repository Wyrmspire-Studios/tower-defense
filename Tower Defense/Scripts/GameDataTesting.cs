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
		var addHealthButton = GetNode<Button>("AddHealthButton");
		addHealthButton.Pressed += AddHealthButton_OnPressed;
		var removeHealthButton = GetNode<Button>("RemoveHealthButton");
		removeHealthButton.Pressed += RemoveHealthButton_OnPressed;
		
		_goldLabel = GetNode<Label>("GoldLabel");
		var addGoldButton = GetNode<Button>("AddGoldButton");
		addGoldButton.Pressed += AddGoldButton_OnPressed;
		var removeGoldButton = GetNode<Button>("RemoveGoldButton");
		removeGoldButton.Pressed += RemoveGoldButton_OnPressed;
		
		_shardsLabel = GetNode<Label>("ShardsLabel");
		var addShardsButton = GetNode<Button>("AddShardsButton");
		addShardsButton.Pressed += AddShardsButton_OnPressed;
		var removeShardsButton = GetNode<Button>("RemoveShardsButton");
		removeShardsButton.Pressed += RemoveShardsButton_OnPressed;
	}

	private void AddHealthButton_OnPressed()
	{
		GameData.AddHealth(1);
		_healthLabel.Text = $"Health: {GameData.Health}";
	}
	
	private void RemoveHealthButton_OnPressed()
	{
		GameData.RemoveHealth(1);
		_healthLabel.Text = $"Health: {GameData.Health}";
	}
	
	private void AddGoldButton_OnPressed()
	{
		GameData.AddGold(1);
		_goldLabel.Text = $"Gold: {GameData.Gold}";
	}
	
	private void RemoveGoldButton_OnPressed()
	{
		GameData.RemoveGold(1);
		_goldLabel.Text = $"Gold: {GameData.Gold}";
	}
	
	private void AddShardsButton_OnPressed()
	{
		GameData.AddShards(1);
		_shardsLabel.Text = $"Shards: {GameData.Shards}";
	}

	private void RemoveShardsButton_OnPressed()
	{
		GameData.RemoveShards(1);
		_shardsLabel.Text = $"Shards: {GameData.Shards}";
	}
}
