using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolScript : MonoBehaviour
{
    public EProjectileType projectileType;
    public GameObject projectilePrefab;
    public int initialSize;
    private Queue<GameObject> poolList = new Queue<GameObject>();

    public void Initialize()
    {
        poolList = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            poolList.Enqueue(projectile);

            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript)
            {
                projectileScript.SetProjectilePool(this);
            }
            else
            {
                Debug.LogError("[PROJECTILE SO ERROR] No Projectile script found on object");
                Debug.Break();
            }
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