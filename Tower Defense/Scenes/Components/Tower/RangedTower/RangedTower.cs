using Godot;
using System;

public partial class RangedTower : Tower
{
	public RangedTowerInfo RangedTowerInfo;
	
	public TowerTargeting TowerTargeting;

	public override void OnStartPlacing(TowerPlacement towerPlacement, bool headless = false)
	{
		base.OnStartPlacing(towerPlacement, headless);
		
		if (headless) return;
		
		TowerTargeting = GetNode<TowerTargeting>("TowerTargeting");
		TowerTargeting.Initialize(this);
		TowerTargeting.ToggleRangeVisible();
	}
}
