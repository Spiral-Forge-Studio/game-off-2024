using System.Collections.Generic;
using UnityEngine;

public class MinigunFireRateBuff : Buff
{
    public BuffType buffType;
    public Rarity rarity;
    public string buffName = "Rapid Barrel Upgrade";

    public float initialAmountFlat = 0.8f;
    public float initialAmountMultiplier = 15f;
    public float consecutiveAmountFlat = 0.25f;
    public float consecutiveAmountMultiplier = 0.075f;
    public float scalingFactor;

    private float totalFlatBonus;    // Tracks total flat bonus to remove
    private float totalMultiplier;   // Tracks total multiplier bonus to remove

    private PlayerStatusSO playerStatus;

    public Dictionary<BuffType, float> buffTypeProbabilities = new Dictionary<BuffType, float>
    {
        { BuffType.Percentage, 0.5f },
        { BuffType.Flat, 0.5f }
    };

    public Dictionary<Rarity, float> rarityProbabilities = new Dictionary<Rarity, float>
    {
        { Rarity.Legendary, 0.05f },
        { Rarity.Epic, 0.1f },
        { Rarity.Rare, 0.2f },
        { Rarity.Uncommon, 0.3f },
        { Rarity.Common, 0.35f }
    };

    private Dictionary<Rarity, float> rarityMultiplier = new Dictionary<Rarity, float>
    {
        { Rarity.Common, 1.0f },
        { Rarity.Uncommon, 1.5f },
        { Rarity.Rare, 2.0f },
        { Rarity.Epic, 3.0f },
        { Rarity.Legendary, 5.0f }
    };

    public override void Initialize(PlayerStatusSO playerStatus, float initAmount, BuffType type, Rarity rarity, float consecAmount, float scaleAmount, float duration)
    {
        // Set properties here
        this.playerStatus = playerStatus;
        this.buffType = type;
        if (buffType == BuffType.Flat)
        {
            this.initialAmountFlat *= rarityMultiplier[rarity];
            this.consecutiveAmountFlat *= rarityMultiplier[rarity];
        }
        if (buffType == BuffType.Percentage)
        {
            this.initialAmountMultiplier *= rarityMultiplier[rarity];
            this.consecutiveAmountMultiplier *= rarityMultiplier[rarity];
        }
        this.rarity = rarity;
        this.scalingFactor = scaleAmount;
        this.duration = duration;
    }

    public override string getBuffName() => buffName;

    public override BuffType getBuffType() => buffType;

    public override Rarity getBuffRarity() => rarity;

    public override Rarity getRandomRarity()
    {
        float randomValue = UnityEngine.Random.value;
        float cumulative = 0;
        foreach (var entry in rarityProbabilities)
        {
            cumulative += entry.Value;
            if (randomValue <= cumulative)
                return entry.Key;
        }
        return Rarity.Common;
    }

    public override BuffType getRandomType()
    {
        float randomValue = UnityEngine.Random.value;
        float cumulative = 0;
        foreach (var entry in buffTypeProbabilities)
        {
            cumulative += entry.Value;
            if (randomValue <= cumulative)
                return entry.Key;
        }
        return BuffType.Flat;
    }

    public override float getBuffBonus() => buffType == BuffType.Flat ? totalFlatBonus : totalMultiplier * 100f;

    public override void ApplyBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.MinigunFireRateFlatBonus, initialAmountFlat);
            totalFlatBonus += initialAmountFlat;
        }
        if (buffType == BuffType.Percentage)
        {
            float multiplierValue = initialAmountMultiplier;
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.MinigunFireRateMultiplier, multiplierValue, true);
            totalMultiplier += multiplierValue;
        }
    }

    public override void ApplyConsecutiveBuff()
    {
        if (buffType == BuffType.Flat)
        {
            float bonusAmount = consecutiveAmountFlat * (1 + scalingFactor);
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.MinigunFireRateFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        if (buffType == BuffType.Percentage)
        {
            float bonusAmount = consecutiveAmountMultiplier * (1 + scalingFactor);
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.MinigunFireRateMultiplier, bonusAmount, true);
            totalMultiplier += bonusAmount;
        }
    }

    public override void RemoveBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.MinigunFireRateFlatBonus, -totalFlatBonus);

        if (buffType == BuffType.Percentage)
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.MinigunFireRateMultiplier, -totalMultiplier, true);
    }

    public override void UpdateBuffValues(Buff.BuffType buffType, Buff.Rarity rarity, float initialAmount = 0, float consecutiveAmount = 0, float scalingFactor = 0)
    {
        this.buffType = buffType;
        this.rarity = rarity;

        if (buffType == BuffType.Flat)
        {
            this.initialAmountFlat *= rarityMultiplier[rarity];
            this.consecutiveAmountFlat *= rarityMultiplier[rarity];
        }
        if (buffType == BuffType.Percentage)
        {
            this.initialAmountMultiplier *= rarityMultiplier[rarity];
            this.consecutiveAmountMultiplier *= rarityMultiplier[rarity];
        }

        this.scalingFactor = scalingFactor;
    }
}
