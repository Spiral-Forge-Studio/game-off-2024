using System.Collections.Generic;
using UnityEngine;

public class RocketReloadTimeBuff : Buff
{
    public BuffType buffType;
    public Rarity rarity;
    public string buffname = "Quick Loader Rocket";

    public float initialAmountFlat = -0.1f;
    public float initialAmountMultiplier = -5.0f;
    public float consecutiveAmountFlat = -0.05f;
    public float consecutiveAmountMultiplier = -2.5f;
    public float scalingFactor;

    private float totalFlatBonus;
    private float totalMultiplier;

    private PlayerStatusSO playerStatus;

    public Dictionary<BuffType, float> bufftypeProbabilities = new Dictionary<BuffType, float>
    {
        { BuffType.Percentage, 0.4f },
        { BuffType.Flat, 0.6f }
    };

    public Dictionary<Rarity, float> rarityProbabilities = new Dictionary<Rarity, float>
    {
        { Rarity.Legendary, 0.05f },
        { Rarity.Epic, 0.1f },
        { Rarity.Rare, 0.15f },
        { Rarity.Uncommon, 0.25f },
        { Rarity.Common, 0.45f }
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

    public override string getBuffName() => buffname;

    public override BuffType getBuffType() => buffType;

    public override Rarity getBuffRarity() => rarity;

    public override Rarity getRandomRarity()
    {
        float randomValue = Random.value;
        float cumulative = 0;
        foreach (var entry in rarityProbabilities)
        {
            cumulative += entry.Value;
            if (randomValue <= cumulative)
            {
                return entry.Key;
            }
        }
        return Rarity.Common;
    }

    public override BuffType getRandomType()
    {
        float randomValue = Random.value;
        float cumulative = 0;
        foreach (var entry in bufftypeProbabilities)
        {
            cumulative += entry.Value;
            if (randomValue <= cumulative)
            {
                return entry.Key;
            }
        }
        return BuffType.Flat;
    }

    public override float getBuffBonus()
    {
        return buffType == BuffType.Flat ? totalFlatBonus : totalMultiplier * 100f;
    }

    public override void ApplyBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.RocketReloadTimeFlatBonus, initialAmountFlat);
            totalFlatBonus += initialAmountFlat;
        }
        else if (buffType == BuffType.Percentage)
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.RocketReloadTimeMultiplier, initialAmountMultiplier, true);
            totalMultiplier += initialAmountMultiplier / 100f;
        }
    }

    public override void ApplyConsecutiveBuff()
    {
        if (buffType == BuffType.Flat)
        {
            float bonusAmount = consecutiveAmountFlat * (1 + scalingFactor);
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.RocketReloadTimeFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        else if (buffType == BuffType.Percentage)
        {
            float bonusAmount = consecutiveAmountMultiplier * (1 + scalingFactor);
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.RocketReloadTimeMultiplier, bonusAmount, true);
            totalMultiplier += bonusAmount / 100f;
        }
    }

    public override void RemoveBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.RocketReloadTimeFlatBonus, -totalFlatBonus);
        }
        else if (buffType == BuffType.Percentage)
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.RocketReloadTimeMultiplier, -totalMultiplier * 100f, false);
        }
    }

    public override void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float initialAmount = 0, float consecutiveAmount = 0, float ScaleAmount = 0)
    {
        this.buffType = bufftype;
        this.rarity = buffrarity;
        if (buffType == BuffType.Flat)
        {
            this.initialAmountFlat *= rarityMultiplier[rarity];
            this.consecutiveAmountFlat *= rarityMultiplier[rarity];
        }
        else if (buffType == BuffType.Percentage)
        {
            this.initialAmountMultiplier *= rarityMultiplier[rarity];
            this.consecutiveAmountMultiplier *= rarityMultiplier[rarity];
        }
        this.scalingFactor = ScaleAmount;
    }
}