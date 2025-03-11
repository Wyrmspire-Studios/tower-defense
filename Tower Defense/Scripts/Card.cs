using Godot;
using System;
using Godot.Collections;

namespace WyrmspireStudios;
public partial class Card : Control
{
	[Export] private ColorRect _highlight;
	
	private bool _dragging = false;
	private Vector2 _defaultPosition;
	private Tween _positionTween;
	
	public override void _Ready()
	{
		_defaultPosition = Position;
	}

	public override void _Process(double delta)
	{
		var newPosition = _dragging ? GetGlobalMousePosition() : _defaultPosition;
		
		_positionTween?.Kill();
		_positionTween = CreateTween()
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Quad);
		_positionTween.TweenProperty(this, "position", newPosition, 0.1f);
		_positionTween.Play();
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
		{
			_dragging = true;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false })
		{
			_dragging = false;
		}
	}

	public override void _Notification(int what)
	{
		switch ((long) what)
		{
			case NotificationMouseEnter:
				_defaultPosition = _defaultPosition + new Vector2(0, -10);
				_highlight.Visible = true;
				ZIndex = 10;
				break;

			case NotificationMouseExit:
				_defaultPosition = _defaultPosition - new Vector2(0, -10);
				_highlight.Visible = false;
				ZIndex = 0;
				break;
		}
	}
}
