using System;
using Godot;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public static class DirectionExtensions
{
	public static Vector2I ToVector2I(this Direction direction)
	{
		return direction switch
		{
			Direction.Up => Vector2I.Up,
			Direction.Down => Vector2I.Down,
			Direction.Left => Vector2I.Left,
			Direction.Right => Vector2I.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}
}