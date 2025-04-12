using Godot;

namespace WyrmspireStudios;
public partial class MainCamera : Camera2D
{
	/// <summary>
	/// Movement speed of the <see cref="MainCamera"/>.
	/// </summary>
	[ExportGroup("Movement")] [Export] private float _movementSpeed = 250f;
	
	/// <summary>
	/// Multiplier applied to movement of the <see cref="MainCamera"/> when <c>Run</c> is pressed.
	/// </summary>
	[Export] private float _runMultiplier = 2f;

	/// <summary>
	/// Movement smoothing value applied to the <see cref="MainCamera"/>.
	/// </summary>
	[Export] private float _movementSmoothing = 15f;
	
	/// <summary>
	/// Speed at which to <see cref="Camera2D.Zoom"/> if using a mouse.
	/// </summary>
	[ExportGroup("Zoom")] [Export] private float _zoomSpeedMouse = 25f;

	/// <summary>
	/// Speed at which to <see cref="Camera2D.Zoom"/> if using a gamepad.
	/// </summary>
	[Export] private float _zoomSpeedGamepad = 12.5f;
	
	/// <summary>
	/// Minimum allowed <see cref="Camera2D.Zoom"/>.
	/// </summary>
	[Export] private float _minZoom = 1f;
	
	/// <summary>
	/// Maximum allowed <see cref="Camera2D.Zoom"/>.
	/// </summary>
	[Export] private float _maxZoom = 3f;

	/// <summary>
	/// Duration the <see cref="Camera2D.Zoom">Zoom </see><see cref="Tween"/> takes to complete.
	/// </summary>
	[Export] private float _zoomTweenDuration = 0.1f;

	/// <summary>
	/// Default <see cref="Camera2D.Zoom"/> to reset to.
	/// </summary>
	private Vector2 _defaultZoom;
	
	/// <summary>
	/// <see cref="Tween"/> used for smooth <see cref="Camera2D.Zoom"/>.
	/// </summary>
	private Tween _zoomTween;

	/// <summary>
	/// <b>Runs when the Node gets created. </b>
	/// </summary>
	/// <remarks>
	/// <list>
	///		<item>
	///			Enables and sets movement smoothing.
	///		</item>
	///		<item>
	///			Creates <see cref="_defaultZoom"/> and sets <see cref="Camera2D.Zoom"/> to it.
	///		</item>
	/// </list>
	/// </remarks>
	public override void _Ready()
	{
		PositionSmoothingEnabled = true;
		PositionSmoothingSpeed = _movementSmoothing;

		_defaultZoom = new Vector2(_minZoom, _minZoom);
		Zoom = _defaultZoom;
	}

	/// <summary>
	/// <b>Runs when an <see cref="InputEvent"/> does not get handled by anything.</b>
	/// </summary>
	/// <remarks>
	/// <list>
	///		<item>
	///			Resets the camera <see cref="Node2D.Position" /> and <see cref="Camera2D.Zoom"/> if <c>Reset Camera</c> was pressed.
	///		</item>
	/// </list>
	/// </remarks>
	/// <param name="ev"><see cref="InputEvent"/> that got fired.</param>
	public override void _UnhandledInput(InputEvent ev)
	{
		if (!ev.IsActionPressed("Reset Camera")) return;
		
		Position = Vector2.Zero;
		_setZoom(_defaultZoom);
	}

	/// <summary>
	/// <b>Runs every frame.</b>
	/// </summary>
	/// <remarks>
	/// <list>
	///		<item>
	///			Converts <paramref name="delta"/> to <see cref="float">float</see>.
	///		</item>
	///		<item>
	///			Handles movement input.
	///		</item>
	///		<item>
	///			Handles <see cref="Camera2D.Zoom"/> input.
	///		</item>
	/// </list>
	/// </remarks>
	/// <param name="delta">Time since last frame.</param>
	public override void _Process(double delta)
	{
		var deltaFloat = (float)delta;
		_handleMovement(deltaFloat);
		_handleZoom(deltaFloat);
	}

