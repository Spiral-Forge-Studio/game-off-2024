using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private List<Buff> activeBuffs = new List<Buff>();
    public PlayerKCC playerKCC;
    public PlayerStatusSO playerStats;
    public float sphereCastInterval = 0.0001f;

    private float sphereCastTimer = 0f;
    private GameObject toBeBuffed;
    private BuffSpawner buffSpawner;
    public PlayerStatusManager playerStatusManager;



    void PerformSphereCast()
    {
        float radius = 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, radius);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                toBeBuffed = hit.collider.gameObject;
                string Buffname = "MinigunMagazineBuff";
                Debug.Log("Buff named " + Buffname+ " collided with an object with the tag: " + hit.collider.gameObject.tag);
                Buff chosenBuff = BuffRegistry.GetBuff("MinigunMagazineBuff");
                chosenBuff.UpdateBuffValues(Buff.BuffType.Flat, Buff.Rarity.Common, 10, 10, 0);
                chosenBuff.ApplyBuff(toBeBuffed);
                //MinigunMagazineBuff MinigunmagazineSizeBuff = toBeBuffed.AddComponent<MinigunMagazineBuff>();
                //MinigunmagazineSizeBuff.Initialize(playerStats, 0f, MinigunMagazineBuff.BuffType.Flat, MinigunMagazineBuff.Rarity.Common, 10f, 5f, 0.2f);
                //HpBuff hpBuff = toBeBuffed.AddComponent<HpBuff>();
                //ShieldBuff ShieldBuff = toBeBuffed.AddComponent<ShieldBuff>();
                //hpBuff.Initialize(playerStats, 0f, HpBuff.BuffType.Flat, HpBuff.Rarity.Common, 10f, 5f, 0.2f);
                //ShieldBuff.Initialize(playerStats, 0f, ShieldBuff.BuffType.Flat, ShieldBuff.Rarity.Common, 100f, 5f, 0.2f);
                AddBuff(chosenBuff);
                Destroy(this.gameObject);
            }
        }
    }

    private void Awake()
    {
        BuffRegistry.InitializeBuffs(playerStats);
        playerKCC = FindObjectOfType<PlayerKCC>();
        buffSpawner = FindAnyObjectByType<BuffSpawner>();
        playerStatusManager = FindAnyObjectByType<PlayerStatusManager>();
        sphereCastTimer = sphereCastInterval;
    }
    
    private void Update()
    {
        PerformSphereCast();
    }

    public void AddBuff(Buff newBuff)
    {
        Buff existingBuff = activeBuffs.Find(b => b.GetType() == newBuff.GetType());

        if (existingBuff == null)
        {
            activeBuffs.Add(newBuff);
            newBuff.StartBuff(toBeBuffed);
        }
        else
        {
            // Optionally update the existing buff
        }
    }

    public void RemoveBuff(Buff buff)
    {
        activeBuffs.Remove(buff);
    }

    
}
