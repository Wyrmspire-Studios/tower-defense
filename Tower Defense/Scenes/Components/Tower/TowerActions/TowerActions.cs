using Godot;
using System;
using WyrmspireStudios;

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

		Tower.TowerCollider.InputEvent += _onColliderInputEvent;

		_sellTowerButton.Pressed += _onSellTower;
		_upgradeTowerButton.Pressed += _onUpgradeTower;

		_currentCost = 50; // TODO: CHANGE TO COST OF ROLLING A NEW CARD
		_upgradeCost = Tower.TowerInfo.BaseUpgradeCost;
		
		GameData.GoldChanged += _recheckUpgradeAvailable;
		_recheckUpgradeAvailable(0, GameData.GetGold());
	}

	public override void _ExitTree()
	{
		Tower.TowerCollider.InputEvent -= _onColliderInputEvent;
		_sellTowerButton.Pressed -= _onSellTower;
		_upgradeTowerButton.Pressed -= _onUpgradeTower;
		GameData.GoldChanged -= _recheckUpgradeAvailable;
	}

	public override void _Input(InputEvent ev)
	{
		if (!Visible) return;
		if (ev is not InputEventMouseButton { Pressed: true } mouseEvent) return;

		var colliderRect = Tower.TowerCollider.GetColliderRect();
		if (!GetGlobalRect().HasPoint(mouseEvent.GlobalPosition) && !colliderRect.HasPoint(mouseEvent.GlobalPosition))
		{
			ToggleVisibility();
		}
	}

	public void ToggleVisibility()
	{
		Visible = !Visible;
	}

	private void _onColliderInputEvent(Node viewport, InputEvent ev, long shapeId)
	{
		if (ev is not InputEventMouseButton { Pressed: true }) return;
		if (!Tower.TowerInfo.Active) return;
		ToggleVisibility();
	}

	private void _recheckUpgradeAvailable(int oldGold, int newGold)
	{
		GD.Print("Old:", oldGold, "New:", newGold);
		_canUpgrade = newGold >= _upgradeCost;
		GD.Print("Upgrade Cost:", _upgradeCost, "Can Upgrade:", _canUpgrade);
		_upgradeTowerButton.Disabled = !_canUpgrade;
	}

	private void _onSellTower()
	{
		GameData.AddGold(Mathf.FloorToInt(_currentCost * 0.5));
		Tower.OnSellTower();
	}

	private void _onUpgradeTower()
	{
		if (!_canUpgrade || Tower.TowerInfo.TowerTier == TowerTier.Four) return;
		
		Tower.TowerInfo.TowerTier = Tower.TowerInfo.TowerTier.Next();
		Tower.OnUpgradeTower(Tower.TowerInfo.TowerTier);
		
		GameData.RemoveGold(_upgradeCost);
		
		_currentCost += _upgradeCost;
		_upgradeCost = Mathf.FloorToInt(_upgradeCost * 1.5);
	}
}
