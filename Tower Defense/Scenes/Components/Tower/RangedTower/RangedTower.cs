using Godot;
using System;

public partial class RangedTower : Tower
{
	public RangedTowerInfo RangedTowerInfo;
	
	public TowerTargeting TowerTargeting;

	public override void OnStartPlacing()
	{
		base.OnStartPlacing();
		
		TowerTargeting = GetNode<TowerTargeting>("TowerTargeting");
		TowerTargeting.Initialize(this);
		TowerTargeting.ToggleRangeVisible();
	}
}
