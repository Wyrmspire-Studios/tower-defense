using Godot;
using System;

[GlobalClass]
public partial class TowerInfo : Resource
{
	[ExportGroup("Tower Info")]
	[Export] public bool Active = true;
	[Export] public TowerTier TowerTier;
	[Export] public Texture2D TowerTierTexture;
}
