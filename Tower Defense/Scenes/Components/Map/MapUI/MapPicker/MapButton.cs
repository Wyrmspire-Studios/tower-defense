using Godot;
using System;
using System.Collections.Generic;

public partial class MapButton : Button
{
	[Export] private NinePatchRect _backgroundRect;
	[Export] private NinePatchRect _backgroundShadowRect;
	[Export] private TextureRect _iconRect;
	[Export] private MarginContainer _killedContainer;

	[Export] private Texture2D _baseTexture;
	[Export] private Texture2D _hoverTexture;
	[Export] private Texture2D _clickTexture;
	[Export] private AtlasTexture _iconTexture;
	
	[Export] public MapType MapType;

	public List<MapButton> ConnectedTo = [];
	
	private bool _isHovered;
	private bool _isClicked;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_iconTexture.Region = new Rect2(new Vector2(32, 0) * MapType.ToTextureIndex(), new Vector2(32, 32));
		_iconRect.Texture = _iconTexture;
		
		_setBackgroundTexture(_baseTexture);

		MouseEntered += _onMouseEntered;
		MouseExited += _onMouseExited;
		ButtonDown += _onMouseDown;
		ButtonUp += _onMouseUp;
		
		Disable();
	}

	private void _setBackgroundTexture(Texture2D texture)
	{
		_backgroundRect.Texture = texture;
		_backgroundShadowRect.Texture = texture;
	}
	
	private void _onMouseEntered()
	{
		if (Disabled) return;
		_isHovered = true;
		_setBackgroundTexture(_hoverTexture);
	}
	private void _onMouseExited() 
	{
		if (Disabled) return;
		_isHovered = false;
		_setBackgroundTexture(_baseTexture);
	}
	private void _onMouseDown()
	{
		if (Disabled) return;
		_isClicked = true;
		_setBackgroundTexture(_clickTexture);
	}
	private void _onMouseUp()
	{
		if (Disabled) return;
		_isClicked = false;
        var texture = GetRect().HasPoint(GetLocalMousePosition()) ? _hoverTexture : _baseTexture;
		_setBackgroundTexture(texture);
	}

	public void MarkKilled()
	{
		_killedContainer.Visible = true;
	}

	public void Enable()
	{
		_setBackgroundTexture(_baseTexture);
		_backgroundRect.SelfModulate = Color.Color8(255, 255, 255);
		_iconRect.SelfModulate = Color.Color8(255, 255, 255);
		Disabled = false;
	}

	public void Disable()
	{
		_setBackgroundTexture(_baseTexture);
		_backgroundRect.SelfModulate = Color.Color8(192, 192, 192);
		_iconRect.SelfModulate = Color.Color8(192, 192, 192);
		Disabled = true;
	}
}
