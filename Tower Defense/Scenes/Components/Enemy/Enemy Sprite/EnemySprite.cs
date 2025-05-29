using Godot;
using System;

public partial class EnemySprite : AnimatedSprite2D
{
	[ExportGroup("Internal")]
	[Export] private Enemy _enemy;
	
	private Vector2 _lastPosition = Vector2.Zero;

	private static readonly string[] AnimationNames = [
		"Left",
		"Up",
		"Right",
		"Down"
	];
	private static readonly int AnimationCount = AnimationNames.Length;

	public override void _Ready()
	{
		Play(AnimationNames[0]);
	}

	public override void _Process(double delta)
	{
		var currentMoveVector = GlobalPosition - _lastPosition;
		_lastPosition = GlobalPosition;
		
		var normalizedMoveVector = currentMoveVector.Normalized();
		var animationIndex = Mathf.FloorToInt(AnimationCount *
			(normalizedMoveVector.Rotated(float.Pi / AnimationCount).Angle() + double.Pi) / double.Tau);
		if (animationIndex < 0 || animationIndex >= AnimationNames.Length) animationIndex = 0;
		Play(AnimationNames[animationIndex]);
	}
}
