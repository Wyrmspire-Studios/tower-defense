using Godot;
using System;
using System.Collections.Generic;
using TowerDefense.Scenes.Components.Map.TowerPlacement;

public partial class Tower : Node2D
{
	public delegate void TowerSoldHandler(Vector2I towerTile);
	public static event TowerSoldHandler TowerSold;
	
	public TowerInfo TowerInfo;
	public List<TowerEnhancement> TowerEnhancements = [];

	public Vector2I PlacedAt;
	public TowerSprite TowerSprite;
	public TowerCollider TowerCollider;
	public TowerUI TowerUi;

	public virtual void OnStartPlacing(TowerPlacement towerPlacement, bool headless = false)
	{
		TowerSprite = GetNode<TowerSprite>("TowerSprite");
		TowerCollider = GetNode<TowerCollider>("TowerCollider");
		TowerUi = GetNode<TowerUI>("TowerUI");
		
		if (headless) return;
		
		TowerSprite.Initialize(this);
		TowerCollider.Initialize(this);
		TowerUi.Initialize(this);
		
		TowerEnhancements.Add(TowerInfo.TowerTierEnhancements[TowerTier.One.ToIndex()]);

		EnableStatsUi(TowerUi.TowerStats);
	}

	public virtual void OnPlaceTower()
	{
	}
	
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
	public virtual void EnableStatsUi(TowerStats towerStats) {}
	public virtual void UpdateStatsUi(TowerStats towerStats) {}

	public virtual CanPlace CheckPlacementRequirements(TowerPlacement towerPlacement, Vector2I tile)
	{
		return CanPlace.Yes;
	}
}
