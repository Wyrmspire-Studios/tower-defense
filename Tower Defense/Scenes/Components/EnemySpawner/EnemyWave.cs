using Godot;
using Godot.Collections;

namespace WyrmspireStudios;


[GlobalClass]
public partial class EnemyWave : Resource
{
    [Export]
    public Array<EnemyGroup> EnemyGroups { get; set; }

    [Export] public float DelayUntilNextWave { get; set; } = 30f;
}