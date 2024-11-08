using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCombatInputs
{
    public bool LeftShoot;
    public bool RightShoot;
    public bool LeftSwap;
    public bool RightSwap;
    public Vector3 mousePos;
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private ProjectileShooter minigunProjectileShooter;
    [SerializeField] private ProjectileShooter rocketProjectileShooter;

    [SerializeField] private PlayerStatusSO playerStats;

    private Minigun minigun;
    private Rocket rocket;

    private Vector3 aimPosition;
    private Quaternion aimRotation;

    // Minigun Params
    private float shotInterval;
    private float minigun_lastShotTime;


    private void Awake()
    {
        minigun = GetComponentInChildren<Minigun>();
        rocket = GetComponentInChildren<Rocket>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindAnyObjectByType<ProjectileManager>().AddProjectileShooter(minigunProjectileShooter);
        FindAnyObjectByType<ProjectileManager>().AddProjectileShooter(rocketProjectileShooter);
    }

    // Update is called once per frame
    void Update()
    {
        // some logic
    }

    public void InitializeBaseWeaponStats()
    {

        
    }

    public void SetInputs(ref PlayerCombatInputs inputs)
    {
        aimPosition = inputs.mousePos;

        if (inputs.LeftShoot)
        {
            if (Time.time - minigun_lastShotTime > 1f / playerStats.MinigunFireRate)
            {

                minigunProjectileShooter.FireProjectile(
                    Quaternion.LookRotation(Vector3.ProjectOnPlane(aimPosition - minigunProjectileShooter.firePoint.position, minigunProjectileShooter.gameObject.transform.up)));

                minigun_lastShotTime = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(minigunProjectileShooter.firePoint.position, aimPosition);
    }
}
