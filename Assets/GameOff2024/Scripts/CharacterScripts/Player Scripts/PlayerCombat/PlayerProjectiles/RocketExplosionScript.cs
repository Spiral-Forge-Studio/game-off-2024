using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosionScript : MonoBehaviour
{
    [SerializeField] private MMFeedbacks normalHitFeedback;
    [SerializeField] private MMFeedbacks criticalHitFeedback;

    [HideInInspector] public float damage;
    [HideInInspector] public float radius;
    [HideInInspector] public bool isCritical;
    [HideInInspector] public UniqueBuffHandler uniqueBuffHandler;

    private EffectsPoolManager effectPoolManager;
    private AudioSource audioSource;

    private void Awake()
    {
        effectPoolManager = FindObjectOfType<EffectsPoolManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public float GetDamage()
    {
        return damage;
    }

    public void Explode()
    {
        gameObject.transform.localScale = Vector3.one*radius;
        GetComponent<SphereCollider>().enabled = true;
        // Perform SphereCast to check for "Enemy" tagged objects

        AudioManager.instance.PlaySFX(audioSource, EGameplaySFX.RocketExplode);

        effectPoolManager.SpawnRocketHitEffect(transform.position);
        StartCoroutine(DestroyAfterDelay(1.5f));
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
