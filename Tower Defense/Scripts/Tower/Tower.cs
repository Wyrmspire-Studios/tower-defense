using Godot;
using System;

namespace WyrmspireStudios;
public partial class Tower : Node2D
{
	[Export(PropertyHint.Enum, "One:0,Two:1,Three:2,Four:3")] public int TowerTier;
	
	public TowerTexture TowerTexture;
	public TowerRange TowerRange;
	public TowerShoot TowerShoot;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
