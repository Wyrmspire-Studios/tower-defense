using Godot;
using System;

public partial class RangedTower : Tower
{
	public RangedTowerInfo RangedTowerInfo;
	
	public TowerTargeting TowerTargeting;
	
	public override void _Ready()
	{
		TowerTargeting = GetNodeOrNull<TowerTargeting>("TowerTargeting");
		
		base._Ready();
		
		TowerTargeting?.Initialize(this);
	}

	public override void OnStartPlacing()
	{
		base.OnStartPlacing();
		TowerTargeting?.ToggleRangeVisible();
	}

	public override void OnPlaceTower()
	{
		base.OnPlaceTower();
		TowerTargeting?.ToggleRangeVisible();
	}
}
