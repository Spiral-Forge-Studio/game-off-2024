using System;
using System.Collections.Generic;
using UnityEngine;

#region --- SFX Enums ---

public enum EGameplaySFX
{
    MinigunFire,
    MinigunBulletHit,
    MinigunReload,
    RocketFire,
    RocketExplode,
    RocketRearm,
    RocketAccumulate,
    PlayerWalk,
    PlayerDash,
    PlayerGetHit,
    PlayerDie,

    MobHit,
    MobDie,
    MobWalk,
    MobRoll,
    MobWindup,
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
    MobExplodeOnDeath
}

#endregion

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("General Audio Sources")]
    public AudioSource menuMusicSource;
    public AudioSource menuUISource;

    private Dictionary<EGameplaySFX, AudioClip[]> GameplaySFXDict = new Dictionary<EGameplaySFX, AudioClip[]>();

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("UI SFX")]
    [SerializeField] private AudioClip[] buttonSfxList;

    [Header("Gameplay SFX")]
    [SerializeField] private AudioClip[] MinigunFireSFX;
    [SerializeField] private AudioClip[] MinigunBulletHitSFX;
    [SerializeField] private AudioClip[] MinigunReloadSFX;
    [SerializeField] private AudioClip[] RocketFireSFX;
    [SerializeField] private AudioClip[] RocketExplodeSFX;
    [SerializeField] private AudioClip[] RocketRearmSFX;
    [SerializeField] private AudioClip[] RocketAccumulateSFX;
    [SerializeField] private AudioClip[] PlayerWalkSFX;
    [SerializeField] private AudioClip[] PlayerDashSFX;
    [SerializeField] private AudioClip[] PlayerGetHitSFX;
    [SerializeField] private AudioClip[] PlayerDieSFX;

    [SerializeField] private AudioClip[] MobHitSFX;
    [SerializeField] private AudioClip[] MobDieSFX;
    [SerializeField] private AudioClip[] MobWindupSFX;
    [SerializeField] private AudioClip[] MobWalkSFX;
    [SerializeField] private AudioClip[] MobRollSFX;
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

    [SerializeField] private AudioClip[] MobExplodeOnDeathSFX;

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
        GameplaySFXDict.Add(EGameplaySFX.MinigunReload, MinigunReloadSFX);
        GameplaySFXDict.Add(EGameplaySFX.RocketFire, RocketFireSFX);
        GameplaySFXDict.Add(EGameplaySFX.RocketExplode, RocketExplodeSFX);
        GameplaySFXDict.Add(EGameplaySFX.RocketRearm, RocketRearmSFX);
        GameplaySFXDict.Add(EGameplaySFX.RocketAccumulate, RocketAccumulateSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerWalk, PlayerWalkSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerDash, PlayerDashSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerGetHit, PlayerGetHitSFX);
        GameplaySFXDict.Add(EGameplaySFX.PlayerDie, PlayerDieSFX);

        GameplaySFXDict.Add(EGameplaySFX.MobHit, MobHitSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobDie, MobDieSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobWindup, MobWindupSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobWalk, MobWalkSFX);
        GameplaySFXDict.Add(EGameplaySFX.MobRoll, MobRollSFX);
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

        GameplaySFXDict.Add(EGameplaySFX.MobExplodeOnDeath, MobExplodeOnDeathSFX);

    }

    public void PlayMusic(AudioSource source, int musicNumber, bool loop = true)
    {
        if (loop)
        {
            source.clip = musicList[musicNumber];
            source.Play();
        }
        else
        {
            source.loop = false;
            source.clip = musicList[musicNumber];
            source.Play();
        }
    }

    public void PlayButtonSFX(AudioSource source, int buttonNumber)
    {
        source.clip = buttonSfxList[buttonNumber];
        source.Play();
    }

    public void PlaySFX(AudioSource audioSource, EGameplaySFX sfxEnum, int index = 0, bool randomSound = false)
    {
        AudioClip[] clips = null;

        clips = GameplaySFXDict[sfxEnum];

        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"No AudioClips found for key '{sfxEnum}'.");
            return;
        }

        audioSource.clip = randomSound ? clips[UnityEngine.Random.Range(0, clips.Length)] : clips[index];

        audioSource.Play();

        //Debug.Log("Played " + sfxEnum);
    }
}