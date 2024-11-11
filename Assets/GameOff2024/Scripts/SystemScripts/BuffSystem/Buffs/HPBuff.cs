using UnityEngine;

public class HpBuff : Buff
{
    public enum BuffType { Flat, Percentage }
    public enum Rarity { Common, Rare, Epic, Legendary }

    public BuffType buffType;
    public Rarity rarity;

    public float initialAmount;
    public float consecutiveAmount;
    public float scalingFactor;

    private float totalFlatBonus;    // Tracks total flat bonus to remove
    private float totalMultiplier;   // Tracks total multiplier bonus to remove

    private PlayerStatusSO playerStatus;

    public HpBuff(PlayerStatusSO status, float duration, BuffType buffType, Rarity rarity, float initialAmount, float consecutiveAmount, float scalingFactor)
        : base(duration)
    {
        this.playerStatus = status;
        this.buffType = buffType;
        this.rarity = rarity;
        this.initialAmount = initialAmount;
        this.consecutiveAmount = consecutiveAmount;
        this.scalingFactor = scalingFactor;
    }

    public void Initialize(PlayerStatusSO playerStats, float amount, HpBuff.BuffType type, HpBuff.Rarity rarity, float initialAmount, float consecutiveAmount, float scaling)
    {
        this.playerStatus = playerStats; // Ensure playerStatus is set here
        this.buffType = type;
        this.rarity = rarity;
        this.initialAmount = initialAmount;
        this.consecutiveAmount = consecutiveAmount;
        this.scalingFactor = scaling;
    }

    public override void ApplyBuff(GameObject target)
    {
        if (buffType == BuffType.Flat)
        {
            // Apply a flat bonus to health
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.HealthFlatBonus, initialAmount);
            totalFlatBonus += initialAmount;
        }
        else
        {
            // Apply a multiplier to health
            float multiplierValue = initialAmount;  // Assuming this is in percentage terms
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.HealthMultiplier, multiplierValue, true);
            totalMultiplier += multiplierValue / 100f;
        }

        // Set up consecutive bonus application if applicable
        //InvokeRepeating(nameof(ApplyConsecutiveBuff), 1f, duration);
    }

    private void ApplyConsecutiveBuff()
    {
        float bonusAmount = consecutiveAmount * (1 + scalingFactor);

        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.HealthFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        else
        {
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
        else
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.HealthMultiplier, -totalMultiplier * 100f, false);
        }

        CancelInvoke(nameof(ApplyConsecutiveBuff));
    }
}
