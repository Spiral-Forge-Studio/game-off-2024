using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using System;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] private GameObject mob;
    [HideInInspector] private GameObject player;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject healthBarGameObject; // Reference to the health bar GameObject
    [SerializeField] private PlayerStatusManager _playerdamage;
    [SerializeField] private TrashMobParameters _mobparameters;

    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    [SerializeField] private MMFeedbacks normalHitFeedback;
    [SerializeField] private MMFeedbacks criticalHitFeedback;

    [Header("Health Bar Display")]
    [SerializeField] private float healthBarDisplayDuration = 3f; // Duration before hiding the health bar

    private float _mobcurrenthealth;
    private float _mobhealthmax;
    private Coroutine healthBarCoroutine;

    private EffectsPoolManager effectsPoolManager;
    private MaterialFlasher materialFlasher;
    private MobPoolableScript mobPoolable;
    private TrashMob trashmob;

    [Header("Events")]
    public UnityEvent OnMobDestroyed;

    private GameStateManager gameStateManager;

    void Awake()
    {
       
        // Event Handling
        gameStateManager = FindObjectOfType<GameStateManager>();
        OnMobDestroyed.AddListener(gameStateManager.MobDied);

        player = GameObject.Find("Player");
        _playerdamage = player.GetComponent<PlayerStatusManager>();
        _mobparameters = GetComponent<TrashMobParameters>();
        trashmob = GetComponent<TrashMob>();
        mobPoolable = GetComponent<MobPoolableScript>();

        if (_mobparameters == null)
        {
            Debug.LogWarning("mob parameters not referenced");
        }
        if (_playerdamage == null)
        {
            Debug.LogWarning("player parameters not referenced");
        }

        // Setup
        _mobhealthmax = _mobparameters.health;
        _mobcurrenthealth = _mobhealthmax;
        _slider.maxValue = _mobhealthmax;
        _fill.color = _gradient.Evaluate(1f);

        effectsPoolManager = FindObjectOfType<EffectsPoolManager>();
        materialFlasher = GetComponent<MaterialFlasher>();

        // Initially hide the health bar
        healthBarGameObject.SetActive(false);
    }

    void Update()
    {
        DestroyMob(_mobcurrenthealth);
        SetMobHealth(_mobcurrenthealth);
    }

    private void MobDamaged(float damage)
    {
        _mobcurrenthealth = Mathf.Clamp(_mobcurrenthealth - damage, 0, _mobhealthmax);

        // Show the health bar and restart the visibility coroutine
        ShowHealthBar();
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }
        healthBarCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMinigun"))
        {
            float minigunDamage = other.gameObject.GetComponent<PlayerMinigunProjectileScript>().GetDamage();
            bool isCritical = other.gameObject.GetComponent<PlayerMinigunProjectileScript>().GetIsCritical();
            PlayHitFeedbacks(isCritical, minigunDamage);
            MobDamaged(minigunDamage);
        }
        else if (other.CompareTag("PlayerRocket"))
        {
            float rocketDamage = other.gameObject.GetComponent<RocketExplosionScript>().GetDamage();
            bool isCritical = other.gameObject.GetComponent<RocketExplosionScript>().isCritical;
            PlayHitFeedbacks(isCritical, rocketDamage);
            MobDamaged(rocketDamage);
        }

        materialFlasher.FlashAllMaterials();
    }

    public void PlayHitFeedbacks(bool isCritical, float damage)
    {
        float roundedDamage = (float)Math.Round(damage, 2);
        if (isCritical)
        {
            criticalHitFeedback.PlayFeedbacks(transform.position, roundedDamage);
        }
        else
        {
            normalHitFeedback.PlayFeedbacks(transform.position, roundedDamage);
        }
    }

    private void DestroyMob(float mobcurrenthealth)
    {
        if (mobcurrenthealth <= 0)
        {
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

    private void ShowHealthBar()
    {
        healthBarGameObject.SetActive(true);
    }

    private IEnumerator HideHealthBarAfterDelay()
    {
        yield return new WaitForSeconds(healthBarDisplayDuration);
        healthBarGameObject.SetActive(false);
    }
}
