using Godot;
using WyrmspireStudios.Data;

namespace WyrmspireStudios.UI;

[Tool]
public partial class UpgradeButton : MenuButton
{
	[Export] public string UpgradeId = "upgrade_default";
	[Export] public Label PriceLabel;
	[Export] public Label ModifierLabel;
	
	private int _defaultShardPrice = 5;
	private int _shardPrice;
	
	[Export]
	public int ShardPrice
	{
		get => _shardPrice;
		set => UpdateDefaultShardPrice(value);
	}

	[Export] public float ModifierUpgrade;

	private void UpdateDefaultShardPrice(int value)
	{
		_defaultShardPrice = value;
		_shardPrice = value;
		if (PriceLabel != null) PriceLabel.Text = $"{_shardPrice} Shards";
	}
	private void UpdateShardPrice(int value)
	{
		_shardPrice = value;
		if (PriceLabel != null) PriceLabel.Text = $"{_shardPrice} Shards";
	}

	public override void _Ready()
	{
		ModifierLabel.Text = UpgradeId != "starting_cards" ? $"{GameData.GetModifier(UpgradeId) * 100}%" : $"{(int)GameData.GetModifier(UpgradeId)} cards";
		UpdateShardPrice(_defaultShardPrice * (GameData.GetUpgradeBoughtTimes(UpgradeId) + 1));
		Pressed += OnUpgradeButtonPressed;
		GameData.UpgradesChanged += OnUpgradesChanged;
	}

	public override void _ExitTree()
	{
		Pressed -= OnUpgradeButtonPressed;
		GameData.UpgradesChanged -= OnUpgradesChanged;
	}

	private void OnUpgradeButtonPressed()
	{
		if (GameData.GetShards() >= _shardPrice)
		{
			GameData.RemoveShards(_shardPrice);
			GameData.BoughtUpgrade(UpgradeId, ModifierUpgrade);
		}
	}

	private void OnUpgradesChanged(string upgradeId, int level, float modifier)
	{
		if (upgradeId != UpgradeId) return;
		UpdateShardPrice(_defaultShardPrice * (level + 1));
		ModifierLabel.Text = UpgradeId != "starting_cards" ? $"{GameData.GetModifier(UpgradeId) * 100}%" : $"{(int)GameData.GetModifier(UpgradeId)} cards";
	}
}
