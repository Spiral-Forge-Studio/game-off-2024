using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text _score;
    public float _scorecount;
    void Start()
    {
        
    }

    
    void Update()
    {
        _score.text = "Score: " + Mathf.Round(_scorecount);

    }

    public void UpdateScore(float score)
    {
        _scorecount += score;
    }
}
