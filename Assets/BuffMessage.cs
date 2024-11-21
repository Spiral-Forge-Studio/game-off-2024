using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float duration = 2f;          // Duration the text stays visible
    public float floatSpeed = 1f;       // Speed at which the text floats up
    private TextMeshPro textMesh;
    private Transform target;           // The object the text should follow
    private Vector3 offset = new Vector3(0, 2f, 0); // Offset from the player's position
    public Vector3 rotationOffset = new Vector3(180f, 180f, 180f); // Fix flipped rotation (180 degrees on Y-axis)

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
    }

    public void Initialize(string message, Transform target)
    {
        textMesh.text = message;
        this.target = target; // Set the target to follow
        transform.Rotate(rotationOffset);
    }

    public void SetText(string message)
    {
        textMesh.text = message;
    }

    private void Update()
    {
        if (target != null)
        {
            // Update position to follow the target with an offset
            transform.position = target.position + offset;

            // Face the camera
            //transform.LookAt(Camera.main.transform);

            // Apply rotation offset to fix flipping
            
        }

        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(gameObject); // Destroy the text after the duration
        }
    }
}
