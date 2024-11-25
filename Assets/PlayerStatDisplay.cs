using UnityEngine;
using System.Text;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    public PlayerStatusSO playerStats;
    public TextMeshProUGUI statsText;

    void Start()
    {
        if (!statsText)
            statsText = GetComponent<TextMeshProUGUI>(); 
    }

    void Update()
    {
        UpdateStatsText();
    }

    void UpdateStatsText()
    {
        StringBuilder stats = new StringBuilder();
        stats.AppendLine("Player Stats:");

        // General Stats
        stats.AppendLine($"Health: {playerStats.Health}");
        stats.AppendLine($"Shield: {playerStats.Shield}");
        stats.AppendLine($"Shield Regen Rate: {playerStats.ShieldRegenAmount}");
        stats.AppendLine($"Shield Regen Delay: {playerStats.ShieldRegenTickInterval}");
        stats.AppendLine($"Damage Reduction: {playerStats.DamageReduction}");
        stats.AppendLine($"Move Speed: {playerStats.MoveSpeed}");
        stats.AppendLine($"Dash Cooldown: {playerStats.DashCooldown}");

        // Minigun Stats
        stats.AppendLine($"Minigun Damage: {playerStats.MinigunDamage}");
        stats.AppendLine($"Minigun Crit Rate: {playerStats.MinigunCritRate}");
        stats.AppendLine($"Minigun Crit Damage: {playerStats.MinigunCritDamage}");
        stats.AppendLine($"Minigun Fire Rate: {playerStats.MinigunFireRate}");
        stats.AppendLine($"Minigun Reload Time: {playerStats.MinigunReloadTime}");
        stats.AppendLine($"Minigun Magazine Size: {playerStats.MinigunMagazineSize}");
        stats.AppendLine($"Minigun Projectile Lifetime: {playerStats.MinigunProjectileLifetime}");
        stats.AppendLine($"Minigun Projectile Speed: {playerStats.MinigunProjectileSpeed}");
        stats.AppendLine($"Minigun Bullet Deviation Angle: {playerStats.MinigunBulletDeviationAngle}");

        // Rocket Stats
        stats.AppendLine($"Rocket Damage: {playerStats.RocketDamage}");
        stats.AppendLine($"Rocket Crit Rate: {playerStats.RocketCritRate}");
        stats.AppendLine($"Rocket Crit Damage: {playerStats.RocketCritDamage}");
        stats.AppendLine($"Rocket Fire Rate: {playerStats.RocketFireRate}");
        stats.AppendLine($"Rocket Reload Time: {playerStats.RocketReloadTime}");
        stats.AppendLine($"Rocket Magazine Size: {playerStats.RocketMagazineSize}");
        stats.AppendLine($"Rocket Projectile Lifetime: {playerStats.RocketProjectileLifetime}");
        stats.AppendLine($"Rocket Projectile Speed: {playerStats.RocketProjectileSpeed}");
        stats.AppendLine($"Rocket Explosion Radius: {playerStats.RocketExplosionRadius}");

        // Update the UI text if `statsText` is assigned
        if (statsText != null)
        {
            statsText.text = stats.ToString();
        }
    }

}
