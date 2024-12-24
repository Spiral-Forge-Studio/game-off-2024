using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class NPCHealth : MonoBehaviour
{

    [SerializeField] private GameObject mob;
    [HideInInspector] private GameObject player;
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerStatusManager _playerdamage;
    [SerializeField] private TrashMobParameters _mobparameters;

    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    [SerializeField] private MMFeedbacks normalHitFeedback;
    [SerializeField] private MMFeedbacks criticalHitFeedback;

    private float _mobcurrenthealth;
    private float _mobhealthmax;

    private EffectsPoolManager effectsPoolManager;
    private MaterialFlasher materialFlasher;
    private MobPoolableScript mobPoolable;
    private TrashMob trashmob;

    [Header("Events")]
    public UnityEvent OnMobDestroyed;
    void Awake()
    {
        //Event Handling
        OnMobDestroyed.AddListener(GameObject.Find("Player").GetComponentInChildren<UniqueBuffHandler>().ApplyOnKillUniqueBuffs);
        player = GameObject.Find("Player");
        _playerdamage = player.GetComponent<PlayerStatusManager>();
        _mobparameters = GetComponent<TrashMobParameters>();
        trashmob = GetComponent<TrashMob>();
        mobPoolable = GetComponent<MobPoolableScript>();

        if(_mobparameters == null)
        {
            Debug.LogWarning("mob parameters not referenced");
        }
        if (_playerdamage == null)
        {
            Debug.LogWarning("player parameters not referenced");
        }

        //Setup
        _mobhealthmax = _mobparameters.health;
        _mobcurrenthealth = _mobhealthmax;
        _slider.maxValue = _mobhealthmax;
        _fill.color = _gradient.Evaluate(1f);

        effectsPoolManager = FindObjectOfType<EffectsPoolManager>();

        materialFlasher = GetComponent<MaterialFlasher>();

    }

    // Update is called once per frame
    void Update()
    {
        DestroyMob(_mobcurrenthealth);
        SetMobHealth(_mobcurrenthealth);
    }

    private void MobDamaged(float damage)
    {
        _mobcurrenthealth = Mathf.Clamp( _mobcurrenthealth - damage, 0, _mobhealthmax);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Mob hit by: " + other);
        if(other.CompareTag("PlayerMinigun"))
        {
            Debug.Log("Taking minigun damage");

            float minigunDamage = other.gameObject.GetComponent<PlayerMinigunProjectileScript>().GetDamage();
            bool isCritical = other.gameObject.GetComponent<PlayerMinigunProjectileScript>().GetIsCritical();

            PlayHitFeedbacks(isCritical, minigunDamage);

            MobDamaged(minigunDamage);
        }

        else if (other.CompareTag("PlayerRocket"))
        {
            Debug.Log("Taking rocket damage");
            float rocketDamage = other.gameObject.GetComponent<RocketExplosionScript>().GetDamage();
            bool isCritical = other.gameObject.GetComponent<RocketExplosionScript>().isCritical;

            PlayHitFeedbacks(isCritical, rocketDamage);

            MobDamaged(rocketDamage);
        }

        materialFlasher.FlashAllMaterials();
    }

    public void PlayHitFeedbacks(bool isCritical, float damage)
    {
        if (isCritical)
        {
            criticalHitFeedback.PlayFeedbacks(transform.position, damage);
        }
        else
        {
            normalHitFeedback.PlayFeedbacks(transform.position, damage);
        }
    }

    private void DestroyMob(float mobcurrenthealth)
    {
        if (mobcurrenthealth <= 0)
        {
            //Debug.Log("Mob Destroyed");
            OnMobDestroyed?.Invoke();

            effectsPoolManager.SpawnMobExplodeEffect(transform.position);

            trashmob.initialized = false;

            mobPoolable.ReturnToPool();
        }
    }

    private void SetMobHealth(float health)
    {
        _slider.value = health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
