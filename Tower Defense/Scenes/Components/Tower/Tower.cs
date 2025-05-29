using Godot;
using System;

public partial class Tower : Node2D
{
	public TowerInfo TowerInfo;
	
	public TowerSprite TowerSprite;
	
	public override void _Ready()
	{
		TowerSprite = GetNodeOrNull<TowerSprite>("TowerSprite");
		TowerSprite?.Initialize(this);
	}

	public virtual void OnStartPlacing() {}
	public virtual void OnPlaceTower() {}
}
