using Godot;
using System;
using System.Linq;

public partial class RangedProjectileTower : RangedTower
{
	[Export] private RangedProjectileTowerInfo _baseRangedProjectileTowerInfo;
	public RangedProjectileTowerInfo RangedProjectileTowerInfo;
	
	public TowerProjectileShooting TowerProjectileShooting;
	
	public virtual void OnEnemyHit(Enemy enemy, Projectile projectile)
	{
		if (RangedProjectileTowerInfo.StunDuration > 0) enemy.ApplyStun(RangedProjectileTowerInfo.StunDuration);
		enemy.TakeDamage(projectile.ProjectileInfo.Damage);
		projectile.QueueFree();
	}

	private void _updateTowerInfo(RangedProjectileTowerInfo rangedProjectileTowerInfo)
	{
		RangedProjectileTowerInfo = rangedProjectileTowerInfo;
		RangedTowerInfo = RangedProjectileTowerInfo;
		TowerInfo = RangedProjectileTowerInfo;
		
		TowerProjectileShooting?.UpdateFireDelay();
		TowerTargeting?.UpdateRange();
	}

	public override void OnStartPlacing(TowerPlacement towerPlacement, bool headless = false) 
	{
		_updateTowerInfo((RangedProjectileTowerInfo)_baseRangedProjectileTowerInfo.Duplicate(true));
		RangedProjectileTowerInfo.Active = false;
		
		base.OnStartPlacing(towerPlacement, headless);
		
		if (headless) return;
		
		TowerProjectileShooting = GetNode<TowerProjectileShooting>("TowerProjectileShooting");
		TowerProjectileShooting.Initialize(this);

		CallDeferred(MethodName.ApplyEnhancements);
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
		newTowerData.Active = RangedProjectileTowerInfo.Active;
		
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
			
			if (towerEnhancement.NewProjectileFireDelay > 0)
				if (towerEnhancement.Additive) newTowerData.FireDelay += towerEnhancement.NewProjectileFireDelay;
				else newTowerData.FireDelay = towerEnhancement.NewProjectileFireDelay;

			if (towerEnhancement.NewRange != -1)
				if (towerEnhancement.Additive) newTowerData.Range += towerEnhancement.NewRange;
				else newTowerData.Range = towerEnhancement.NewRange;

			if (towerEnhancement.NewRangeOffset != Vector2.Inf)
				if (towerEnhancement.Additive) newTowerData.RangeOffset += towerEnhancement.NewRangeOffset;
				else newTowerData.RangeOffset = towerEnhancement.NewRangeOffset;

			if (towerEnhancement.NewProjectileStunDuration > 0)
				if (towerEnhancement.Additive) newTowerData.StunDuration += towerEnhancement.NewProjectileStunDuration;
				else newTowerData.StunDuration = towerEnhancement.NewProjectileStunDuration;
		}
		
		_updateTowerInfo(newTowerData);
		EnableStatsUi(TowerUi.TowerStats);
		UpdateStatsUi(TowerUi.TowerStats);
	}

	public override void EnableStatsUi(TowerStats towerStats)
	{
		base.EnableStatsUi(towerStats);
		
		towerStats.EnableDamageStat();
		towerStats.EnableFireDelayStat();
		towerStats.EnableProjectileSpeedStat();
		towerStats.EnableRangeStat();
		if (RangedProjectileTowerInfo.StunDuration > 0) towerStats.EnableStunStat();
	}

	public override void UpdateStatsUi(TowerStats towerStats)
	{
		towerStats.DamageStatContainer.SetStatText($"{RangedProjectileTowerInfo.BaseProjectileInfo.Damage}");
		towerStats.FireDelayStatContainer.SetStatText($"{RangedProjectileTowerInfo.FireDelay}s");
		towerStats.ProjectileSpeedStatContainer.SetStatText($"{RangedProjectileTowerInfo.BaseProjectileInfo.Speed}/s");
		towerStats.RangeStatContainer.SetStatText($"{RangedProjectileTowerInfo.Range}m");
		towerStats.StunStatContainer?.SetStatText($"{RangedProjectileTowerInfo.StunDuration}s");
	}
}
