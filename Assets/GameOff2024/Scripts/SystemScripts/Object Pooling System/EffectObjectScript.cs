using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectScript : MonoBehaviour
{
    public EffectsPoolScript effectPool;
    private ParticleSystem[] particleSystems;

    private void Awake()
    {
        // Get all particle systems in this object, including children
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        if (particleSystems.Length == 0)
        {
            Debug.LogError("No ParticleSystem components found on this object!");
        }
    }

    public void SetEffectPool(EffectsPoolScript effectPool)
    {
        this.effectPool = effectPool;
    }

    public void PlayEffect()
    {
        float duration = 0;
        // Play all particle systems
        foreach (var ps in particleSystems)
        {
            ps.Play();

            if (duration < ps.main.duration)
            {
                duration = ps.main.duration;
            }
        }

        StartCoroutine(ReturnToPoolAfterEffect(duration));
    }

    private IEnumerator ReturnToPoolAfterEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Return to pool or destroy
        ReturnToPool();
    }

    private bool AreAnyParticlesAlive()
    {
        // Check if any particle system is still alive
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps != null && ps.IsAlive(true))
            {
                return true;
            }
        }
        return false;
    }

    protected void ReturnToPool()
    {
        if (effectPool != null)
        {
            effectPool.ReturnEffectObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

