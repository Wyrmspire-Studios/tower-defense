using Godot;
using System;

public partial class TowerProjectileShooting : Node2D
{
	[ExportGroup("Internal")]
	[Export] private Timer _shootTimer;
	
	public RangedProjectileTower Tower;

	private Vector2 _target;
	
	public void Initialize(RangedProjectileTower tower)
	{
		Tower = tower;

		_shootTimer.Timeout += _shoot;
	}

	private void _shoot()
	{
		var currentTarget = Tower.TowerTargeting.GetCurrentTarget();
		if (currentTarget == null) return;
		
		var projectile = Tower.RangedProjectileTowerInfo.ProjectileScene.Instantiate<Projectile>();
		projectile.ShotBy = Tower;
		projectile.Position = Tower.RangedProjectileTowerInfo.ProjectileSpawnOffset;
		projectile.SetTarget(currentTarget.GlobalPosition - GlobalPosition);
		AddChild(projectile);
	}
}
