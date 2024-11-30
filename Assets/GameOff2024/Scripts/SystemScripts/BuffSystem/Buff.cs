using UnityEngine;
using static Buff;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buffs/BaseBuff")]
public abstract class Buff : ScriptableObject
{
    public enum BuffType { Flat, Percentage, Unique }
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }

    public float duration; // Duration of the buff
    protected bool isActive = false; // Tracks if the buff is active

    // Abstract methods that child classes must implement
    public abstract void Initialize(PlayerStatusSO playerStatus, float initAmount, BuffType type, Rarity rarity, float consecAmount, float scaleAmount, float duration);
    public abstract void ApplyBuff(GameObject target);
    public abstract void RemoveBuff(GameObject target);
    public abstract void ApplyConsecutiveBuff();
    public abstract void UpdateBuffValues(BuffType bufftype, Rarity buffrarity, float InitAmount = 0, float ConsecAmount = 0, float ScaleAmount = 0);
    public abstract string getBuffName();
    public abstract BuffType getBuffType();
    public abstract Rarity getBuffRarity();
    public abstract Rarity getRandomRarity();
    public abstract BuffType getRandomType();
    public abstract float getBuffBonus();


    public void StartBuff(GameObject target)
    {
        if (isActive)
        {
            ApplyConsecutiveBuff();
        }
        else
        {
            ApplyBuff(target);
            isActive = true;

        }
    }

    public void EndBuff(GameObject target)
    {
        if (isActive)
        {
            RemoveBuff(target);
            isActive = false;
        }
    }
}
