using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class HpBuff : Buff
{
    public BuffType buffType;
    public Rarity rarity;
    public string buffname = "Steel - Plated Armor";

    public float initialAmountFlat = 50f;
    public float initialAmountMultiplier = 30f;
    public float consecutiveAmountFlat = 25f;
    public float consecutiveAmountMultiplier = 5f;
    public float scalingFactor;

    private float totalFlatBonus;    // Tracks total flat bonus to remove
    private float totalMultiplier;   // Tracks total multiplier bonus to remove

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



    public HpBuff(PlayerStatusSO status, float duration, BuffType buffType, Rarity rarity, float initialAmount, float consecutiveAmount, float scalingFactor)
        : base(duration)
    {
        this.playerStatus = status;
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
        if(buffType == BuffType.Flat)
        {
            return totalFlatBonus;
        }
        else
        {
            return totalMultiplier*100f;
        }
    }

    public override void ApplyBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            // Apply a flat bonus to health
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.HealthFlatBonus, initialAmountFlat);
            totalFlatBonus += initialAmountFlat;
        }
        if (buffType == BuffType.Percentage)
        {
            // Apply a multiplier to health
            float multiplierValue = initialAmountMultiplier;  // Assuming this is in percentage terms
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.HealthMultiplier, multiplierValue, true);
            totalMultiplier += multiplierValue / 100f;
        }

        // Set up consecutive bonus application if applicable
        if (duration > 0)
        {
            InvokeRepeating(nameof(ApplyConsecutiveBuff), 1f, duration);
        }
    }

    public override void ApplyConsecutiveBuff()
    {
        

        if (buffType == BuffType.Flat)
        {
            float bonusAmount = consecutiveAmountFlat * (1 + scalingFactor);
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.HealthFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        if (buffType == BuffType.Percentage)
        {
            float bonusAmount = consecutiveAmountMultiplier * (1 + scalingFactor);
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.HealthMultiplier, bonusAmount, true);
            totalMultiplier += bonusAmount / 100f;
        }
    }

    public override void RemoveBuff(GameObject target)
    {
        // Remove the accumulated bonuses
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.HealthFlatBonus, -totalFlatBonus);
        }
        if (buffType == BuffType.Percentage)
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.HealthMultiplier, -totalMultiplier * 100f, false);
        }

        CancelInvoke(nameof(ApplyConsecutiveBuff));
    }

    public override void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float initialAmount = 0, float consecutiveAmount = 0, float ScaleAmount = 0)
    {
        this.buffType = bufftype;
        this.rarity = buffrarity;
        if (buffType == BuffType.Flat)
        {
            //this.initialAmountFlat = initialAmount * rarityMultiplier[rarity];
            this.initialAmountFlat*=rarityMultiplier[rarity];
            //this.consecutiveAmountFlat = consecutiveAmount * rarityMultiplier[rarity];
            this.consecutiveAmountFlat*=rarityMultiplier[rarity];
        }
        if (buffType == BuffType.Percentage)
        {
            //this.initialAmountMultiplier = initialAmount * rarityMultiplier[rarity];
            this.initialAmountMultiplier*=rarityMultiplier[rarity];
            //this.consecutiveAmountMultiplier = consecutiveAmount * rarityMultiplier[rarity];
            this.consecutiveAmountMultiplier*=rarityMultiplier[rarity];
        }
        this.scalingFactor = ScaleAmount;
    }

}