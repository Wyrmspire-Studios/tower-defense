using Godot;
using Godot.Collections;

namespace WyrmspireStudios;

[GlobalClass]
public partial class Enemy : Resource
{
    [Export] public PackedScene EnemyScene { get; set; }
    [Export] public int PathIdToSpawnOn { get; set; } = 0;
    [Export] public float DelayUntilNextEnemy { get; set; } = 0f;
}