namespace WyrmspireStudios;

public static class GameData
{
    public delegate void ValueChangeHandler(int oldValue, int newValue);

    public static event ValueChangeHandler HealthChanged;
    public static event ValueChangeHandler GoldChanged;
    public static event ValueChangeHandler ShardsChanged;
    public static event ValueChangeHandler EnemyDeath;

    private static int Health { get; set; } = 100;
    private static int Gold { get; set; } = 100;
    private static int Shards { get; set; }
    private static int EnemyDeaths { get; set; }

    public static int GetHealth()
    {
        return Health;
    }

    public static void SetHealth(int amount)
    {
        var oldHealth = Health;
        Health = amount;
        HealthChanged?.Invoke(oldHealth, Health);
    }
    
    public static void AddHealth(int amount)
    {
        var oldHealth = Health;
        Health += amount;
        HealthChanged?.Invoke(oldHealth, Health);
    }

    public static void RemoveHealth(int amount)
    {
        var oldHealth = Health;
        Health -= amount;
        HealthChanged?.Invoke(oldHealth, Health);
    }

    public static int GetGold()
    {
        return Gold;
    }

    public static void SetGold(int amount)
    {
        var oldGold = Gold;
        Gold = amount;
        GoldChanged?.Invoke(oldGold, Gold);
    }
    
    public static void AddGold(int amount)
    {
        var oldGold = Gold;
        Gold += amount;
        GoldChanged?.Invoke(oldGold, Gold);
    }

    public static void RemoveGold(int amount)
    {
        var oldGold = Gold;
        Gold -= amount;
        GoldChanged?.Invoke(oldGold, Gold);
    }
    
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
        EnemyDeath?.Invoke(oldEnemyDeaths, EnemyDeaths);
    }
}