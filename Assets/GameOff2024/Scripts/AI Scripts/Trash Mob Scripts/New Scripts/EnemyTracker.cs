using System;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public event Action onDestroyed;

    private void OnDestroy()
    {
        onDestroyed?.Invoke();
        Debug.Log($"[EnemyTracker] {gameObject.name} destroyed.");
    }
}
