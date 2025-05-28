using Godot;
using Godot.Collections;

namespace WyrmspireStudios;

[GlobalClass]
public partial class EnemyGroup : Resource
{
	[Export] public Enemy EnemyToSpawn { get; set; }
	[Export] public int EnemyCount { get; set; } = 1;
	[Export] public float SpawnIntervalSeconds { get; set; } = 1.0f;
	[Export] public float DelayUntilNextGroup { get; set; } = 1.0f;
}
