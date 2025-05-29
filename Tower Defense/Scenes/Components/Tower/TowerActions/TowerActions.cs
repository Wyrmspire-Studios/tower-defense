using Godot;
using System;

public partial class TowerActions : NinePatchRect
{
	public Tower Tower;
	
	[ExportGroup("Internal")]
	[Export] private TextureButton _sellTowerButton;
	[Export] private TextureButton _upgradeTowerButton;
	public void Initialize(Tower tower)
	{
		Tower = tower;

		Tower.TowerCollider.InputEvent += _onColliderInputEvent;

		_sellTowerButton.Pressed += _onSellTower;
		_upgradeTowerButton.Pressed += _onUpgradeTower;
	}
	
	public override void _Input(InputEvent ev)
	{
		if (!Visible) return;
		if (ev is not InputEventMouseButton { Pressed: true } mouseEvent) return;

		var colliderRect = Tower.TowerCollider.GetColliderRect();
		if (!GetGlobalRect().HasPoint(mouseEvent.GlobalPosition) && !colliderRect.HasPoint(mouseEvent.GlobalPosition))
		{
			ToggleVisibility();
		}
	}

	public void ToggleVisibility()
	{
		Visible = !Visible;
	}

	private void _onColliderInputEvent(Node viewport, InputEvent ev, long shapeId)
	{
		if (ev is not InputEventMouseButton { Pressed: true }) return;
		if (!Tower.TowerInfo.Active) return;
		ToggleVisibility();
	}
	private void _onSellTower() {}
	private void _onUpgradeTower() {}
}
