using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float _minigundamage;
    private float _rocketdamage;
    // Start is called before the first frame update
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

        //Acquire relevant data
        _minigundamage = _playerdamage.GetComputedDamage(EWeaponType.Minigun);
        _rocketdamage =  _playerdamage.GetComputedDamage(EWeaponType.Rocket);

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
        if(other.CompareTag("PlayerMinigun"))
        {
            Debug.Log("Taking minigun damage");
            MobDamaged(_minigundamage);
        }

        else if (other.CompareTag("PlayerRocket"))
        {
            Debug.Log("Taking rocket damage");
            MobDamaged(_rocketdamage);
        }
    }

    private void DestroyMob(float mobcurrenthealth)
    {
        if (mobcurrenthealth <= 0)
        {
            Debug.Log("Mob Destroyed");
            mob.SetActive(false);
        }
    }

    private void SetMobHealth(float health)
    {
        _slider.value = health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
