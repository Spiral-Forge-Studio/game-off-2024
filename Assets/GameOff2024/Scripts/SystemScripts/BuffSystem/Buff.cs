using UnityEngine;

public abstract class Buff : MonoBehaviour
{

    public enum BuffType { Flat, Percentage, Unique }
    public enum Rarity { Common, Rare, Epic, Legendary }


    public float duration;
    protected bool isActive = false;

    public Buff(float duration)
    {
        this.duration = duration;
    }

    public abstract void ApplyBuff(GameObject target);   // Define how the buff affects the target
    public abstract void RemoveBuff(GameObject target);  // Define how to remove the buff's effect

    public abstract void UpdateBuffValues(Buff.BuffType bufftype, Buff.Rarity buffrarity, float InitAmount = 0, float ConsecAmount = 0, float ScaleAmount = 0);

    public void StartBuff(GameObject target)
    {
        ApplyBuff(target);
        isActive = true;
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
