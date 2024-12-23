using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberFeedback : MonoBehaviour
{
    private MMFeedbacks feedback;
    [SerializeField] private MMFloatingTextMeshPro floatingText;

    private void Awake()
    {
        feedback = GetComponent<MMFeedbacks>();
    }

    private void OnEnable()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.05f);
        feedback.PlayFeedbacks();
    }
}
