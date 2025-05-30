using Godot;
using System;

public partial class TowerCollider : Area2D
{
	public delegate void TowerClickHandler(InputEventMouseButton ev);
	public event TowerClickHandler TowerClickedInside;
	public event TowerClickHandler TowerClickedOutside;
	
	public Tower Tower;
	
	[ExportGroup("Internal")]
	[Export] private CollisionShape2D _collisionShape;
	private RectangleShape2D _rectangleShape;
	private Vector2 _currentSize;

	public override void _Input(InputEvent ev)
	{
		if (ev is not InputEventMouseButton { Pressed: true } mouseEvent) return;
		if (!Tower.TowerInfo.Active) return;
		
		if (GetColliderRect().HasPoint(mouseEvent.GlobalPosition))
		{
			TowerClickedInside?.Invoke(mouseEvent);
		}
		else
		{
			TowerClickedOutside?.Invoke(mouseEvent);
		}
	}

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
