using Godot;
using System;

namespace WyrmspireStudios;
public partial class TowerPlaceholderEnemy : AnimatableBody2D
{
	private string _id = Guid.NewGuid().ToString();
	[Export] private int _health = 100;
	
	public int GetHealth()
	{
		return _health;
	}
	public void ReduceHealth(int amount)
	{
		_health -= amount;
		if (_health <= 0) QueueFree();
	}
	
	public override void _Process(double delta)
	{
		Position += new Vector2(8, 0);
	}
}
