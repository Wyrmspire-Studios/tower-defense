using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TowerTargeting : Area2D
{
	public delegate void OnTargetChange(Enemy enemy);
	public event OnTargetChange TargetChange;
	
	[ExportGroup("Internal")]
	[Export] private CollisionShape2D _rangeCollisionShape;
	[Export] private Timer _recalculateTargetTimer;
	
	public RangedTower Tower;

	private bool _rangeVisible = true;
	private CircleShape2D _rangeShape = new();
	private List<Enemy> _enemiesInRange = [];
	private Enemy _currentTarget;
	
	public void Initialize(RangedTower tower)
	{
		Tower = tower;

		_rangeCollisionShape.Shape = _rangeShape;
		UpdateRange();

		Tower.TowerCollider.TowerClickedInside += _onTowerClickedInside;
		Tower.TowerCollider.TowerClickedOutside += _onTowerClickedOutside;
		
		AreaEntered += _onAreaEntered;
		AreaExited += _onAreaExited;
		_recalculateTargetTimer.Timeout += _recalculateTarget;

		Enemy.EnemyDied += _onEnemyDied;
	}

	public override void _ExitTree()
	{
		Tower.TowerCollider.TowerClickedInside -= _onTowerClickedInside;
		Tower.TowerCollider.TowerClickedOutside -= _onTowerClickedOutside;
		AreaEntered -= _onAreaEntered;
		AreaExited -= _onAreaExited;
		_recalculateTargetTimer.Timeout -= _recalculateTarget;
		Enemy.EnemyDied -= _onEnemyDied;
	}

	public override void _Draw()
	{
		if (!_rangeVisible) return;
		DrawCircle(Vector2.Zero, Tower.RangedTowerInfo.Range + 2, Color.Color8(128, 128, 255, 192), false, 4);
		DrawCircle(Vector2.Zero, Tower.RangedTowerInfo.Range, Color.Color8(128, 128, 255, 128));
	}

	public Enemy GetCurrentTarget() => _currentTarget;

	public void ToggleRangeVisible()
	{
		_rangeVisible = !_rangeVisible;
		QueueRedraw();
	}

	public void UpdateRange()
	{
		_rangeShape.Radius = Tower.RangedTowerInfo.Range;
		Position = Tower.RangedTowerInfo.RangeOffset;
		QueueRedraw();
	}

	private void _onAreaEntered(Area2D other)
	{
		var otherParent = other.GetParent();
		if (otherParent is Enemy enemy) _enemiesInRange.Add(enemy);
	}

	private void _onAreaExited(Area2D other)
	{
		var otherParent = other.GetParent();
		if (otherParent is Enemy enemy) _enemiesInRange.Remove(enemy);
	}

	private void _recalculateTarget()
	{
		if (_enemiesInRange.Count == 0)
		{
			_currentTarget = null;
			TargetChange?.Invoke(_currentTarget);
		}
		else
		{
			var newTarget = Tower.RangedTowerInfo.TargetingMode switch
			{
				TowerTargetingMode.First => _enemiesInRange.OrderBy(enemy => enemy.ProgressRatio).Last(),
				TowerTargetingMode.Last => _enemiesInRange.OrderBy(enemy => enemy.ProgressRatio).First(),
				TowerTargetingMode.MostHealth => _enemiesInRange.OrderBy(enemy => enemy.GetHealth()).Last(),
				TowerTargetingMode.LeastHealth => _enemiesInRange.OrderBy(enemy => enemy.GetHealth()).First(),
				_ => throw new ArgumentOutOfRangeException()
			};

			if (newTarget != _currentTarget) TargetChange?.Invoke(newTarget);
			_currentTarget = newTarget;
		}
	}

	private void _onTowerClickedInside(InputEventMouseButton ev)
	{
		ToggleRangeVisible();
	}

	private void _onTowerClickedOutside(InputEventMouseButton ev)
	{
		if (!_rangeVisible) return;
		if (Tower.TowerUi.TowerActions.GetGlobalRect().HasPoint(ev.GlobalPosition)) return;
		ToggleRangeVisible();
	}

	private void _onEnemyDied(Enemy enemy)
	{
		if (!_enemiesInRange.Contains(enemy)) return;
		
		_enemiesInRange.Remove(enemy);
		_recalculateTarget();
	}
}
