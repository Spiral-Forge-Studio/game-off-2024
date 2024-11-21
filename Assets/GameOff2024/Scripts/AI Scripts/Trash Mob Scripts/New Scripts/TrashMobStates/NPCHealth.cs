using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; 

public class NPCHealth : MonoBehaviour
{

    [SerializeField] private GameObject mob;
    [HideInInspector] private GameObject player;
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerStatusManager _playerdamage;
    [SerializeField] private TrashMobParameters _mobparameters;

    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    private float _mobcurrenthealth;
    private float _mobhealthmax;


    [Header("Events")]
    public UnityEvent OnMobDestroyed;
    void Awake()
    {
        player = GameObject.Find("Player");
        _playerdamage = player.GetComponent<PlayerStatusManager>();
        _mobparameters = GetComponent<TrashMobParameters>();

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
            float minigunDamage = other.gameObject.GetComponent<MinigunProjectileScript>().GetDamage();
            MobDamaged(minigunDamage);
        }

        else if (other.CompareTag("PlayerRocket"))
        {
            Debug.Log("Taking rocket damage");
            float rocketDamage = other.gameObject.GetComponent<RocketExplosionScript>().GetDamage();
            MobDamaged(rocketDamage);
        }
    }

    private void DestroyMob(float mobcurrenthealth)
    {
        if (mobcurrenthealth <= 0)
        {
            //Debug.Log("Mob Destroyed");
            OnMobDestroyed?.Invoke();
            mob.SetActive(false);
        }
    }

    private void SetMobHealth(float health)
    {
        _slider.value = health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
