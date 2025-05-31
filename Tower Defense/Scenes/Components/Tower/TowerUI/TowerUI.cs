using Godot;
using System;

public partial class TowerUI : CanvasLayer
{
	[Export] public TowerActions TowerActions;
	[Export] public TowerStats TowerStats;

	private Tower _tower;
	public void Initialize(Tower tower)
	{
		_tower = tower;
		
		_tower.TowerCollider.TowerClickedInside += _onTowerClickedInside;
		_tower.TowerCollider.TowerClickedOutside += _onTowerClickedOutside;
		
		TowerActions.Initialize(_tower);
		TowerStats.Initialize(_tower);
	}
	
	public void ToggleVisibility()
	{
		Visible = !Visible;
	}

	private void _onTowerClickedInside(InputEventMouseButton ev)
	{
		ToggleVisibility();
	}

	private void _onTowerClickedOutside(InputEventMouseButton ev)
	{
		if (!Visible) return;
		if (TowerActions.GetGlobalRect().HasPoint(ev.GlobalPosition)) return;
		ToggleVisibility();
	}
}
