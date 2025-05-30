using Godot;
using System;

public partial class TowerProjectileShooting : Node2D
{
	[ExportGroup("Internal")]
	[Export] private Timer _shootTimer;
	
	public RangedProjectileTower Tower;

	public void Initialize(RangedProjectileTower tower)
	{
		Tower = tower;

		_shootTimer.Timeout += _shoot;
	}

	public void UpdateFireDelay()
	{
		_shootTimer.WaitTime = Tower.RangedProjectileTowerInfo.FireDelay;
	}

	private void _shoot()
	{
		if (!Tower.RangedProjectileTowerInfo.Active) return;
		
		var currentTarget = Tower.TowerTargeting.GetCurrentTarget();
		if (currentTarget == null) return;
		
		var projectile = Tower.RangedProjectileTowerInfo.ProjectileScene.Instantiate<Projectile>();
		projectile.ProjectileInfo = Tower.RangedProjectileTowerInfo.BaseProjectileInfo;
		projectile.ShotBy = Tower;
		projectile.Position = Tower.RangedProjectileTowerInfo.ProjectileSpawnOffset;
		projectile.SetTarget(currentTarget.GlobalPosition - GlobalPosition);
		AddChild(projectile);
	}
}
