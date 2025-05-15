using Godot;

namespace WyrmspireStudios;

[Tool]
public partial class CustomTexturedButton : Button
{
	[Export]
	public Texture2D BaseTexture
	{
		get => _baseTexture;
		set
		{
			_baseTexture = value;
			_Ready();
		}
	}

	[Export]
	public Texture2D HoverTexture
	{
		get => _hoverTexture;
		set 
		{
			_hoverTexture = value;
			_Ready();
		}
	}

	[Export]
	public Texture2D PressedTexture
	{
		get => _pressedTexture;
		set
		{
			_pressedTexture = value;
			_Ready();
		}
	}

	[Export]
	public string ButtonLabel
	{
		get => _buttonLabel;
		set
		{
			_buttonLabel = value;
			_Ready();
		}
	}

	[Export]
	public int FontSize
	{
		get => _fontSize;
		set
		{
			_fontSize = value;
			_Ready();
		}
	}
	
	private Texture2D _baseTexture;
	private Texture2D _hoverTexture;
	private Texture2D _pressedTexture;
	private string _buttonLabel;
	private int _fontSize;
	
	private NinePatchRect _background;
	private bool _signalsConnected = false;
	public override void _Ready()
	{
		_background = GetNodeOrNull<NinePatchRect>("Background");
		if(_background == null) return;
		_background.Texture = _baseTexture;

		if (!_signalsConnected)
		{
			ButtonDown += OnButtonDown;
			ButtonUp += OnButtonUp;
			MouseEntered += OnMouseEntered;
			MouseExited += OnMouseExited;
			_signalsConnected = true;
		}
		
		var label = GetNodeOrNull<Label>("Label");
		if (label == null) return;
		var labelSettings = new LabelSettings();
		labelSettings.FontSize = _fontSize;
		label.Text = _buttonLabel;
		label.LabelSettings = labelSettings;
	}
	
	private void OnButtonDown()
	{
		_background.Texture = _pressedTexture;
	}

	private void OnButtonUp()
	{
		_background.Texture = GetRect().HasPoint(GetLocalMousePosition()) ? _baseTexture : _hoverTexture;
	}

	private void OnMouseEntered()
	{
		if (!IsPressed())
			_background.Texture = _hoverTexture;
	}

	private void OnMouseExited()
	{
		if (!IsPressed())
			_background.Texture = _baseTexture;
	}
	
	// public override void _Process(double delta)
	// {
	// 	var backgroundTexture = _baseTexture;
	//
	// 	if (IsPressed())
	// 	{
	// 		backgroundTexture = _pressedTexture;
	// 	}
	// 	else if (IsHovered())
	// 	{
	// 		backgroundTexture = _hoverTexture;
	// 	}
	//
	// 	if (_background.Texture != backgroundTexture)
	// 	{
	// 		_background.Texture = backgroundTexture;
	// 	}
	// }
}
