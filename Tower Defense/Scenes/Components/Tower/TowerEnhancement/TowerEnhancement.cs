using Godot;
using System;

[GlobalClass]
public partial class TowerEnhancement : Resource
{
	[Export] public bool Additive;
	
	[ExportGroup("Projectile Enhancement")]
	[Export] public int NewProjectileDamage = -1;
	[Export] public int NewProjectileSpeed = -1;
	[Export] public Vector2 NewProjectileSpawnOffset = Vector2.Inf;
	[Export] public float NewProjectileFireDelay = -1;
	[Export] public float NewProjectileStunDuration = -1;
	
	[ExportGroup("Beam Enhancement")]
	[Export] public int NewBeamDamage = -1;
	[Export] public Vector2 NewBeamSpawnOffset = Vector2.Inf;
	[Export] public float NewBeamFireDelay = -1;
	[Export] public float NewBeamFireDuration = -1;
	[Export] public float NewBeamStunDuration = -1;
	
	[ExportGroup("Unit Enhancement")]
	[Export] public int NewUnitHealth = -1;
	[Export] public float NewUnitSpawnRate = -1;
	[Export] public float NewUnitStunDuration = -1;
	
	[ExportGroup("Range Enhancement")]
	[Export] public int NewRange = -1;
	[Export] public Vector2 NewRangeOffset = Vector2.Inf;
}
