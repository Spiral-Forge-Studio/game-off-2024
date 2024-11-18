using UnityEngine;
using static Buff;

public abstract class Buff : MonoBehaviour
{
    
    public enum BuffType { Flat, Percentage, Unique }
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
    

    public float duration;
    protected bool isActive = false;

    public Buff(float duration)
    {
        this.duration = duration;
    }

    public abstract void ApplyBuff(GameObject target);   // Define how the buff affects the target
    public abstract void RemoveBuff(GameObject target);  // Define how to remove the buff's effect
    public abstract void ApplyConsecutiveBuff();
    public abstract void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float InitAmount = 0, float ConsecAmount = 0, float ScaleAmount = 0);
    public abstract string getBuffName();
    public abstract BuffType getBuffType();
    public abstract Rarity getBuffRarity();
    public abstract Rarity getRandomRarity();
    public abstract BuffType getRandomType();
    public abstract float getBuffBonus();

    public void StartBuff(GameObject target)
    {
        if (isActive) {
            ApplyConsecutiveBuff();
        }
        else
        {
            ApplyBuff(target);
            isActive = true;
        }
        
        // Do not invoke EndBuff here, as we don't want to destroy the buff GameObject
    }

    private void EndBuff()
    {
        if (isActive)
        {
            RemoveBuff(gameObject);
            isActive = false;
            Destroy(this); // Clean up
        }
    }
}
