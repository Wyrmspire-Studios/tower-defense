using Godot;
using System;

public partial class MapUi : Control
{
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("ShowUI");
	}

	public override void _ExitTree()
	{
		_animationPlayer.Play("HideUI");
	}
}
