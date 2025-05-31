using Godot;
using System;

[GlobalClass]
public partial class TowerEnhancement : Resource
{
	[Export] public bool Additive;
	
	[ExportGroup("Projectile Enhancement")]
	[Export] public int NewProjectileDamage = -1;
	[Export] public int NewProjectileSpeed = -1;
	
	[ExportGroup("Shooting Enhancement")]
	[Export] public Vector2 NewProjectileSpawnOffset = Vector2.Inf;
	
	[ExportGroup("Range Enhancement")]
	[Export] public int NewRange = -1;
	[Export] public Vector2 NewRangeOffset = Vector2.Inf;
}
