using Godot;
using System;

public partial class Tower : Node2D
{
	public delegate void TowerSoldHandler(Vector2I towerTile);
	public static event TowerSoldHandler TowerSold;
	
	public TowerInfo TowerInfo;

	public Vector2I PlacedAt;
	public TowerSprite TowerSprite;
	public TowerCollider TowerCollider;
	public TowerActions TowerActions;
	
	public override void _Ready()
	{
		TowerSprite = GetNodeOrNull<TowerSprite>("TowerSprite");
		TowerCollider = GetNodeOrNull<TowerCollider>("TowerCollider");
		TowerActions = GetNodeOrNull<TowerActions>("TowerActions");
		
		TowerSprite?.Initialize(this);
		TowerCollider?.Initialize(this);
		TowerActions?.Initialize(this);
	}

	public virtual void OnStartPlacing() {}
	public virtual void OnPlaceTower() {}
	public virtual void OnUpgradeTower(TowerTier upgradedTo)
	{
		TowerSprite.UpgradeSpriteToTier(upgradedTo);
	}
	public virtual void OnSellTower()
	{
		TowerSold?.Invoke(PlacedAt);
		
		TowerSprite.ShowSmoke();
		QueueFree();
	}
}
