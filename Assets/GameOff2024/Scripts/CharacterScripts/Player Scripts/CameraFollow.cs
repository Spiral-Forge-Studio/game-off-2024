using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerFollowPoint;
    private Vector3 mouseFollowPoint;


    [Header("Parameters")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float maxTargetDistance;
    [SerializeField] private float minCameraSplitMoveDistance;
    [SerializeField] private LayerMask targetableMasks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, targetableMasks, QueryTriggerInteraction.Collide))
        {
            mouseFollowPoint = hitInfo.point;

            // Calculate direction and distance from player to mouseFollowPoint
            Vector3 direction = mouseFollowPoint - playerFollowPoint.position;
            float distance = direction.magnitude;

            // Clamp to maxFollowDistance if necessary
            if (distance > maxTargetDistance)
            {
                mouseFollowPoint = playerFollowPoint.position + direction.normalized * maxTargetDistance;
            }
        }

        // Calculate the midpoint between the player and clamped mouseFollowPoint
        Vector3 followPosition = (playerFollowPoint.position + mouseFollowPoint) / 2;

        // Smoothly move the transform toward followPosition
        //Vector3 newPosition = Vector3.Lerp(transform.position, followPosition, followSpeed * Time.deltaTime);
        Vector3 newPosition = Vector3.Lerp(transform.position, playerFollowPoint.position, followSpeed * Time.deltaTime);

        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mouseFollowPoint, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerFollowPoint.position, maxTargetDistance);
    }
}
