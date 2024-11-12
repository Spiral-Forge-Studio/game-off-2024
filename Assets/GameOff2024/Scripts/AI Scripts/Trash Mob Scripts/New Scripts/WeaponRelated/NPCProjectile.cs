using UnityEngine;
using System.Collections;

public class NPCProjectile : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private float damage;
    private NPCPoolingScript poolingScript;

    public void Initialize(NPCWeaponType weaponType, WeaponParameters weaponParams, NPCPoolingScript pool)
    {
        poolingScript = pool;

        switch (weaponType)
        {
            case NPCWeaponType.Rifle:
                speed = weaponParams.riflespeed;
                lifetime = weaponParams.riflelifetime;
                damage = weaponParams.rifledamage;
                break;
            case NPCWeaponType.Shotgun:
                speed = weaponParams.shotgunspeed;
                lifetime = weaponParams.shotgunlifetime;
                damage = weaponParams.shotgundamage;
                break;
            case NPCWeaponType.Rocket:
                speed = weaponParams.rocketspeed;
                lifetime = weaponParams.rocketlifetime;
                damage = weaponParams.rocketdamage;
                break;
        }

        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        DeactivateProjectile();
    }

    private void Update()
    {
        // Move the projectile forward
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void DeactivateProjectile()
    {
        gameObject.SetActive(false);
        poolingScript.ReturnProjectile(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit Object" + other); 
        // Implement damage or other effects here, if needed
        DeactivateProjectile();
    }
}
