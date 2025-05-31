using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using WyrmspireStudios.Data;

namespace WyrmspireStudios;

public partial class EnemySpawner : Node2D
{
    [Export] public Array<Path2D> SpawnPoints;
    [Export] public Array<EnemyWave> Waves;
    public override void _Ready()
    {
        LevelData.ResetCurrentWave();
        LevelData.SetMaxWave(Waves.Count);
        _ = SpawnWave();
    }

    private async Task SpawnWave()
    {
        int currentWave = LevelData.GetCurrentWave();

        EnemyWave wave = Waves[currentWave];
        foreach (var group in Waves[currentWave].EnemyGroups)
        {
            _ = SpawnGroup(group);
            await ToSignal(GetTree().CreateTimer(group.DelayUntilNextGroup), "timeout");
        }

        if (currentWave >= LevelData.GetMaxWave() - 1)
        {
            GD.Print("Waves finished.");
        }
        else
        {
            await ToSignal(GetTree().CreateTimer(wave.DelayUntilNextWave), "timeout");
            LevelData.NextWave();
            await SpawnWave();
        }
    }

    private async Task SpawnGroup(EnemyGroup enemyGroup)
    {
        for (int enemyIndex = 0; enemyIndex < enemyGroup.EnemyCount; enemyIndex++)
        {
            SpawnEnemy(enemyGroup.EnemyToSpawn, enemyGroup.PathIdToSpawnOn);
            await ToSignal(GetTree().CreateTimer(enemyGroup.SpawnIntervalSeconds), "timeout");
        }
    }
    
    private void SpawnEnemy(PackedScene enemyScene, int pathIdToSpawnOn)
    {
        var enemy = (PathFollow2D)enemyScene.Instantiate();
        SpawnPoints[pathIdToSpawnOn].AddChild(enemy);
    }
}