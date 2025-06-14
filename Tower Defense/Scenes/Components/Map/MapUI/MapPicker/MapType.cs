using System;
using Godot;

public enum MapType
{
	Enemy,
	HardEnemy,
	VeryHardEnemy,
	Boss,
	Shop
}

public static class MapTypeExtensions
{
	public static int ToTextureIndex(this MapType mapType)
	{
		return mapType switch
		{
			MapType.Enemy => 0,
			MapType.HardEnemy => 1,
			MapType.VeryHardEnemy => 2,
			MapType.Boss => 3,
			MapType.Shop => 4,
			_ => throw new ArgumentOutOfRangeException(nameof(mapType), mapType, null)
		};
	}
}