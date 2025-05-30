using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Node2D
{
	public delegate void TowerSoldHandler(Vector2I towerTile);
	public static event TowerSoldHandler TowerSold;
	
	public TowerInfo TowerInfo;
	public List<TowerEnhancement> TowerEnhancements = [];

	public Vector2I PlacedAt;
	public TowerSprite TowerSprite;
	public TowerCollider TowerCollider;
	public TowerActions TowerActions;

	public virtual void OnStartPlacing()
	{
		TowerSprite = GetNodeOrNull<TowerSprite>("TowerSprite");
		TowerCollider = GetNodeOrNull<TowerCollider>("TowerCollider");
		TowerActions = GetNodeOrNull<TowerActions>("TowerActions");
		
		TowerSprite?.Initialize(this);
		TowerCollider?.Initialize(this);
		TowerActions?.Initialize(this);
		
		TowerEnhancements.Add(TowerInfo.TowerTierEnhancements[TowerTier.One.ToIndex()]);
	}
	public virtual void OnPlaceTower() {}
	public virtual void OnUpgradeTower(TowerTier upgradedTo)
	{
		TowerSprite.UpgradeSpriteToTier(upgradedTo);
		TowerEnhancements.Add(TowerInfo.TowerTierEnhancements[upgradedTo.ToIndex()]);
		ApplyEnhancements();
	}
	public virtual void OnSellTower()
	{
		TowerSold?.Invoke(PlacedAt);
		
		TowerSprite.ShowSmoke();
		QueueFree();
	}
	public virtual void ApplyEnhancements() {}
}
