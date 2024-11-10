using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 30f;
    public float lifetime = 4f;
    public float reloadCooldown = 3f;    // Time between shots
    public float weaponRange = 20f;      // Maximum effective range
    public float explosionRadius = 5f;

    public GameObject explosionEffect;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                //hitCollider.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}