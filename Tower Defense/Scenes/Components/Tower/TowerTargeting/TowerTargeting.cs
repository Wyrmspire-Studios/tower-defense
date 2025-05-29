using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TowerTargeting : Area2D
{
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

		Position = Tower.RangedTowerInfo.RangeOffset;
		
		_rangeShape.Radius = Tower.RangedTowerInfo.Range;
		_rangeCollisionShape.Shape = _rangeShape;

		AreaEntered += _onAreaEntered;
		AreaExited += _onAreaExited;
		_recalculateTargetTimer.Timeout += _recalculateTarget;
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
		if (_enemiesInRange.Count == 0) _currentTarget = null;
		else
			_currentTarget = Tower.RangedTowerInfo.TargetingMode switch
			{
				TowerTargetingMode.First => _enemiesInRange.OrderBy(enemy => enemy.ProgressRatio).Last(),
				TowerTargetingMode.Last => _enemiesInRange.OrderBy(enemy => enemy.ProgressRatio).First(),
				TowerTargetingMode.MostHealth => _enemiesInRange.OrderBy(enemy => enemy.GetHealth()).Last(),
				TowerTargetingMode.LeastHealth => _enemiesInRange.OrderBy(enemy => enemy.GetHealth()).First(),
				_ => throw new ArgumentOutOfRangeException()
			};
	}
}
