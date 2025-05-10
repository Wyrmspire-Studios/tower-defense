using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WyrmspireStudios;
public partial class TowerShoot : Node2D
{
	[ExportGroup("Shooting")]
	[Export] private float _shootInterval;
	
	[ExportGroup("Projectile")]
	[Export] private PackedScene _projectileScene;
	
	[ExportGroup("Internal")]
	[Export] private Timer _recalculateTargetTimer;
	[Export] private Timer _shootTargetTimer;
	
	private TowerTargeting _towerTargeting = TowerTargeting.First;
	private readonly List<TowerPlaceholderEnemy> _enemiesInRange = [];
	private TowerPlaceholderEnemy _currentTarget;
	
	private Tower _tower;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetParent<Tower>();
		_tower.TowerShoot = this;

		_shootTargetTimer.WaitTime = _shootInterval;

		_tower.TowerRange.EnemyEnteredRange += OnEnemyEnteredRange;
		_tower.TowerRange.EnemyExitedRange += OnEnemyExitedRange;

		_recalculateTargetTimer.Timeout += OnRecalculateTarget;
		_shootTargetTimer.Timeout += OnShootTarget;
	}

	private void OnEnemyEnteredRange(TowerPlaceholderEnemy enemy)
	{
		_enemiesInRange.Add(enemy);
	}

	private void OnEnemyExitedRange(TowerPlaceholderEnemy enemy)
	{
		_enemiesInRange.Remove(enemy);
	}

	private void OnRecalculateTarget()
	{
		if (_enemiesInRange.Count == 0)
		{
			_currentTarget = null;
			return;
		}

		_currentTarget = _towerTargeting switch
		{
			TowerTargeting.First => _enemiesInRange.OrderBy(enemy => enemy.GlobalPosition.X).Last(),
			TowerTargeting.Last => _enemiesInRange.OrderBy(enemy => enemy.GlobalPosition.X).First(),
			TowerTargeting.MostHealth => _enemiesInRange.OrderBy(enemy => enemy.GetHealth()).Last(),
			TowerTargeting.LeastHealth => _enemiesInRange.OrderBy(enemy => enemy.GetHealth()).First(),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private void OnShootTarget()
	{
		if (_currentTarget == null) return;
		
		var projectile = _projectileScene.Instantiate<TowerProjectile>();
		projectile.SetTarget(_currentTarget.GlobalPosition);
		
		AddChild(projectile);
	}
}
