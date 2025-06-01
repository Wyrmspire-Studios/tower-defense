using Godot;
using System;
using TowerDefense.Scenes.Components.Map.TowerPlacement;

public partial class SpawningTower : Tower
{
	[Export] private SpawningTowerInfo _baseSpawningTowerInfo;
	public SpawningTowerInfo SpawningTowerInfo;

	public TowerPlacement TowerPlacement;

	public TowerSpawning TowerSpawning;
	
	public virtual void OnEnemyHit(Enemy enemy, Unit unit)
	{
		var enemyHealth = enemy.GetHealth();
		var unitHealth = unit.Health;

		if (unitHealth > enemyHealth)
		{
			if (SpawningTowerInfo.UnitStunDuration > 0) enemy.ApplyStun(SpawningTowerInfo.UnitStunDuration);
			enemy.TakeDamage(enemyHealth);
			unit.Health -= enemyHealth;
		}
		else
		{
			if (SpawningTowerInfo.UnitStunDuration > 0) enemy.ApplyStun(SpawningTowerInfo.UnitStunDuration);
			enemy.TakeDamage(unit.Health);
			unit.QueueFree();
			TowerSpawning.SpawnedUnits.Remove(unit.Direction);
		}
	}

	public override void OnStartPlacing(TowerPlacement towerPlacement, bool headless = false)
	{
		_updateTowerInfo((SpawningTowerInfo) _baseSpawningTowerInfo.Duplicate(true));
		SpawningTowerInfo.Active = false;
		TowerPlacement = towerPlacement;
		
		base.OnStartPlacing(towerPlacement, headless);
		
		TowerSpawning = GetNode<TowerSpawning>("TowerSpawning");

		if (headless) return;
		
		TowerSpawning.Initialize(this);
		TowerSpawning.UpdateSpawnRate();
		
		CallDeferred(MethodName.ApplyEnhancements);
	}

	public override CanPlace CheckPlacementRequirements(TowerPlacement towerPlacement, Vector2I tile)
	{
		TowerPlacement = towerPlacement;
		
		var up = tile + Vector2I.Up;
		var down = tile + Vector2I.Down;
		var left = tile + Vector2I.Left;
		var right = tile + Vector2I.Right;

		return TowerPlacement.Paths.IsTileOccupied(up)
		       || TowerPlacement.Paths.IsTileOccupied(down)
		       || TowerPlacement.Paths.IsTileOccupied(left) 
		       || TowerPlacement.Paths.IsTileOccupied(right) ? CanPlace.Yes : CanPlace.No;
	}
	
	public override void ApplyEnhancements()
	{
		var newTowerData = (SpawningTowerInfo)_baseSpawningTowerInfo.Duplicate(true);
		newTowerData.TowerTier = SpawningTowerInfo.TowerTier;
		newTowerData.Active = SpawningTowerInfo.Active;
		
		foreach (var towerEnhancement in TowerEnhancements)
		{
			if (towerEnhancement.NewUnitHealth != -1)
				if (towerEnhancement.Additive) newTowerData.UnitHealth += towerEnhancement.NewUnitHealth;
				else newTowerData.UnitHealth = towerEnhancement.NewUnitHealth;
			
			if (towerEnhancement.NewUnitSpawnRate > 0)
				if (towerEnhancement.Additive) newTowerData.UnitSpawnRate -= towerEnhancement.NewUnitSpawnRate;
				else newTowerData.UnitSpawnRate = towerEnhancement.NewUnitSpawnRate;

			if (towerEnhancement.NewUnitStunDuration > 0)
				if (towerEnhancement.Additive) newTowerData.UnitStunDuration += towerEnhancement.NewUnitStunDuration;
				else newTowerData.UnitStunDuration = towerEnhancement.NewUnitStunDuration;
		}
		
		_updateTowerInfo(newTowerData);
		EnableStatsUi(TowerUi.TowerStats);
		UpdateStatsUi(TowerUi.TowerStats);
	}
	
	private void _updateTowerInfo(SpawningTowerInfo spawningTowerInfo)
	{
		SpawningTowerInfo = spawningTowerInfo;
		TowerInfo = SpawningTowerInfo;
		
		TowerSpawning?.UpdateSpawnRate();
	}

	public override void OnPlaceTower()
	{
		SpawningTowerInfo.Active = true;
		TowerSpawning.ToggleSpawnPointsVisible();
		TowerSpawning.CalculateSpawnPoints();
		
		base.OnPlaceTower();
	}

	public override void EnableStatsUi(TowerStats towerStats)
	{
		base.EnableStatsUi(towerStats);
		
		towerStats.EnableHealthStat();
		towerStats.EnableFireDelayStat();
		if (SpawningTowerInfo.UnitStunDuration > 0) towerStats.EnableStunStat();
	}

	public override void UpdateStatsUi(TowerStats towerStats)
	{
		towerStats.HealthStatContainer.SetStatText($"{SpawningTowerInfo.UnitHealth}");
		towerStats.FireDelayStatContainer.SetStatText($"{SpawningTowerInfo.UnitSpawnRate}");
		towerStats.StunStatContainer?.SetStatText($"{SpawningTowerInfo.UnitStunDuration}s");
	}
}
