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
		RangedProjectileTowerInfo.Active = false;
		
		base.OnStartPlacing();
		
		TowerProjectileShooting = GetNode<TowerProjectileShooting>("TowerProjectileShooting");
		TowerProjectileShooting.Initialize(this);
		
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
		newTowerData.Active = true;
		
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
			
			if (Math.Abs(towerEnhancement.NewProjectileFireDelay - -1) > 0.001)
			{
				if (towerEnhancement.Additive) newTowerData.FireDelay += towerEnhancement.NewProjectileFireDelay;
				else newTowerData.FireDelay = towerEnhancement.NewProjectileFireDelay;
				
				TowerProjectileShooting.UpdateFireDelay();
			}

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
		UpdateStatsUi(TowerUi.TowerStats);
	}

	public override void EnableStatsUi(TowerStats towerStats)
	{
		towerStats.EnableDamageStat();
		towerStats.EnableFireDelayStat();
		towerStats.EnableProjectileSpeedStat();
		towerStats.EnableRangeStat();
	}

	public override void UpdateStatsUi(TowerStats towerStats)
	{
		towerStats.DamageStatContainer.SetStatText($"{RangedProjectileTowerInfo.BaseProjectileInfo.Damage}");
		towerStats.FireDelayStatContainer.SetStatText($"{RangedProjectileTowerInfo.FireDelay}s");
		towerStats.ProjectileSpeedStatContainer.SetStatText($"{RangedProjectileTowerInfo.BaseProjectileInfo.Speed}/s");
		towerStats.RangeStatContainer.SetStatText($"{RangedProjectileTowerInfo.Range}m");
	}
}
