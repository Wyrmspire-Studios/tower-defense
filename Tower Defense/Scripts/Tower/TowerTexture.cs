using Godot;
using System;

namespace WyrmspireStudios;
public partial class TowerTexture : Sprite2D
{
	private const int TowerTextureWidth = 32;
	private const int TowerTextureHeight = 64;
	
	[Export] public Texture2D TowerTierTexture;

	private Tower _tower;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetParent<Tower>();
		_tower.TowerTexture = this;

		Texture = TowerTierTexture;
		Centered = false;
		Offset = new Vector2(-(TowerTextureWidth / 2f), -TowerTextureHeight);
		RegionEnabled = true;
		SetTowerTierTexture(_tower.TowerTier);
	}

	public void SetTowerTierTexture(int towerTier)
	{
		RegionRect = new Rect2(TowerTextureWidth * towerTier, 0, TowerTextureWidth, TowerTextureHeight);
	}
}
