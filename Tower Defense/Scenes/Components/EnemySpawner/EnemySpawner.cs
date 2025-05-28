using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace WyrmspireStudios;

public partial class EnemySpawner : Node2D
{
    [Export] public Array<Path2D> SpawnPoints;
    [Export] public Array<EnemyWave> Waves;
    private int _waveIndex = 0;

    public override void _Ready()
    {
        _ = SpawnWave();
    }

    private async Task SpawnWave()
    {
        if (_waveIndex >= Waves.Count)
        {
            GD.Print("All waves completed.");
            return;
        }

        EnemyWave wave = Waves[_waveIndex];
        foreach (var group in Waves[_waveIndex].EnemyGroups)
        {
            _ = SpawnGroup(group);
            await ToSignal(GetTree().CreateTimer(group.DelayUntilNextGroup), "timeout");
        }

        if (_waveIndex < Waves.Count)
        {
            await ToSignal(GetTree().CreateTimer(wave.DelayUntilNextWave), "timeout");
            _waveIndex++;
            await SpawnWave();
        }
    }

    private async Task SpawnGroup(EnemyGroup enemyGroup)
    {
        for (int enemyIndex = 0; enemyIndex < enemyGroup.EnemyCount; enemyIndex++)
        {
            SpawnEnemy(enemyGroup.EnemyToSpawn);
            await ToSignal(GetTree().CreateTimer(enemyGroup.SpawnIntervalSeconds + enemyGroup.EnemyToSpawn.DelayUntilNextEnemy), "timeout");
        }
    }
    
    private void SpawnEnemy(Enemy enemyResource)
    {
        var enemy = (PathFollow2D)enemyResource.EnemyScene.Instantiate();
        SpawnPoints[enemyResource.PathIdToSpawnOn].AddChild(enemy);
    }
}