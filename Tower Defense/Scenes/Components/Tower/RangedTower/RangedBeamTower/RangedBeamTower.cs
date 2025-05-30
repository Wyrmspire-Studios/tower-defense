using Godot;
using System;
using System.Linq;

public partial class RangedBeamTower : RangedTower
{
	[Export] private RangedBeamTowerInfo _baseRangedBeamTowerInfo;
	public RangedBeamTowerInfo RangedBeamTowerInfo;

	public TowerBeamShooting TowerBeamShooting;

	public virtual void OnEnemyShot(Enemy enemy)
	{
		enemy.TakeDamage(RangedBeamTowerInfo.Damage);
	}

	private void _updateTowerInfo(RangedBeamTowerInfo rangedBeamTowerInfo)
	{
		RangedBeamTowerInfo = rangedBeamTowerInfo;
		RangedTowerInfo = RangedBeamTowerInfo;
		TowerInfo = RangedBeamTowerInfo;
	}

	public override void OnStartPlacing(bool headless = false)
	{
		_updateTowerInfo((RangedBeamTowerInfo) _baseRangedBeamTowerInfo.Duplicate(true));
		RangedBeamTowerInfo.Active = false;
		
		base.OnStartPlacing(headless);

		if (headless) return;

		TowerBeamShooting = GetNode<TowerBeamShooting>("TowerBeamShooting");
		TowerBeamShooting.Initialize(this);

		CallDeferred(MethodName.ApplyEnhancements);
	}

	public override void OnPlaceTower()
	{
		RangedBeamTowerInfo.Active = true;
		base.OnPlaceTower();
	}

	public override void ApplyEnhancements()
	{
		var newTowerData = (RangedBeamTowerInfo)_baseRangedBeamTowerInfo.Duplicate(true);
		newTowerData.TowerTier = RangedBeamTowerInfo.TowerTier;
		newTowerData.Active = true;
		
		foreach (var towerEnhancement in TowerEnhancements.OrderBy(enhancement => enhancement.Additive))
		{
			if (towerEnhancement.NewBeamDamage != -1)
				if (towerEnhancement.Additive) newTowerData.Damage += towerEnhancement.NewBeamDamage;
				else newTowerData.Damage = towerEnhancement.NewBeamDamage;
			
			if (towerEnhancement.NewBeamSpawnOffset != Vector2.Inf)
				if (towerEnhancement.Additive) newTowerData.BeamSpawnOffset += towerEnhancement.NewBeamSpawnOffset;
				else newTowerData.BeamSpawnOffset = towerEnhancement.NewBeamSpawnOffset;
			
			if (towerEnhancement.NewBeamFireDelay > 0)
			{
				if (towerEnhancement.Additive) newTowerData.FireDelay += towerEnhancement.NewBeamFireDelay;
				else newTowerData.FireDelay = towerEnhancement.NewBeamFireDelay;
				TowerBeamShooting.UpdateTimerDurations();
			}
			
			if (towerEnhancement.NewBeamFireDuration > 0)
			{
				if (towerEnhancement.Additive) newTowerData.FireDuration += towerEnhancement.NewBeamFireDuration;
				else newTowerData.FireDuration = towerEnhancement.NewBeamFireDuration;
				TowerBeamShooting.UpdateTimerDurations();
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
		towerStats.EnableFireDurationStat();
		towerStats.EnableRangeStat();
	}

	public override void UpdateStatsUi(TowerStats towerStats)
	{
		towerStats.DamageStatContainer.SetStatText($"{RangedBeamTowerInfo.Damage}");
		towerStats.FireDelayStatContainer.SetStatText($"{RangedBeamTowerInfo.FireDelay}s");
		towerStats.FireDurationStatContainer.SetStatText($"{RangedBeamTowerInfo.FireDuration}s");
		towerStats.RangeStatContainer.SetStatText($"{RangedBeamTowerInfo.Range}m");
	}
}
