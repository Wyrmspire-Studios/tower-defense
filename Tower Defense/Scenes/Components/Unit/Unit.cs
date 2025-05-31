using Godot;
using System;

public partial class Unit : Area2D
{
	public SpawningTower SpawnedBy;
	public int Health;
	public Direction Direction;

	public override void _Ready()
	{
		AreaEntered += _onAreaEntered;
	}

	private void _onAreaEntered(Area2D other)
	{
		var otherParent = other.GetParent();
		if (otherParent is Enemy enemy) SpawnedBy.OnEnemyHit(enemy, this);
	}
}
