using UnityEngine;

public class ShotgunProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 5f;
    public float lifetime = 2f;
    public float reloadCooldown = 2f;     // Time between shots
    public float weaponRange = 8f;        // Maximum effective range
    public float spreadAngle = 10f;

    private void Start()
    {
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);
        transform.Rotate(spreadX, spreadY, 0);

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
