using Godot;
using System;

[GlobalClass]
public partial class EnemyInfo : Resource
{
	[Export] public float Speed;
	[Export] public float Health;
	[Export] public float Damage;
}
