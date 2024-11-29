using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FinalScore : MonoBehaviour
{

    [SerializeField] private TMP_Text _finalscore;
    [SerializeField] ScoreManager _score;

    private float _finalscorecount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _finalscore.text = "Score: " + _score._scorecount;
    }
}
