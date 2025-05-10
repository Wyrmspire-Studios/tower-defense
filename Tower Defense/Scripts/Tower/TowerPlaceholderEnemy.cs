using Godot;

namespace WyrmspireStudios;
public partial class TowerPlaceholderEnemy : AnimatableBody2D
{
	[Export] private float _health = 100f;
	
	public float GetHealth()
	{
		return _health;
	}
	
	public void ReduceHealth(float amount)
	{
		_health -= amount;
		if (_health <= 0) QueueFree();
	}
	
	public override void _Process(double delta)
	{
		Position += new Vector2(8, 0);
	}
}
