using Godot;
using System;

public partial class RangedProjectileTower : RangedTower
{
	[Export] public RangedProjectileTowerInfo RangedProjectileTowerInfo;
	
	public TowerProjectileShooting TowerProjectileShooting;
	
	public override void _Ready()
	{
		_updateTowerInfo(RangedProjectileTowerInfo);

		TowerProjectileShooting = GetNodeOrNull<TowerProjectileShooting>("TowerProjectileShooting");
		
		base._Ready();
		
		TowerProjectileShooting?.Initialize(this);
	}

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
		base.OnStartPlacing();
		RangedProjectileTowerInfo.Active = false;
	}

	public override void OnPlaceTower()
	{
		base.OnPlaceTower();
		RangedProjectileTowerInfo.Active = true;
	}
}
