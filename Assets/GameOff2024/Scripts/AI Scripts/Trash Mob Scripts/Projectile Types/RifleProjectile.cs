using UnityEngine;

public class RifleProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 3f;
    public float reloadCooldown = 1.5f;  // Time between shots
    public float weaponRange = 15f;      // Maximum effective range

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
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}