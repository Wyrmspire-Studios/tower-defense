using Godot;
using System;

namespace WyrmspireStudios.Enemies;

public partial class Slime : PathFollow2D
{
	[Export] private int _speed = 100;
	[Export] private int _health = 100;
	[Export] private int _goldDropped = 1;
	[Export] private int _damage = 1;
	
	
	public override void _Process(double delta)
	{
		SetProgress((float)(GetProgress() + _speed * delta));
		if (ProgressRatio >= 1.0)
		{
			GameData.RemoveHealth(_damage);
			QueueFree();
		}
	}

	public void TakeDamage(int damage)
	{
		_health -= damage;
		if (_health <= 0)
		{
			GameData.AddGold(_goldDropped);
			GameData.AddEnemyDeath();
			QueueFree();
		}
	}
}
