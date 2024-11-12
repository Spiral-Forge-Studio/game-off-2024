using UnityEngine;

public class ShieldBuff : Buff
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

    public ShieldBuff(PlayerStatusSO status, float duration, BuffType buffType, Rarity rarity, float initialAmount, float consecutiveAmount, float scalingFactor)
        : base(duration)
    {
        this.playerStatus = status;
        this.buffType = buffType;
        this.rarity = rarity;
        this.initialAmount = initialAmount;
        this.consecutiveAmount = consecutiveAmount;
        this.scalingFactor = scalingFactor;
    }

    public void Initialize(PlayerStatusSO playerStats, float amount, ShieldBuff.BuffType type, ShieldBuff.Rarity rarity, float initialAmount, float consecutiveAmount, float scaling)
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
            // Apply a flat bonus to shield
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.ShieldFlatBonus, initialAmount);
            totalFlatBonus += initialAmount;
        }
        else
        {
            // Apply a multiplier to shield
            float multiplierValue = initialAmount;
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.ShieldMultiplier, multiplierValue, true);
            totalMultiplier += multiplierValue / 100f;
        }
    }

    private void ApplyConsecutiveBuff()
    {
        float bonusAmount = consecutiveAmount * (1 + scalingFactor);

        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.ShieldFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        else
        {
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
        else
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.ShieldMultiplier, -totalMultiplier * 100f, false);
        }

        CancelInvoke(nameof(ApplyConsecutiveBuff));
    }

    public override void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float InitAmount = 0, float ConsecAmount = 0, float ScaleAmount = 0)
    {

    }
}
