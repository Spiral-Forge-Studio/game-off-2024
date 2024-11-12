using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectileManager : MonoBehaviour
{
    [Header("Projectile Parameter Handling")]
    public PlayerStatusSO playerStats;
    private PlayerStatusManager playerStatusManager;

    // projectile params
    private MinigunProjectileParams minigunParams;

    private Dictionary<EProjectileType, ProjectileParams> projectileParamsDict = new Dictionary<EProjectileType, ProjectileParams>();

    [Header("Projectile Object Pooling")]
    public List<ProjectilePoolScript> projectilePoolList;
    private Dictionary<EProjectileType, ProjectilePoolScript> projectilePoolDict = new Dictionary<EProjectileType, ProjectilePoolScript>();

    // List to manage all active projectiles
    private List<Projectile> activeProjectiles = new List<Projectile>();
    private List<Projectile> deadProjectiles = new List<Projectile>();

    private List<ProjectileShooter> currentProjectileShooters = new List<ProjectileShooter>();
    private List<ProjectileShooter> newProjectileShooters = new List<ProjectileShooter>();
    private List<ProjectileShooter> deadProjectileShooters = new List<ProjectileShooter>();

    // Local Variables
    private ProjectilePoolScript currentProjectilePool;

    private void Awake()
    {
        playerStatusManager = FindObjectOfType<PlayerStatusManager>();
    }

    private void Start()
    {
        foreach (ProjectilePoolScript pool in projectilePoolList)
        {
            pool.Initialize();
            projectilePoolDict[pool.projectileType] = pool;
        }
    }

    private void Update()
    {
        if (NumberOfShootersChanged())
        {
            UpdateCurrentProjectileShooters();
        }

        FireProjectiles();

        UpdateProjectileMovement();

        UpdateProjectileActiveStatus();
    }

    #region --- Projectile Pooling and Movement Update ---

    public void FireProjectiles()
    {
        foreach (ProjectileShooter shooter in currentProjectileShooters)
        {
            if (shooter.fireProjectile)
            {
                for (int i = 0; i < shooter.projectileAmount; i++)
                {
                    if (currentProjectilePool == null || currentProjectilePool.projectileType != shooter.projectileType)
                    {
                        currentProjectilePool = projectilePoolDict[shooter.projectileType];
                    }

                    GameObject projectileObject = currentProjectilePool.GetProjectile();

                    projectileObject.transform.position = shooter.firePoint.position;
                    projectileObject.transform.rotation = shooter.fireRotations[i];

                    Projectile projectile = projectileObject.GetComponent<Projectile>();

                    if (projectile != null)
                    {
                        // Initialize the projectile and add to active projectiles list
                        projectile.SetProjectilePool(projectilePoolDict[shooter.projectileType]);
                        activeProjectiles.Add(projectile);

                        // Set Parameters
                        projectile.SetProjectileParams(GetProjectileParams(shooter.projectileType));
                    }
                }

                shooter.fireProjectile = false;
            }
        }
    }

    private void UpdateProjectileMovement()
    {
        foreach (Projectile projectile in activeProjectiles)
        {
            // Check if the projectile needs to be returned to the pool
            if (!projectile.isActive) // Assuming IsInactive indicates it's ready to be returned
            {
                deadProjectiles.Add(projectile);
            }
            else
            {
                projectile.ProjectileMovementUpdate();
            }
        }
    }

    private void UpdateProjectileActiveStatus()
    {
        if (deadProjectiles.Count > 0)
        {
            foreach (Projectile projectile in deadProjectiles)
            {
                activeProjectiles.Remove(projectile);
            }
        }

        deadProjectiles.Clear();
    }

    #endregion

    #region --- Projectile Parameter Setup ---


    /// <summary>
    /// The parameters you want to pass to your projectile based on the enum projectile type, edit as needed
    /// </summary>
    /// <param name="projectileType"></param>
    /// <returns></returns>

    private ProjectileParams GetProjectileParams(EProjectileType projectileType)
    {
        if (projectileType == EProjectileType.Minigun)
        {
            return playerStatusManager.GetMinigunProjectileParams();
        }
        else if (projectileType == EProjectileType.Rocket)
        {
            return playerStatusManager.GetRocketProjectileParams();
        }

        // TODO: Add your own conditions and return values, you can reference other scripts.

        return null;
    }

    #endregion

    #region --- Projectile Shooter Management ---

    public void AddProjectileShooter(ProjectileShooter shooter)
    {
        newProjectileShooters.Add(shooter);
    }

    public void RemoveProjectileShooter(ProjectileShooter shooter)
    {
        deadProjectileShooters.Add(shooter);
    }

    private void UpdateCurrentProjectileShooters()
    {
        foreach (ProjectileShooter shooter in deadProjectileShooters)
        {
            currentProjectileShooters.Remove(shooter);
        }
        deadProjectileShooters.Clear();

        foreach (ProjectileShooter shooter in newProjectileShooters)
        {
            currentProjectileShooters.Add(shooter);
        }
        newProjectileShooters.Clear();
    }

    private bool NumberOfShootersChanged()
    {
        if (deadProjectileShooters.Count > 0 || newProjectileShooters.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}
