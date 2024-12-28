using System.Collections.Generic;
using UnityEngine;

public class RocketCritDamageBuff : Buff
{
    public BuffType buffType;
    public Rarity rarity;
    public string buffname = "Rocket Crit Damage Buff";

    public float initialAmountFlat = 0.5f; // Reduced deviation (negative value for reduction)
    public float initialAmountMultiplier = 40f; // Percentage-based reduction
    public float consecutiveAmountFlat = 0.01f;
    public float consecutiveAmountMultiplier = 5f;
    public float scalingFactor;

    private float totalFlatReduction;
    private float totalMultiplierReduction;

    private PlayerStatusSO playerStatus;

    public Dictionary<BuffType, float> bufftypeProbabilities = new Dictionary<BuffType, float>
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

    public override string getBuffName()
    {
        return buffname;
    }

    public override BuffType getBuffType()
    {
        return buffType;
    }

    public override Rarity getBuffRarity()
    {
        return rarity;
    }

    public override Rarity getRandomRarity()
    {
        float randomValue = UnityEngine.Random.value;
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
        float randomValue = UnityEngine.Random.value;
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
        if (buffType == BuffType.Flat)
        {
            return totalFlatReduction;
        }
        else
        {
            return totalMultiplierReduction * 100f;
        }
    }

    public override void ApplyBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.RocketCritDamageFlatBonus, initialAmountFlat);
            totalFlatReduction += initialAmountFlat;
        }
        if (buffType == BuffType.Percentage)
        {
            float multiplierValue = initialAmountMultiplier;
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.RocketCritDamageMultiplier, multiplierValue, true);
            totalMultiplierReduction += multiplierValue;
        }
    }

    public override void ApplyConsecutiveBuff()
    {
        if (buffType == BuffType.Flat)
        {
            float reductionAmount = consecutiveAmountFlat * (1 + scalingFactor);
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.RocketCritDamageFlatBonus, reductionAmount);
            totalFlatReduction += reductionAmount;
        }
        if (buffType == BuffType.Percentage)
        {
            float reductionAmount = consecutiveAmountMultiplier * (1 + scalingFactor);
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.RocketCritDamageMultiplier, reductionAmount, true);
            totalMultiplierReduction += reductionAmount;
        }
    }

    public override void RemoveBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.RocketCritDamageFlatBonus, -totalFlatReduction);
        }
        if (buffType == BuffType.Percentage)
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.RocketCritDamageMultiplier, -totalMultiplierReduction, true);
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
        if (buffType == BuffType.Percentage)
        {
            this.initialAmountMultiplier *= rarityMultiplier[rarity];
            this.consecutiveAmountMultiplier *= rarityMultiplier[rarity];
        }
        this.scalingFactor = ScaleAmount;
    }
}
