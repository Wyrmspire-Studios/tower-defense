using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TowerSpawning : Node2D
{
	[Export] private Timer _spawnUnitTimer;
	
	public SpawningTower Tower;

	private bool _spawnPointsVisible = true;
	private Dictionary<Direction, bool> _spawnPointEnabled = new();
	public static Dictionary<Direction, Unit> SpawnedUnits = new();

	public void Initialize(SpawningTower tower)
	{
		Tower = tower;
		
		Tower.TowerCollider.TowerClickedInside += _onTowerClickedInside;
		Tower.TowerCollider.TowerClickedOutside += _onTowerClickedOutside;

		_spawnUnitTimer.Timeout += _spawnUnit;
	}

	public void UpdateSpawnRate()
	{
		_spawnUnitTimer.WaitTime = Tower.SpawningTowerInfo.UnitSpawnRate;
	}
	
	public override void _Draw()
	{
		if (!_spawnPointsVisible) return;
	
		var mouseTile = Tower.SpawningTowerInfo.Active ? _getPlacedOnTile() : Tower.TowerPlacement.GetMouseTile();
		
		var up = mouseTile + Vector2I.Up;
		var down = mouseTile + Vector2I.Down;
		var left = mouseTile + Vector2I.Left;
		var right = mouseTile + Vector2I.Right;
		
		if (Tower.TowerPlacement.Paths.IsTileOccupied(up))
			DrawCircle(up * 32 - GlobalPosition + new Vector2(16, 16), 8, Color.Color8(128, 128, 255, 128));
		
		if (Tower.TowerPlacement.Paths.IsTileOccupied(down))
			DrawCircle(down * 32 - GlobalPosition + new Vector2(16, 16), 8, Color.Color8(128, 128, 255, 128));
		
		if (Tower.TowerPlacement.Paths.IsTileOccupied(left))
			DrawCircle(left * 32 - GlobalPosition + new Vector2(16, 16), 8, Color.Color8(128, 128, 255, 128));
		
		if (Tower.TowerPlacement.Paths.IsTileOccupied(right))
			DrawCircle(right * 32 - GlobalPosition + new Vector2(16, 16), 8, Color.Color8(128, 128, 255, 128));
	}

	private Vector2I _getPlacedOnTile()
	{
		return (Vector2I)(GlobalPosition / 32).Floor();
	}
	
	public void ToggleSpawnPointsVisible()
	{
		_spawnPointsVisible = !_spawnPointsVisible;
	}

	public void CalculateSpawnPoints()
	{
		var placedOnTile = _getPlacedOnTile();
		
		var up = placedOnTile + Vector2I.Up;
		var down = placedOnTile + Vector2I.Down;
		var left = placedOnTile + Vector2I.Left;
		var right = placedOnTile + Vector2I.Right;
		
		_spawnPointEnabled.Add(Direction.Up, Tower.TowerPlacement.Paths.IsTileOccupied(up));
		_spawnPointEnabled.Add(Direction.Down, Tower.TowerPlacement.Paths.IsTileOccupied(down));
		_spawnPointEnabled.Add(Direction.Left, Tower.TowerPlacement.Paths.IsTileOccupied(left));
		_spawnPointEnabled.Add(Direction.Right, Tower.TowerPlacement.Paths.IsTileOccupied(right));
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	
	private void _onTowerClickedInside(InputEventMouseButton ev)
	{
		ToggleSpawnPointsVisible();
	}

	private void _onTowerClickedOutside(InputEventMouseButton ev)
	{
		if (!_spawnPointsVisible) return;
		if (Tower.TowerUi.TowerActions.GetGlobalRect().HasPoint(ev.GlobalPosition)) return;
		if (Tower.TowerUi.TowerStats.GetGlobalRect().HasPoint(ev.GlobalPosition)) return;
		ToggleSpawnPointsVisible();
	}

	private void _spawnUnit()
	{
		if (!Tower.SpawningTowerInfo.Active) return;
		
		var enabledSpawnPoints = _spawnPointEnabled.Where(kvp => kvp.Value).ToList();
		
		var emptySpawnPoints = enabledSpawnPoints.Where(kvp => !SpawnedUnits.ContainsKey(kvp.Key)).Select(kvp => kvp.Key).ToList();
		if (emptySpawnPoints.Count == 0) return;
		
		var emptySpawnPoint = emptySpawnPoints[Random.Shared.Next(0, emptySpawnPoints.Count)];
		var spawnAt = (_getPlacedOnTile() + emptySpawnPoint.ToVector2I()) * 32 + new Vector2I(16, 16);

		var unit = Tower.SpawningTowerInfo.SpawnedUnit.Instantiate<Unit>();
		unit.GlobalPosition = spawnAt - GlobalPosition;
		unit.Health = Tower.SpawningTowerInfo.UnitHealth;
		unit.Direction = emptySpawnPoint;
		unit.SpawnedBy = Tower;
		SpawnedUnits.Add(emptySpawnPoint, unit);
		AddChild(unit);
	}
}
