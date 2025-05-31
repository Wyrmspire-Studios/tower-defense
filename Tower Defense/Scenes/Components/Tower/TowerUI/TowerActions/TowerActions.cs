using Godot;
using System;
using WyrmspireStudios;
using WyrmspireStudios.Data;

public partial class TowerActions : NinePatchRect
{
	public Tower Tower;
	
	[ExportGroup("Internal")]
	[Export] private Label _sellLabel;
	[Export] private Label _upgradeLabel;
	[Export] private TextureButton _sellTowerButton;
	[Export] private TextureButton _upgradeTowerButton;

	private int _currentCost;
	private int _upgradeCost;
	private bool _canUpgrade;
	
	private static readonly Color UpgradeableTint = Color.Color8(128, 255, 128);
	private static readonly Color NotUpgradeableTint = Color.Color8(255, 128, 128);
	
	public void Initialize(Tower tower)
	{
		Tower = tower;

		_sellTowerButton.Pressed += _onSellTower;
		_upgradeTowerButton.Pressed += _onUpgradeTower;

		_currentCost = 50;
		_upgradeCost = Tower.TowerInfo.BaseUpgradeCost;
		
		LevelData.GoldChanged += _recheckUpgradeAvailable;
		_recheckUpgradeAvailable(0, LevelData.GetGold());
		_updateLabels();
	}

	public override void _ExitTree()
	{
		_sellTowerButton.Pressed -= _onSellTower;
		_upgradeTowerButton.Pressed -= _onUpgradeTower;
		LevelData.GoldChanged -= _recheckUpgradeAvailable;
	}

	private void _recheckUpgradeAvailable(int oldGold, int newGold)
	{
		_canUpgrade = Tower.TowerInfo.TowerTier != TowerTier.Four && newGold >= _upgradeCost;
		_upgradeTowerButton.Disabled = !_canUpgrade;
		_updateLabels();
	}

	private void _onSellTower()
	{
		LevelData.AddGold(Mathf.FloorToInt(_currentCost * 0.5));
		Tower.OnSellTower();
	}

	private void _onUpgradeTower()
	{
		_recheckUpgradeAvailable(LevelData.GetGold(), LevelData.GetGold());
		
		if (!_canUpgrade || Tower.TowerInfo.TowerTier == TowerTier.Four) return;
		
		Tower.TowerInfo.TowerTier = Tower.TowerInfo.TowerTier.Next();
		Tower.OnUpgradeTower(Tower.TowerInfo.TowerTier);
		
		LevelData.RemoveGold(_upgradeCost);
		
		_currentCost += _upgradeCost;
		_upgradeCost = Mathf.FloorToInt(_upgradeCost * 1.5);
		_updateLabels();
	}

	public void IncreaseCurrentCost(int increaseBy)
	{
		_currentCost += increaseBy;
		_updateLabels();
	}

	public void _updateLabels()
	{
		if (Tower.TowerInfo.TowerTier == TowerTier.Four) _upgradeLabel.Visible = false;
		
		_sellLabel.Text = $"${Mathf.Floor(_currentCost * 0.5)}";
		_upgradeLabel.Text = $"${_upgradeCost}";
		_upgradeLabel.Modulate = _canUpgrade ? UpgradeableTint : NotUpgradeableTint; 
	}
}
