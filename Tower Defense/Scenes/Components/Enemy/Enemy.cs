using Godot;
using System;
using WyrmspireStudios.Data;

public partial class Enemy : PathFollow2D
{
	public delegate void EnemyDiedHandler(Enemy enemy);
	public static event EnemyDiedHandler EnemyDied;
	
	[Export] private EnemyInfo _enemyInfo;
	private EnemyInfo _currentEnemyInfo;

	private float _secondsActive;
	private float _stunnedUntil = -1;
	
	private int _health;

	public override void _Ready()
	{
		_currentEnemyInfo = (EnemyInfo) _enemyInfo.Duplicate();
		_health = _currentEnemyInfo.Health;
	}

	public override void _Process(double delta)
	{
		_secondsActive += (float)delta;
		if (IsStunned()) return;
		
		Progress += _currentEnemyInfo.Speed * (float) delta;
		
		if (ProgressRatio >= 1) OnDamagePlayer();
	}
	
	public int GetHealth() => _health;
	public bool IsStunned() => _stunnedUntil > _secondsActive;

	public void TakeDamage(int damage)
	{
		_health -= damage;
		if (_health <= 0) OnDeath();
	}

	public void ApplyStun(float stunSeconds)
	{
		if (IsStunned()) _stunnedUntil += stunSeconds;
		else _stunnedUntil = _secondsActive + stunSeconds;
	}

	private void OnDeath()
	{
		LevelData.AddEnemyDeath();
		LevelData.AddGold(_currentEnemyInfo.GoldDrop);
		EnemyDied?.Invoke(this);
		QueueFree();
	}

	private void OnDamagePlayer()
	{
		if (GetHealth() > 0) LevelData.RemoveHealth(_currentEnemyInfo.Damage * _currentEnemyInfo.Health / GetHealth());
		EnemyDied?.Invoke(this);
		QueueFree();
	}
}
