using Godot;
using System;

public partial class Tower : Node2D
{
	public TowerInfo TowerInfo;
	
	public TowerSprite TowerSprite;
	public TowerCollider TowerCollider;
	public TowerActions TowerActions;
	
	public override void _Ready()
	{
		TowerSprite = GetNodeOrNull<TowerSprite>("TowerSprite");
		TowerCollider = GetNodeOrNull<TowerCollider>("TowerCollider");
		TowerActions = GetNodeOrNull<TowerActions>("TowerActions");
		
		TowerSprite?.Initialize(this);
		TowerCollider?.Initialize(this);
		TowerActions?.Initialize(this);
	}

	public virtual void OnStartPlacing() {}
	public virtual void OnPlaceTower() {}
}
