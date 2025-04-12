using Godot;
using System;

namespace WyrmspireStudios;
public partial class MainCamera : Camera2D
{
	/// <summary>
	/// Movement speed of the camera
	/// </summary>
	[Export] private float _movementSpeed = 250f;
	
	/// <summary>
	/// Multiplier applied when running
	/// </summary>
	[Export] private float _runMultiplier = 2f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var movement = Vector2.Zero;
		if (Input.IsActionPressed("Move Up")) movement += Vector2.Up;
		if (Input.IsActionPressed("Move Down")) movement += Vector2.Down;
		if (Input.IsActionPressed("Move Left")) movement += Vector2.Left;
		if (Input.IsActionPressed("Move Right")) movement += Vector2.Right;
		movement = movement.Normalized();
		
		var movementMultiplier = _movementSpeed * (Input.IsActionPressed("Run") ? _runMultiplier : 1f);

		Position += movement * (movementMultiplier * (float)delta);
	}
}