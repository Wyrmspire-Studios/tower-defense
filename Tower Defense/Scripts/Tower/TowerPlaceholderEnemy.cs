using Godot;
using System;

public partial class TowerPlaceholderEnemy : AnimatableBody2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(8, 0);
	}
}
