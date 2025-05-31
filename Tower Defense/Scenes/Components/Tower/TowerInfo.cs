using Godot;
using System;

[GlobalClass]
public partial class TowerInfo : Resource
{
	[ExportGroup("Tower Info")]
	[Export] public string Name;
	[Export] public bool Active;
	[Export] public TowerTier TowerTier;
	[Export] public Texture2D TowerTierTexture;
	[Export] public int BaseUpgradeCost;
	[Export] public TowerEnhancement[] TowerTierEnhancements;
	[Export] public TowerEnhancement GivenEnhancement;
}
