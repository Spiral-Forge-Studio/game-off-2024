using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetbuffs : MonoBehaviour
{

    [SerializeField] private PlayerStatusSO _playerstats;
    [SerializeField] private PlayerStatusSO _bossstats;
    // Start is called before the first frame update
    
    public void ResetBuffs()
    {
        _playerstats.ResetMultipliersAndFlatBonuses();
        _bossstats.ResetMultipliersAndFlatBonuses();
    }
}
