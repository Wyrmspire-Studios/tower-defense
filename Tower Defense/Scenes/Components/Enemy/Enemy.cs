using Godot;
using System;
using WyrmspireStudios.Data;

public partial class Enemy : PathFollow2D
{
	[Export] private EnemyInfo _enemyInfo;
	private EnemyInfo _currentEnemyInfo;

	private int _health;

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
	
	public int GetHealth() => _health;

	public void TakeDamage(int damage)
	{
		_health -= damage;
		if (_health <= 0) OnDeath();
	}

	private void OnDeath()
	{
		LevelData.AddEnemyDeath();
		LevelData.AddGold(_currentEnemyInfo.GoldDrop);
		QueueFree();
	}

	private void OnDamagePlayer()
	{
		LevelData.RemoveHealth(_currentEnemyInfo.Damage * _currentEnemyInfo.Health / GetHealth());
		QueueFree();
	}
}
