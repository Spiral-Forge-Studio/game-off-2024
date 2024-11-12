using UnityEngine;

public class MinigunMagazineBuff : Buff
{
    public BuffType buffType;
    public Rarity rarity;

    public float initialAmount;
    public float consecutiveAmount;
    public float scalingFactor;

    private float totalFlatBonus;    // Tracks total flat bonus to remove
    private float totalMultiplier;   // Tracks total multiplier bonus to remove

    private PlayerStatusSO playerStatus;

    public MinigunMagazineBuff(PlayerStatusSO status, float duration, BuffType buffType, Rarity rarity, float initialAmount, float consecutiveAmount, float scalingFactor)
        : base(duration)
    {
        this.playerStatus = status;
        this.buffType = buffType;
        this.rarity = rarity;
        this.initialAmount = initialAmount;
        this.consecutiveAmount = consecutiveAmount;
        this.scalingFactor = scalingFactor;
    }

    public void Initialize(PlayerStatusSO playerStats, float amount, MinigunMagazineBuff.BuffType type, MinigunMagazineBuff.Rarity rarity, float initialAmount, float consecutiveAmount, float scaling)
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
            // Apply a flat bonus to MinigunMagazineSize
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, initialAmount);
            totalFlatBonus += initialAmount;
        }
        else
        {
            // Apply a multiplier to MinigunMagazineSize
            float multiplierValue = initialAmount;  // Assuming this is in percentage terms
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.MinigunMagazineSizeMultiplier, multiplierValue, true);
            totalMultiplier += multiplierValue / 100f;
        }

        // Set up consecutive bonus application if applicable
        if (duration > 0)
        {
            InvokeRepeating(nameof(ApplyConsecutiveBuff), 1f, duration);
        }
    }

    private void ApplyConsecutiveBuff()
    {
        float bonusAmount = consecutiveAmount * (1 + scalingFactor);

        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, bonusAmount);
            totalFlatBonus += bonusAmount;
        }
        else
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.MinigunMagazineSizeMultiplier, bonusAmount, true);
            totalMultiplier += bonusAmount / 100f;
        }
    }

    public override void RemoveBuff(GameObject target)
    {
        // Remove the accumulated bonuses
        if (buffType == BuffType.Flat)
        {
            playerStatus.ModifyFlatBonus(EStatTypeFlatBonus.MinigunMagazineSizeFlatBonus, -totalFlatBonus);
        }
        else
        {
            playerStatus.ModifyMultiplier(EStatTypeMultiplier.MinigunMagazineSizeMultiplier, -totalMultiplier * 100f, false);
        }

        CancelInvoke(nameof(ApplyConsecutiveBuff));
    }

    public override void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float InitAmount = 0, float ConsecAmount = 0, float ScaleAmount = 0)
    {
        this.buffType = bufftype;
        this.rarity = buffrarity;
        this.initialAmount = InitAmount;
        this.consecutiveAmount = ConsecAmount;
        this.scalingFactor = ScaleAmount;

    }
}
