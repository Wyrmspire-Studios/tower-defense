using Godot;
using System;

[GlobalClass]
public partial class ProjectileInfo : Resource
{
	[Export] public float Speed;
	[Export] public int Damage;
}
