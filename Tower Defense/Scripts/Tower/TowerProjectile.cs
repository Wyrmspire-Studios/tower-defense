using Godot;

namespace WyrmspireStudios;
public partial class TowerProjectile : Area2D
{
	[Export] private float _projectileLifeTime;
	[Export] private float _projectileSpeed;
	[Export] private float _projectileDamage;

	[ExportGroup("Internal")]
	[Export] private Timer _projectileDeleteTimer;
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;

		_projectileDeleteTimer.WaitTime = _projectileLifeTime;
		_projectileDeleteTimer.Timeout += OnProjectileDelete;
	}
	
	public override void _Process(double delta)
	{
		Position += Transform.X * (_projectileSpeed * (float) delta);
	}

	public void SetTarget(Vector2 target)
	{
		LookAt(target);
	}

	private void OnProjectileDelete()
	{
		QueueFree();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is TowerPlaceholderEnemy enemy) HitEnemy(enemy);
	}

	private void HitEnemy(TowerPlaceholderEnemy enemy)
	{
		enemy.ReduceHealth(_projectileDamage);
		OnProjectileDelete();
	}
}
