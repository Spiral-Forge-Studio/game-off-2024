using System;
using System.Collections.Generic;
using UnityEngine;

#region --- SFX Enums ---

public enum EGameplaySFX
{
    MinigunFire,
    MinigunBulletHit,
    RocketFire,
    RocketExplode,
    PlayerWalk,
    PlayerDash,
    PlayerGetHit,
    PlayerDie,

    MobHit,
    MobDie,
    SuicideMobPrime,
    SuicideMobExplode,
    MobShotgunFire,
    MobRifleFire,
    MobRocketFire,
    MobRocketExplode,

    NormalLevelRoomEntered,
    NormalRoomCleared,
    BossLevelRoomEntered,
    BossLevelRoomCleared,
    BossKilled,
}

#endregion

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("General Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource musicNonLoopSource;
    [SerializeField] private AudioSource UIAudioSource;


    private Dictionary<EGameplaySFX, AudioClip[]> GameplaySFXDict = new Dictionary<EGameplaySFX, AudioClip[]>();

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("UI SFX")]
    [SerializeField] private AudioClip[] buttonSfxList;

    [Header("Gameplay SFX")]
    [SerializeField] private AudioClip[] MinigunFireSFX;
    [SerializeField] private AudioClip[] MinigunBulletHitSFX;
    [SerializeField] private AudioClip[] RocketFireSFX;
    [SerializeField] private AudioClip[] RocketExplodeSFX;
    [SerializeField] private AudioClip[] PlayerWalkSFX;
    [SerializeField] private AudioClip[] PlayerDashSFX;
    [SerializeField] private AudioClip[] PlayerGetHitSFX;
    [SerializeField] private AudioClip[] PlayerDieSFX;

    [SerializeField] private AudioClip[] MobHitSFX;
    [SerializeField] private AudioClip[] MobDieSFX;
    [SerializeField] private AudioClip[] SuicideMobPrimeSFX;
    [SerializeField] private AudioClip[] SuicideMobExplodeSFX;
    [SerializeField] private AudioClip[] MobShotgunFireSFX;
    [SerializeField] private AudioClip[] MobRifleFireSFX;
    [SerializeField] private AudioClip[] MobRocketFireSFX;
    [SerializeField] private AudioClip[] MobRocketExplodeSFX;

    [SerializeField] private AudioClip[] NormalLevelRoomEnteredSFX;
    [SerializeField] private AudioClip[] NormalRoomClearedSFX;
    [SerializeField] private AudioClip[] BossLevelRoomEnteredSFX;
    [SerializeField] private AudioClip[] BossLevelRoomClearedSFX;
    [SerializeField] private AudioClip[] BossKilledSFX;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGameplaySFXDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGameplaySFXDictionaries()
    {
        GameplaySFXDict.Add(EGameplaySFX.MinigunFire, MinigunFireSFX);
        GameplaySFXDict.Add(EGameplaySFX.MinigunBulletHit, MinigunBulletHitSFX);
        GameplaySFXDict.Add(EGameplaySFX.RocketFire, RocketFireSFX);
        GameplaySFXDict.Add(EGameplaySFX.RocketExplode, RocketExplodeSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerWalk, PlayerWalkSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerDash, PlayerDashSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerGetHit, PlayerGetHitSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerDie, PlayerDieSFX);

        GameplaySFXDict.Add(EGameplaySFX.MobHit, MobHitSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobDie, MobDieSFX);
        GameplaySFXDict.Add(EGameplaySFX.SuicideMobPrime, SuicideMobPrimeSFX);
        GameplaySFXDict.Add(EGameplaySFX.SuicideMobExplode, SuicideMobExplodeSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobShotgunFire, MobShotgunFireSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobRifleFire, MobRifleFireSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobRocketFire, MobRocketFireSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobRocketExplode, MobRocketExplodeSFX);

        GameplaySFXDict.Add(EGameplaySFX.NormalLevelRoomEntered, NormalLevelRoomEnteredSFX);
        GameplaySFXDict.Add(EGameplaySFX.NormalRoomCleared, NormalRoomClearedSFX);
        GameplaySFXDict.Add(EGameplaySFX.BossLevelRoomEntered, BossLevelRoomEnteredSFX);
        GameplaySFXDict.Add(EGameplaySFX.BossLevelRoomCleared, BossLevelRoomClearedSFX);
        GameplaySFXDict.Add(EGameplaySFX.BossKilled, BossKilledSFX);
    }

    public void PlayMusic(int musicNumber, float volume = 1, bool loop = true)
    {
        if (loop)
        {
            musicSource.clip = musicList[musicNumber];
            musicSource.volume = volume;
            musicSource.Play();
        }
        else
        {
            musicNonLoopSource.clip = musicList[musicNumber];
            musicNonLoopSource.volume = volume;
            musicNonLoopSource.Play();
        }
    }

    public void PlayButtonSFX(int buttonNumber)
    {
        UIAudioSource.clip = buttonSfxList[buttonNumber];
        UIAudioSource.Play();
    }

    public void PlaySFX(AudioSource audioSource, EGameplaySFX sfxEnum, bool randomSound = false)
    {
        AudioClip[] clips = null;

        clips = GameplaySFXDict[sfxEnum];

        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"No AudioClips found for key '{sfxEnum}'.");
            return;
        }

        audioSource.clip = randomSound ? clips[UnityEngine.Random.Range(0, clips.Length)] : clips[0];
        audioSource.Play();
    }

    public void SetMusicAndUIAudioSourcesToPlayerPosition(Transform playerTransform)
    {
        musicSource.gameObject.transform.SetParent(playerTransform, true);
        musicNonLoopSource.gameObject.transform.SetParent(playerTransform, true);
        UIAudioSource.gameObject.transform.SetParent(playerTransform, true);
    }
}