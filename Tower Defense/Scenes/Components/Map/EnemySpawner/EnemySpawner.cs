using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using WyrmspireStudios.Data;

namespace WyrmspireStudios;

public partial class EnemySpawner : Node2D
{
    public delegate void EnemiesDeadHandler();
    public static event EnemiesDeadHandler EnemiesDead;
    
    [Export] public Array<Path2D> SpawnPoints;
    [Export] public Array<EnemyWave> Waves;

    private bool _finishedSpawning;
    private bool _enemiesDead;
    
    public override void _Ready()
    {
        LevelData.ResetCurrentWave();
        LevelData.SetMaxWave(Waves.Count);
        _ = SpawnWave();
    }

    public override void _Process(double delta)
    {
        if (!_finishedSpawning || _enemiesDead) return;
        if (SpawnPoints.Select(path => path.GetChildCount()).Sum() == 0)
        {
            _enemiesDead = true;
            if (LevelData.GetHealth() > 0) EnemiesDead?.Invoke();
        }
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
            _finishedSpawning = true;
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