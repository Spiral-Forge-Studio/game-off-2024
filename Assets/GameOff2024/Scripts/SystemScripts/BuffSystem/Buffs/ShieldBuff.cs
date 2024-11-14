using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : Buff
{
    public BuffType buffType;
    public Rarity rarity;
    public string buffname = "Reinforced Shields";

    public float initialAmountFlat = 100f;
    public float initialAmountMultiplier = 1.1f;
    public float consecutiveAmountFlat = 50f;
    public float consecutiveAmountMultiplier = 1.05f;
    public float scalingFactor;

    private float totalFlatBonus;    // Tracks total flat bonus to remove
    private float totalMultiplier;   // Tracks total multiplier bonus to remove

    private PlayerStatusSO playerStatus;

    private Dictionary<Rarity, float> rarityProbabilities = new Dictionary<Rarity, float>
    {
        { Rarity.Common, 0.45f },
        { Rarity.Uncommon, 0.25f },
        { Rarity.Rare, 0.15f },
        { Rarity.Epic, 0.1f },
        { Rarity.Legendary, 0.05f }
    };

    private Dictionary<Rarity, float> rarityMultiplier = new Dictionary<Rarity, float>
    {
        { Rarity.Common, 1.0f },
        { Rarity.Uncommon, 1.5f },
        { Rarity.Rare, 2.0f },
        { Rarity.Epic, 3.0f },
        { Rarity.Legendary, 5.0f }
    };

    public ShieldBuff(PlayerStatusSO status, float duration, BuffType buffType, Rarity rarity, float initialAmount, float consecutiveAmount, float scalingFactor)
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
    public override float getBuffBonus()
    {
        if (buffType == BuffType.Flat)
        {
            return totalFlatBonus;
        }
        else
        {
            return totalMultiplier;
        }
    }

    public override void ApplyBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.ShieldFlatBonus, initialAmountFlat);
            totalFlatBonus += initialAmountFlat;
        }
        if (buffType == BuffType.Percentage)
        {
            float multiplierValue = initialAmountMultiplier;
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.ShieldMultiplier, multiplierValue, true);
            totalMultiplier += multiplierValue / 100f;
        }

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
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.ShieldFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        if (buffType == BuffType.Percentage)
        {
            float bonusAmount = consecutiveAmountMultiplier * (1 + scalingFactor);
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.ShieldMultiplier, bonusAmount, true);
            totalMultiplier += bonusAmount / 100f;
        }
    }

    public override void RemoveBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.ShieldFlatBonus, -totalFlatBonus);
        }
        if (buffType == BuffType.Percentage)
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.ShieldMultiplier, -totalMultiplier * 100f, false);
        }

        CancelInvoke(nameof(ApplyConsecutiveBuff));
    }

    public override void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float initialAmount = 0, float consecutiveAmount = 0, float ScaleAmount = 0)
    {
        this.buffType = bufftype;
        this.rarity = buffrarity;
        if (buffType == BuffType.Flat)
        {
            this.initialAmountFlat = initialAmount * rarityMultiplier[rarity];
            this.consecutiveAmountFlat = consecutiveAmount * rarityMultiplier[rarity];
        }
        if (buffType == BuffType.Percentage)
        {
            this.initialAmountMultiplier = initialAmount * rarityMultiplier[rarity];
            this.consecutiveAmountMultiplier = consecutiveAmount * rarityMultiplier[rarity];
        }
        this.scalingFactor = ScaleAmount;
    }
}
