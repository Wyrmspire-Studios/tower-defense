namespace WyrmspireStudios;

public static class GameData
{
    public delegate void ValueChangeHandler(int oldValue, int newValue);
    public static event ValueChangeHandler HealthChanged;
    public static event ValueChangeHandler GoldChanged;
    public static event ValueChangeHandler ShardsChanged;
    
    public static int Health { get; private set; } = 100;
    public static int Gold { get; private set; }
    public static int Shards {get; private set;}

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
}
