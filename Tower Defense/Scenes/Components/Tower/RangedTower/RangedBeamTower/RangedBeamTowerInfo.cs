using Godot;
using System;

[GlobalClass]
public partial class RangedBeamTowerInfo : RangedTowerInfo
{
	[ExportGroup("Ranged Beam Tower Info")]
	[Export] public Vector2 BeamSpawnOffset = new(16, 16);
	[Export] public float FireDelay = 1f;
	[Export] public float FireDuration = 1f;
	[Export] public int Damage = 1;
	[Export] public Color BackgroundBeamColor = Color.Color8(255, 255, 255);
	[Export] public Color ForegroundBeamColor = Color.Color8(192, 192, 192);
}
