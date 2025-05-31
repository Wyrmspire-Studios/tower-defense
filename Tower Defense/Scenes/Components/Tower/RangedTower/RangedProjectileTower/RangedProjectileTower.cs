using Godot;
using System;
using System.Linq;

public partial class RangedProjectileTower : RangedTower
{
	[Export] private RangedProjectileTowerInfo _baseRangedProjectileTowerInfo;
	public RangedProjectileTowerInfo RangedProjectileTowerInfo;
	
	public TowerProjectileShooting TowerProjectileShooting;
	
	public virtual void OnEnemyShot(Enemy enemy, Projectile projectile)
	{
		enemy.TakeDamage(projectile.ProjectileInfo.Damage);
		projectile.QueueFree();
	}

	private void _updateTowerInfo(RangedProjectileTowerInfo rangedProjectileTowerInfo)
	{
		RangedProjectileTowerInfo = rangedProjectileTowerInfo;
		RangedTowerInfo = RangedProjectileTowerInfo;
		TowerInfo = RangedProjectileTowerInfo;
	}

	public override void OnStartPlacing()
	{
		_updateTowerInfo((RangedProjectileTowerInfo)_baseRangedProjectileTowerInfo.Duplicate(true));
		
		base.OnStartPlacing();
		
		TowerProjectileShooting = GetNodeOrNull<TowerProjectileShooting>("TowerProjectileShooting");
		TowerProjectileShooting?.Initialize(this);
		
		RangedProjectileTowerInfo.Active = false;
		
		ApplyEnhancements();
	}

	public override void OnPlaceTower()
	{
		RangedProjectileTowerInfo.Active = true;
		base.OnPlaceTower();
	}

	public override void ApplyEnhancements()
	{
		var newTowerData = (RangedProjectileTowerInfo)_baseRangedProjectileTowerInfo.Duplicate(true);
		newTowerData.TowerTier = RangedProjectileTowerInfo.TowerTier;
		
		
		
		foreach (var towerEnhancement in TowerEnhancements.OrderBy(enhancement => enhancement.Additive))
		{
			if (towerEnhancement.NewProjectileDamage != -1)
				if (towerEnhancement.Additive) newTowerData.BaseProjectileInfo.Damage += towerEnhancement.NewProjectileDamage;
				else newTowerData.BaseProjectileInfo.Damage = towerEnhancement.NewProjectileDamage;
			
			if (towerEnhancement.NewProjectileSpeed != -1)
				if (towerEnhancement.Additive) newTowerData.BaseProjectileInfo.Speed += towerEnhancement.NewProjectileSpeed;
				else newTowerData.BaseProjectileInfo.Speed = towerEnhancement.NewProjectileSpeed;
			
			if (towerEnhancement.NewProjectileSpawnOffset != Vector2.Inf)
				if (towerEnhancement.Additive) newTowerData.ProjectileSpawnOffset += towerEnhancement.NewProjectileSpawnOffset;
				else newTowerData.ProjectileSpawnOffset = towerEnhancement.NewProjectileSpawnOffset;

			if (towerEnhancement.NewRange != -1)
			{
				if (towerEnhancement.Additive) newTowerData.Range += towerEnhancement.NewRange;
				else newTowerData.Range = towerEnhancement.NewRange;
				
				TowerTargeting.UpdateRange();
			}

			if (towerEnhancement.NewRangeOffset != Vector2.Inf)
			{
				if (towerEnhancement.Additive) newTowerData.RangeOffset += towerEnhancement.NewRangeOffset;
				else newTowerData.RangeOffset = towerEnhancement.NewRangeOffset;
				
				TowerTargeting.UpdateRange();
			}
		}
		
		_updateTowerInfo(newTowerData);
	}
}
