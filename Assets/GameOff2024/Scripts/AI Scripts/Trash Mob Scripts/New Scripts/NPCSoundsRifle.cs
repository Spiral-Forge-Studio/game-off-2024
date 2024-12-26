using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSoundsRifle : MonoBehaviour
{
    public NPCWeaponType weaponType;
    public AudioSource _weaponsource;
    public AudioSource _wheelsource;
    public NavMeshAgent _agent;

    //private bool _isMoving;

    private void Awake()
    {
        _weaponsource = GetComponents<AudioSource>()[0];
        //_wheelsource = GetComponents<AudioSource>()[1];
    }

    private void Update()
    {
        /*
        // Check if the agent is moving
        if (_agent.velocity.magnitude > 0.01f)
        {
            if (!_isMoving) // Only play the audio if not already moving
            {
                _isMoving = true;
                if (!_wheelsource.isPlaying) // Ensure the sound doesn't restart
                {
                    AudioManager.instance.PlaySFX(_wheelsource, EGameplaySFX.MobRoll, 0, false);
                }
            }
        }
        else
        {
            _isMoving = false; // Reset the state if the agent stops
            _wheelsource.Stop();
        }
        */
    }
    private void Shoot()
    {
        AudioManager.instance.PlaySFX(_weaponsource, EGameplaySFX.MobRifleFire, 0, true);
    }
}
