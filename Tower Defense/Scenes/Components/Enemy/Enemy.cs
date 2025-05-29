using Godot;
using System;

public partial class Enemy : PathFollow2D
{
	[Export] private EnemyInfo _enemyInfo;
	private EnemyInfo _currentEnemyInfo;

	private float _health;

	public override void _Ready()
	{
		_currentEnemyInfo = (EnemyInfo) _enemyInfo.Duplicate();
		_health = _currentEnemyInfo.Health;
	}

	public override void _Process(double delta)
	{
		Progress += _currentEnemyInfo.Speed * (float) delta;

		if (ProgressRatio >= 1) OnDamagePlayer();
	}
	
	public float GetHealth() => _health;

	public void TakeDamage(float damage)
	{
		_health -= damage;
		if (_health <= 0) OnDeath();
	}

	private void OnDeath()
	{
		QueueFree();
	}

	private void OnDamagePlayer()
	{
		QueueFree();
	}
}
