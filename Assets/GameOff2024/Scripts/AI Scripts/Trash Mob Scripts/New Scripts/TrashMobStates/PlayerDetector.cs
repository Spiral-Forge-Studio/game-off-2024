using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private TrashMobParameters _parameters;
    SphereCollider _playerdetectionsphere;

    [Header("Player Detection Range")]
    public float _colliderradius;
    [Header("Player Detection Left Range")]
    public float _playerleft;


    public bool PlayerInRange => _detectedPlayer != null;

    private Transform _detectedPlayer;

    private void Awake()
    {
        _parameters = GetComponentInParent<TrashMobParameters>();
        _playerdetectionsphere = GetComponent<SphereCollider>();

        if (_playerdetectionsphere == null) { Debug.LogWarning("No Sphere Collider Referenced"); }
        if (_parameters == null) { Debug.LogWarning("No Parameters Referenced"); }

        _colliderradius = _parameters.PlayerDetectionRadius;
        _playerdetectionsphere.radius = _colliderradius;
        _playerleft = _parameters.PlayerExitCombatRange;



    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player In Range");
            _detectedPlayer = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ClearDetectedPlayer());
        }
    }

    private IEnumerator ClearDetectedPlayer()
    {
        yield return new WaitForSeconds(_playerleft);
        _detectedPlayer = null;
    }

   /*
    private void OnDrawGizmos()
    {
        Color color = Color.red;
        Gizmos.color = color;

        Gizmos.DrawWireSphere(transform.position, _colliderradius);
    }
   */
}