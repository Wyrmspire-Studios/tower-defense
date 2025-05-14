using Godot;
using System;

namespace WyrmspireStudios;

[Tool]
public partial class CustomTexturedButton : NinePatchRect
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
	private string _buttonLabel;
	private int _fontSize;

	public override void _Ready()
	{
		Texture = _baseTexture;
		var label = GetNodeOrNull<Label>("Button/Label");
		if (label == null) return;
		var labelSettings = new LabelSettings();
		labelSettings.FontSize = _fontSize;
		label.Text = _buttonLabel;
		label.LabelSettings = labelSettings;
	}

	public override void _Notification(int what)
	{
		Texture = (long)what switch
		{
			NotificationMouseEnter => _hoverTexture,
			NotificationMouseExit => _baseTexture,
			_ => Texture
		};
	}
}
