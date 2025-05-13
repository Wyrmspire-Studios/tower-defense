using Godot;

namespace WyrmspireStudios;

public static class GameData
{
    public static int Health { get; private set; } = 100;
    public static int Gold { get; private set; } = 0;
    public static int Shards {get; private set;} = 0;

    public static void AddHealth(int amount)
    {
        Health += amount;
    }
    
    public static void RemoveHealth(int amount)
    {
        Health -= amount;
    }
    
    public static void AddGold(int amount)
    {
        Gold += amount;
    }
    
    public static void RemoveGold(int amount)
    {
        Gold -= amount;
    }
    
    public static void AddShards(int amount)
    {
        Shards += amount;
    }
    
    public static void RemoveShards(int amount)
    {
        Shards -= amount;
    }
}
