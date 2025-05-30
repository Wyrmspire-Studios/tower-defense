using System;
using Godot;

public enum TowerTier
{
	One,
	Two,
	Three,
	Four
}

public static class TowerTierExtensions
{
	public static TowerTier Next(this TowerTier towerTier)
	{
		return towerTier switch
		{
			TowerTier.One => TowerTier.Two,
			TowerTier.Two => TowerTier.Three,
			TowerTier.Three => TowerTier.Four,
			TowerTier.Four => TowerTier.Four,
			_ => throw new ArgumentOutOfRangeException(nameof(towerTier), towerTier, null)
		};
	}
	public static Vector2 ToSpriteOffset(this TowerTier towerTier)
	{
		return towerTier switch
		{
			TowerTier.One => new Vector2(TowerSprite.TowerTextureWidth * 0, 0),
			TowerTier.Two => new Vector2(TowerSprite.TowerTextureWidth * 1, 0),
			TowerTier.Three => new Vector2(TowerSprite.TowerTextureWidth * 2, 0),
			TowerTier.Four => new Vector2(TowerSprite.TowerTextureWidth * 3, 0),
			_ => throw new ArgumentOutOfRangeException(nameof(towerTier), towerTier, null)
		};
	}

	public static int ToIndex(this TowerTier towerTier)
	{
		return towerTier switch
		{
			TowerTier.One => 0,
			TowerTier.Two => 1,
			TowerTier.Three => 2,
			TowerTier.Four => 3,
			_ => throw new ArgumentOutOfRangeException(nameof(towerTier), towerTier, null)
		};
	}
}