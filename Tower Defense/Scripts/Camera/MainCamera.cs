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

	/// <summary>
	/// Movement smoothing value applied to the camera
	/// </summary>
	[Export] private float _movementSmoothing = 15f;
	
	/// <summary>
	/// Speed at which to zoom
	/// </summary>
	[Export] private float _zoomSpeed = 25f;
	
	/// <summary>
	/// Minimum allowed zoom
	/// </summary>
	[Export] private float _minZoom = 1f;
	
	/// <summary>
	/// Maximum allowed zoom
	/// </summary>
	[Export] private float _maxZoom = 3f;

	/// <summary>
	/// Duration the zoom Tween takes to complete
	/// </summary>
	[Export] private float _zoomTweenDuration = 0.1f;

	/// <summary>
	/// Default zoom to reset to
	/// </summary>
	private Vector2 _defaultZoom;
	
	/// <summary>
	/// Tween used for smooth zooming
	/// </summary>
	private Tween _zoomTween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PositionSmoothingEnabled = true;
		PositionSmoothingSpeed = _movementSmoothing;

		_defaultZoom = new Vector2(_minZoom, _minZoom);
		Zoom = _defaultZoom;
	}

	public override void _Input(InputEvent ev)
	{
		if (!ev.IsActionPressed("Reset Camera")) return;
		
		Position = Vector2.Zero;
		_setZoom(_defaultZoom);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var deltaFloat = (float)delta;

		#region Movement

		var movement = Vector2.Zero;
		if (Input.IsActionPressed("Move Up")) movement += Vector2.Up;
		if (Input.IsActionPressed("Move Down")) movement += Vector2.Down;
		if (Input.IsActionPressed("Move Left")) movement += Vector2.Left;
		if (Input.IsActionPressed("Move Right")) movement += Vector2.Right;
		movement = movement.Normalized();
		
		var movementMultiplier = _movementSpeed * (Input.IsActionPressed("Run") ? _runMultiplier : 1f);

		Position += movement * (movementMultiplier * deltaFloat);
		
		#endregion

		#region Zoom

		var zoom = 0f;
		if (Input.IsActionJustPressed("Zoom In")) zoom += _zoomSpeed;
		if (Input.IsActionJustPressed("Zoom Out")) zoom -= _zoomSpeed;

		if (zoom == 0) return;
		var newZoom = Mathf.Clamp(Zoom.X + zoom * deltaFloat, _minZoom, _maxZoom);
		
		_setZoom(new Vector2(newZoom, newZoom));

		#endregion
	}

	private void _setZoom(Vector2 newZoom)
	{
		_zoomTween?.Kill();
		_zoomTween = CreateTween()
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Quad);
		_zoomTween.TweenProperty(this, "zoom", newZoom, _zoomTweenDuration);
		_zoomTween.Play();
	}
}