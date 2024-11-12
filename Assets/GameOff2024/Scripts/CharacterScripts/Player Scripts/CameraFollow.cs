using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    public Transform cameraFollowPoint;
    public Vector3 mouseFollowPoint;


    [Header("Parameters")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float maxTargetDistance;
    [SerializeField] private float minCameraSplitMoveDistance;
    [SerializeField] private LayerMask targetableMasks;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerKCC>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, targetableMasks, QueryTriggerInteraction.Collide))
        {
            mouseFollowPoint = hitInfo.point;

            //// Calculate direction and distance from player to mouseFollowPoint
            //Vector3 direction = mouseFollowPoint - cameraFollowPoint.position;
            //float distance = direction.magnitude;

            //// Clamp to maxFollowDistance if necessary
            //if (distance > maxTargetDistance)
            //{
            //    mouseFollowPoint = playerTransform.position + direction.normalized * maxTargetDistance;
            //}
        }

        // Calculate the midpoint between the player and clamped mouseFollowPoint
        Vector3 followPosition = (cameraFollowPoint.position + mouseFollowPoint) / 2;

        // Smoothly move the transform toward followPosition
        //Vector3 newPosition = Vector3.Lerp(transform.position, followPosition, followSpeed * Time.deltaTime);
        Vector3 newPosition = Vector3.Lerp(transform.position, cameraFollowPoint.position + Vector3.back * cameraDistance, followSpeed * Time.deltaTime);

        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mouseFollowPoint, 0.1f);
    }
}
