using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobMovementAudio : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private TrashMobParameters _mobParameters;
    [SerializeField] private AudioSource _audioSource;

    private float movementspeed;
    private bool isPlayingAudio;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _mobParameters = GetComponent<TrashMobParameters>();
        _audioSource = GetComponent<AudioSource>();
        if( _agent == null )
        {
            Debug.LogWarning("NavMeshAgent not referenced;");
        }

        else if (_mobParameters == null)
        {
            Debug.LogWarning("MobParameters not referenced;");
        }

        else if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source not referenced;");
        }

        _audioSource.loop = true;
    }

    private void Update()
    {
        MovementAudio();
    }

    private void MovementAudio()
    {
        // Check if the agent's velocity magnitude is greater than a small threshold to confirm movement
        if (_agent.velocity.magnitude > 0.1f )
        {
            Debug.Log("entered here");
            if (!isPlayingAudio)
            {
                // Start playing the movement audio once
                AudioManager.instance.PlaySFX(_audioSource, EGameplaySFX.MobWalk, 0, true);
                
                isPlayingAudio = true;
            }
        }
        
        else
        {
            // Stop playing audio if agent stops moving
            if (isPlayingAudio)
            {
                _audioSource.Stop();
                isPlayingAudio = false;
            }
        }
        
    }
}
