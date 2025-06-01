using System;
using System.Collections.Generic;
using WyrmspireStudios.Events;

namespace WyrmspireStudios.Data;

public static class GameData
{
    private static int Shards { get; set; } = 1000;
    private static int EnemyDeaths { get; set; }
    
    private static readonly Dictionary<string, int> Upgrades = new();
    private const float DefaultStartingCards = 5f;
    private const float DefaultDamageModifier = 1f;
    private const float DefaultUpgradePriceModifier = 1f;
    private const float DefaultNextCardPriceModifier = 1f;
    private const float DefaultCooldownModifier = 1f;
    private static float StartingCards { get; set; } = DefaultStartingCards;
    private static float DamageModifier { get; set; } = DefaultDamageModifier;
    private static float UpgradePriceModifier { get; set; } = DefaultUpgradePriceModifier;
    private static float NextCardPriceModifier { get; set; } = DefaultNextCardPriceModifier;
    private static float CooldownModifier { get; set; } = DefaultCooldownModifier;
    
    
    
    public static event EventHandlers.UpgradesChangesHandler UpgradesChanged;
    
    public static int GetUpgradeBoughtTimes(string id)
    {
        return Upgrades.GetValueOrDefault(id, 0);
    }

    /// <summary>
    /// Retrieves the value of a specific gameplay modifier by its string ID.
    /// </summary>
    /// <param name="id">
    /// The ID of the modifier to retrieve. Valid values are:
    /// <list type="bullet">
    ///   <item>
    ///     <term>"starting_cards"</term>
    ///     <description>Number of starting cards given to the player.</description>
    ///   </item>
    ///   <item>
    ///     <term>"damage_modifier"</term>
    ///     <description>Multiplier applied to damage dealt by all towers.</description>
    ///   </item>
    ///   <item>
    ///     <term>"upgrade_price_modifier"</term>
    ///     <description>Modifier applied to the cost of tower upgrades.</description>
    ///   </item>
    ///   <item>
    ///     <term>"next_card_price_modifier"</term>
    ///     <description>Modifier applied to the cost of the next card in a level.</description>
    ///   </item>
    ///   <item>
    ///     <term>"cooldown_modifier"</term>
    ///     <description>Multiplier that affects cooldown times of towers.</description>
    ///   </item>
    /// </list>
    /// </param>
    /// <returns>The float value of the corresponding modifier.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if an unknown modifier ID is passed.
    /// </exception>
    public static float GetModifier(string id)
    {
        return id switch
        {
            "starting_cards" => StartingCards,
            "damage_modifier" => DamageModifier,
            "upgrade_price_modifier" => UpgradePriceModifier,
            "next_card_price_modifier" => NextCardPriceModifier,
            "cooldown_modifier" => CooldownModifier,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }

    private static void UpgradeModifier(string id, float upgradeModifier)
    {
        switch (id)
        {
            case "starting_cards":
                StartingCards += upgradeModifier;
                break;
            case "damage_modifier":
                DamageModifier += upgradeModifier;
                break;
            case "upgrade_price_modifier":
                UpgradePriceModifier += upgradeModifier;
                break;
            case "next_card_price_modifier":
                NextCardPriceModifier += upgradeModifier;
                break;
            case "cooldown_modifier":
                CooldownModifier += upgradeModifier;
                break;
        }
    }

    private static void ResetModifiers()
    { 
        StartingCards = DefaultStartingCards; 
        DamageModifier = DefaultDamageModifier; 
        UpgradePriceModifier = DefaultUpgradePriceModifier; 
        NextCardPriceModifier = DefaultNextCardPriceModifier; 
        CooldownModifier = DefaultCooldownModifier;
    }
    
    public static void BoughtUpgrade(string id, float upgradeModifier)
    {
        UpgradeModifier(id, upgradeModifier);
        int level = GetUpgradeBoughtTimes(id) + 1;
        UpgradesChanged?.Invoke(id, level, GetModifier(id));
        Upgrades[id] = level;
    }

    private static void ResetUpgrades()
    {
        Upgrades.Clear();
    }
    
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
        ResetModifiers();
        ResetUpgrades();
    }
}