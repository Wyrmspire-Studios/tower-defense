using Godot;
using System;

public partial class Slime : PathFollow2D
{
	[Export] private int _speed = 100;
	[Export] private int _health = 100;
	[Export] private int _coinsDropped = 1;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		SetProgress((float)(GetProgress() + _speed * delta));
	}

	public void TakeDamage(int damage)
	{
		_health -= damage;
	}
}
