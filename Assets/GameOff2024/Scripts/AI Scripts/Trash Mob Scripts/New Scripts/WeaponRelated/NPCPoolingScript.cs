using System.Collections.Generic;
using UnityEngine;

public class NPCPoolingScript : MonoBehaviour
{
    public NPCWeaponType weaponType;
    public GameObject projectilePrefab;
    public WeaponParameters weaponParameters;
    private Queue<GameObject> poolList = new Queue<GameObject>();
    public int poolSize;

    private void Awake()
    {
        weaponParameters = GetComponentInParent<WeaponParameters>();

        switch (weaponType)
        {
            case NPCWeaponType.Rifle:
                poolSize = weaponParameters.riflemagazinesize;
                break;
            case NPCWeaponType.Shotgun:
                poolSize = weaponParameters.shotgunmagazinesize;
                break;
            case NPCWeaponType.Rocket:
                poolSize = weaponParameters.rocketmagazinesize;
                break;
        }

        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            poolList.Enqueue(projectile);
        }
    }

    public GameObject GetProjectile()
    {
        if (poolList.Count > 0)
        {
            GameObject projectile = poolList.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            // Instantiate new projectile if pool is empty
            GameObject newProjectile = Instantiate(projectilePrefab);
            return newProjectile;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        poolList.Enqueue(projectile);
    }
}
