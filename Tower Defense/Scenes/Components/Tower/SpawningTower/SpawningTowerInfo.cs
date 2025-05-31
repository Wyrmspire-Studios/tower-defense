using Godot;
using System;

[GlobalClass]
public partial class SpawningTowerInfo : TowerInfo
{
	[ExportGroup("Spawning Tower Info")]
	[Export] public PackedScene SpawnedUnit;
	[Export] public int UnitHealth;
	[Export] public float UnitSpawnRate;
	[Export] public float UnitStunDuration;
}
