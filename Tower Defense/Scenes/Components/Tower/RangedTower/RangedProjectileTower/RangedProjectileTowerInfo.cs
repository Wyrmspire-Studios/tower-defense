using Godot;
using System;

[GlobalClass]
public partial class RangedProjectileTowerInfo : RangedTowerInfo
{
	[ExportGroup("Ranged Projectile Tower Info")]
	[Export] public PackedScene ProjectileScene;
	[Export] public ProjectileInfo BaseProjectileInfo = new();
	[Export] public Vector2 ProjectileSpawnOffset = new(16, 16);
	[Export] public float FireDelay = 1f;
}
