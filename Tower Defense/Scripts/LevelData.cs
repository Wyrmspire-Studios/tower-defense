using System.Collections.Generic;
using Godot;
using WyrmspireStudios.Events;

namespace WyrmspireStudios.Data;

public static class LevelData
{
    private const int DefaultHealth = 25;
    private const int DefaultGold = 150;
    private const int DefaultCardCost = 50;
    private static int Health { get; set; } = DefaultHealth;
    private static int Gold { get; set; } = DefaultGold;
    private static int EnemyDeaths { get; set; }
    
    private static int CurrentWave { get; set; }
    private static int MaxWave { get; set; }
    private static int CardCost { get; set; } = DefaultCardCost;
    private static List<Card> Cards { get; set; } = new List<Card>();
    
    public static event EventHandlers.ValueChangeHandler HealthChanged;
    public static event EventHandlers.ValueChangeHandler GoldChanged;
    public static event EventHandlers.ValueChangeHandler EnemyDeath;
    public static event EventHandlers.ValueChangeHandler CurrentWaveChanged;
    public static event EventHandlers.ValueChangeHandler MaxWaveChanged;
    public static event EventHandlers.ValueChangeHandler CardCostChanged;
    
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
    
    private static void ResetHealth()
    {
        SetHealth(DefaultHealth);
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

    private static void ResetGold()
    {
        SetGold(DefaultGold);
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
    
    public static void AddEnemyDeath()
    {
        var oldEnemyDeaths = EnemyDeaths;
        EnemyDeaths += 1;
        GameData.AddEnemyDeath();
        EnemyDeath?.Invoke(oldEnemyDeaths, EnemyDeaths);
    }

    private static void ResetEnemyDeaths()
    {
        EnemyDeaths = 0;
    }

    public static int GetCurrentWave()
    {
        return CurrentWave;
    }

    public static void NextWave()
    {
        int oldCurrentWave = CurrentWave;
        CurrentWave++;
        CurrentWaveChanged?.Invoke(oldCurrentWave, CurrentWave);
    }

    public static void ResetCurrentWave()
    {
        CurrentWaveChanged?.Invoke(CurrentWave, 0);
        CurrentWave = 0;
    }

    public static int GetMaxWave()
    {
        return MaxWave;
    }
    
    public static void SetMaxWave(int maxWave)
    {
        int oldMaxWave = MaxWave;
        MaxWave = maxWave;
        MaxWaveChanged?.Invoke(oldMaxWave, MaxWave);
    }

    private static void ResetMaxWave()
    {
        MaxWaveChanged?.Invoke(MaxWave, 0);
        MaxWave = 0;
    }

    public static int GetCardCost()
    {
        return CardCost;
    }

    public static void AddCardCost(int amount)
    {
        int oldCardCost = CardCost;
        CardCost += Mathf.FloorToInt(amount / GameData.GetModifier("next_card_price_modifier"));
        CardCostChanged?.Invoke(oldCardCost, CardCost);
    }

    private static void ResetCardCost()
    {
        CardCost = Mathf.FloorToInt(DefaultCardCost / GameData.GetModifier("next_card_price_modifier"));
    }
    
    private static void ResetEvents()
    {
        HealthChanged = null;
        GoldChanged = null;
        EnemyDeath = null;
        CurrentWaveChanged = null;
        MaxWaveChanged = null;
        CardCostChanged = null;
    }
    
    public static void ResetLevelData()
    {
        ResetHealth();
        ResetGold();
        ResetEnemyDeaths();
        ResetCurrentWave();
        ResetMaxWave();
        ResetCardCost();
        ResetEvents();
    }
}