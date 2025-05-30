using Godot;
using System;
using WyrmspireStudios;
using WyrmspireStudios.Data;

public partial class TowerActions : NinePatchRect
{
	public Tower Tower;
	
	[ExportGroup("Internal")]
	[Export] private TextureButton _sellTowerButton;
	[Export] private TextureButton _upgradeTowerButton;

	private int _currentCost;
	private int _upgradeCost;
	private bool _canUpgrade;
	
	public void Initialize(Tower tower)
	{
		Tower = tower;

		Tower.TowerCollider.TowerClickedInside += _onTowerClickedInside;
		Tower.TowerCollider.TowerClickedOutside += _onTowerClickedOutside;

		_sellTowerButton.Pressed += _onSellTower;
		_upgradeTowerButton.Pressed += _onUpgradeTower;

		_currentCost = 50; // TODO: CHANGE TO COST OF ROLLING A NEW CARD
		_upgradeCost = Tower.TowerInfo.BaseUpgradeCost;
		
		LevelData.GoldChanged += _recheckUpgradeAvailable;
		_recheckUpgradeAvailable(0, LevelData.GetGold());
	}

	public override void _ExitTree()
	{
		Tower.TowerCollider.TowerClickedInside -= _onTowerClickedInside;
		Tower.TowerCollider.TowerClickedOutside -= _onTowerClickedOutside;
		_sellTowerButton.Pressed -= _onSellTower;
		_upgradeTowerButton.Pressed -= _onUpgradeTower;
		LevelData.GoldChanged -= _recheckUpgradeAvailable;
	}

	public void ToggleVisibility()
	{
		Visible = !Visible;
	}

	private void _onTowerClickedInside(InputEventMouseButton ev)
	{
		ToggleVisibility();
	}

	private void _onTowerClickedOutside(InputEventMouseButton ev)
	{
		if (!Visible) return;
		if (GetGlobalRect().HasPoint(ev.GlobalPosition)) return;
		ToggleVisibility();
	}

	private void _recheckUpgradeAvailable(int oldGold, int newGold)
	{
		_canUpgrade = Tower.TowerInfo.TowerTier != TowerTier.Four && newGold >= _upgradeCost;
		_upgradeTowerButton.Disabled = !_canUpgrade;
	}

	private void _onSellTower()
	{
		LevelData.AddGold(Mathf.FloorToInt(_currentCost * 0.5));
		Tower.OnSellTower();
	}

	private void _onUpgradeTower()
	{
		if (!_canUpgrade || Tower.TowerInfo.TowerTier == TowerTier.Four) return;
		
		Tower.TowerInfo.TowerTier = Tower.TowerInfo.TowerTier.Next();
		Tower.OnUpgradeTower(Tower.TowerInfo.TowerTier);
		
		LevelData.RemoveGold(_upgradeCost);
		
		_currentCost += _upgradeCost;
		_upgradeCost = Mathf.FloorToInt(_upgradeCost * 1.5);
	}
}