	/// <summary>
	/// <b>Handles movement input and position updates.</b>
	/// </summary>
	/// <remarks>
	/// <list>
	///		<item>
	///			Creates a movement <see cref="Vector2"/>.
	///		</item>
	///		<item>
	///			Returns early if no movement input detected.
	///		</item>
	///		<item>
	///			Creates a movement multiplier based on the <c>Run</c> input action.
	///		</item>
	///		<item>
	///			Moves the <see cref="Camera2D"/>.
	///		</item>
	/// </list>
	/// </remarks>
	/// <param name="deltaFloat">Time since last frame.</param>
	private void _handleMovement(float deltaFloat)
	{
		var movement = Input.GetVector(
			"Move Left",
			"Move Right",
			"Move Up",
			"Move Down"
		);

		if (movement.IsZeroApprox()) return;
		
		var movementMultiplier = _movementSpeed * (Input.IsActionPressed("Run") ? _runMultiplier : 1f);
		Position += movement * (movementMultiplier * deltaFloat);
	}

	/// <summary>
	/// <b>Handles zoom input and zoom updates.</b>
	/// </summary>
	/// <remarks>
	/// <list>
	///		<item>
	///			Creates a <see cref="Camera2D.Zoom"/> <see cref="float">float</see>.
	///		</item>
	///		<item>
	///			Returns early if no <see cref="Camera2D.Zoom"/> input detected.
	///		</item>
	///		<item>
	///			Creates a new <see cref="Camera2D.Zoom"/> value clamped between <see cref="_minZoom"/> and <see cref="_maxZoom"/>.
	///		</item>
	///		<item>
	///			Runs <see cref="_setZoom"/> to tween to the new <see cref="Camera2D.Zoom"/>.
	///		</item>
	/// </list>
	/// </remarks>
	/// <param name="deltaFloat">Time since last frame.</param>
	private void _handleZoom(float deltaFloat)
	{
		var zoom = _getZoomInput();
		if (zoom == 0f) return;
		
		var newZoom = Mathf.Clamp(Zoom.X + zoom * deltaFloat, _minZoom, _maxZoom);
		_setZoom(new Vector2(newZoom, newZoom));
	}

	/// <summary>
	/// <b>Gets the current <see cref="Camera2D.Zoom"/> input, based on <see cref="InputType"/>.</b>
	/// </summary>
	/// <remarks>
	///	<list>
	///		<item>
	///			If using a gamepad, uses <see cref="Input.GetAxis"/>.
	///		</item>
	///		<item>
	///			If using a mouse, uses <see cref="Input.IsActionJustPressed"/> for mouse wheel input.
	///		</item>
	/// </list>
	/// </remarks>
	/// <returns>The <see cref="Camera2D.Zoom"/> input, positive for zooming in and negative for zooming out.</returns>
	private float _getZoomInput()
	{
		if (!InputType.IsKeyboardAndMouse)
			return Input.GetAxis("Zoom Out", "Zoom In") * _zoomSpeedGamepad;
		
		return Input.IsActionJustPressed("Zoom In") ? _zoomSpeedMouse :
			Input.IsActionJustPressed("Zoom Out") ? -_zoomSpeedMouse : 0f;
	}

	/// <summary>
	/// <b>Creates and runs a <see cref="Tween"/> that tweens the <see cref="Camera2D.Zoom"/> to a new value.</b>
	/// </summary>
	/// <remarks>
	///	<list>
	///		<item>
	///			Kills the currently running <see cref="Tween"/>, if there is one.
	///		</item>
	///		<item>
	///			Constructs a new <see cref="Tween"/> with <see cref="Tween.TransitionType.Quad">Quad</see>
	///			<see cref="Tween.EaseType.Out">Out</see> easing.
	///		</item>
	///		<item>
	///			Tweens the camera's <see cref="Camera2D.Zoom"/> to the provided <paramref name="newZoom"/>.
	///		</item>
	///		<item>
	///			Plays the <see cref="Tween"/>.
	///		</item>
	/// </list>
	/// </remarks>
	/// <param name="newZoom">The new <see cref="Camera2D.Zoom"/> to tween to.</param>
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