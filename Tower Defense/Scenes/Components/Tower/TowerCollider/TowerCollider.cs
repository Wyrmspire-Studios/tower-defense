using Godot;
using System;

public partial class TowerCollider : Area2D
{
	public Tower Tower;
	
	[ExportGroup("Internal")]
	[Export] private CollisionShape2D _collisionShape;
	private RectangleShape2D _rectangleShape;
	private Vector2 _currentSize;
	
	public Rect2 GetColliderRect() => new Rect2(GlobalPosition, _currentSize);
	
	public void Initialize(Tower tower)
	{
		Tower = tower;
		
		_rectangleShape = new RectangleShape2D();
		_collisionShape.Shape = _rectangleShape;
		ChangeSize(new Vector2(32, 32));
	}

	public void ChangeSize(Vector2 newSize)
	{
		_rectangleShape.Size = newSize;
		_collisionShape.Position = new Vector2(16, newSize.Y / 2);
		_currentSize = newSize;
	}
}
