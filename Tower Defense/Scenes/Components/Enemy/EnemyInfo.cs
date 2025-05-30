using Godot;
using System;

[GlobalClass]
public partial class EnemyInfo : Resource
{
	[Export] public int Speed;
	[Export] public int Health;
	[Export] public int Damage;
	[Export] public int GoldDrop;
}
