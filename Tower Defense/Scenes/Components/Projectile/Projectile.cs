using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export] public ProjectileInfo ProjectileInfo;
	public RangedProjectileTower ShotBy;
	
	public override void _Ready()
	{
		AreaEntered += _onAreaEntered;
	}

	public override void _Process(double delta)
	{
		Position += Transform.X * (ProjectileInfo.Speed * (float)delta);
	}

	public void SetTarget(Vector2 target)
	{
		LookAt(target);
	}

	private void _onAreaEntered(Area2D other)
	{
		var otherParent = other.GetParent();
		if (otherParent is Enemy enemy) ShotBy.OnEnemyHit(enemy, this);
	}
}
