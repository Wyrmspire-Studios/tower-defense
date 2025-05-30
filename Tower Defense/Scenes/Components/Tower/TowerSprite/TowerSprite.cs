using Godot;
using System;

public partial class TowerSprite : Sprite2D
{
	[Export] private PackedScene _placementSmokeScene;

	public Tower Tower;
	
	public const int TowerTextureWidth = 32;
	public const int TowerTextureHeight = 64;
	
	public void Initialize(Tower tower)
	{
		Tower = tower;
		
		Texture = Tower.TowerInfo.TowerTierTexture;
		RegionEnabled = true;
		
		SetSpriteToTier(Tower.TowerInfo.TowerTier);
	}

	public void ShowSmoke()
	{
		var placementSmoke = _placementSmokeScene.Instantiate<AnimatedSprite2D>();
		placementSmoke.GlobalPosition = Tower.GlobalPosition;
		placementSmoke.AnimationFinished += placementSmoke.QueueFree; 
		GetTree().GetRoot().AddChild(placementSmoke);
	}

	public void SetSpriteToTier(TowerTier towerTier)
	{
		var spriteOffset = towerTier.ToSpriteOffset();
		var newRect = new Rect2(spriteOffset, new Vector2(TowerTextureWidth, TowerTextureHeight));
		RegionRect = newRect;
	}
	
	public void UpgradeSpriteToTier(TowerTier towerTier)
	{
		SetSpriteToTier(towerTier);
		ShowSmoke();
	}
}
