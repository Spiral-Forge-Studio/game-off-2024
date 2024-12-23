using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerScript : MonoBehaviour
{
    public List<AudioSource> AudioSources;
    public List<EGameplaySFX> SFXs;
    // Start is called before the first frame update

    void OnEnable()
    {
        if (AudioSources.Count == SFXs.Count)
        {
            for (int i = 0; i < SFXs.Count; i++)
            {
                AudioManager.instance.PlaySFX(AudioSources[i], SFXs[i]);
            }
        }
        else
        {
            Debug.Log("ERROR: Sources do not match SFX count");
        }
    }
}
