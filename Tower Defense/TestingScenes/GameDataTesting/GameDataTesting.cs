using Godot;

using WyrmspireStudios.Data;

public partial class GameDataTesting : GridContainer
{
	private Label _healthLabel;
	private Label _goldLabel;
	private Label _shardsLabel;
	public override void _Ready()
	{
		_healthLabel = GetNode<Label>("HealthLabel");
		LevelData.HealthChanged += UpdateHealthLabel;
		_goldLabel = GetNode<Label>("GoldLabel");
		LevelData.GoldChanged += UpdateGoldLabel;
		_shardsLabel = GetNode<Label>("ShardsLabel");
		GameData.ShardsChanged += UpdateShardsLabel;
		
		var addHealthButton = GetNode<Button>("AddHealthButton");
		addHealthButton.Pressed += AddHealthButton;
		var removeHealthButton = GetNode<Button>("RemoveHealthButton");
		removeHealthButton.Pressed += RemoveHealthButton;
		var resetHealthButton = GetNode<Button>("ResetHealthButton");
		resetHealthButton.Pressed += ResetHealthButton;
		
		var addGoldButton = GetNode<Button>("AddGoldButton");
		addGoldButton.Pressed += AddGoldButton;
		var removeGoldButton = GetNode<Button>("RemoveGoldButton");
		removeGoldButton.Pressed += RemoveGoldButton;
		var resetGoldButton = GetNode<Button>("ResetGoldButton");
		resetGoldButton.Pressed += ResetGoldButton;
		
		var addShardsButton = GetNode<Button>("AddShardsButton");
		addShardsButton.Pressed += AddShardsButton;
		var removeShardsButton = GetNode<Button>("RemoveShardsButton");
		removeShardsButton.Pressed += RemoveShardsButton;
		var resetShardsButton = GetNode<Button>("ResetShardsButton");
		resetShardsButton.Pressed += ResetShardsButton;
	}

	private static void AddHealthButton()
	{
		LevelData.AddHealth(1);
	}
	
	private static void RemoveHealthButton()
	{
		LevelData.RemoveHealth(1);
	}
	
	private static void ResetHealthButton()
	{
		LevelData.SetHealth(0);
	}
	
	private static void AddGoldButton()
	{
		LevelData.AddGold(1);
	}
	
	private static void RemoveGoldButton()
	{
		LevelData.RemoveGold(1);
	}
	
	private static void ResetGoldButton()
	{
		LevelData.SetGold(100);
	}
	
	private static void AddShardsButton()
	{
		GameData.AddShards(1);
	}
	
	private static void RemoveShardsButton()
	{
		GameData.RemoveShards(1);
	}
	
	private static void ResetShardsButton()
	{
		GameData.SetShards(0);
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
