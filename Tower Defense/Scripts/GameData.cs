using WyrmspireStudios.Events;

namespace WyrmspireStudios.Data;

public static class GameData
{
    private static int Shards { get; set; }
    private static int EnemyDeaths { get; set; }
    
    public static event EventHandlers.ValueChangeHandler ShardsChanged;
    
    public static int GetShards()
    {
        return Shards;
    }

    public static void SetShards(int amount)
    {
        var oldShards = Shards;
        Shards = amount;
        ShardsChanged?.Invoke(oldShards, Shards);
    }

    private static void ResetShards()
    {
        Shards = 0;
    }
    
    public static void AddShards(int amount)
    {
        var oldShards = Shards;
        Shards += amount;
        ShardsChanged?.Invoke(oldShards, Shards);
    }

    public static void RemoveShards(int amount)
    {
        var oldShards = Shards;
        Shards -= amount;
        ShardsChanged?.Invoke(oldShards, Shards);
    }

    public static void AddEnemyDeath()
    {
        var oldEnemyDeaths = EnemyDeaths;
        EnemyDeaths += 1;
    }
    
    private static void ResetEnemyDeaths()
    {
        EnemyDeaths = 0;
    }

    private static void ResetEvents()
    {
        ShardsChanged = null;
    }

    public static void ResetGameData()
    {
        ResetShards();
        ResetEnemyDeaths();
        ResetEvents();
    }
}