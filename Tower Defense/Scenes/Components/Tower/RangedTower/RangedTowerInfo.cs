using Godot;
using System;

[GlobalClass]
public partial class RangedTowerInfo : TowerInfo
{
	[ExportGroup("Ranged Tower Info")]
	[Export] public int Range = 64;
	[Export] public Vector2 RangeOffset = new(16, 16);
	[Export] public TowerTargetingMode TargetingMode = TowerTargetingMode.First;
}
