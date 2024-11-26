using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MobSpawnerOld : MonoBehaviour

{
    [SerializeField] private List<Enemy> _enemyList = new List<Enemy>();
    [SerializeField] private List<GameObject> _enemywave = new List<GameObject>();
    [SerializeField] private float _currentwave = 1f;
    [SerializeField] private float _wavevalue;
    [SerializeField] private float _wavedifficulty = 10f;


    public void Start()
    {
        GenerateWave();
    }

    public void GenerateWave()
    {
        _wavevalue = _currentwave * _wavedifficulty;
        GenerateMobs();
    }

    public void GenerateMobs()
    {
        List<GameObject> _generatedenemies = new List<GameObject>();
        while(_wavevalue > 0)
        {
            int randEnemyID = Random.Range(0, _enemyList.Count);
            float randEnemyCost = _enemyList[randEnemyID]._cost;

            if (_wavevalue - randEnemyCost >= 0)
            {
                _generatedenemies.Add(_enemyList[randEnemyID]._mobprefab);
                _wavevalue -= randEnemyCost;
            }

            else if(_wavevalue <= 0)
            {
                break;
            }
        
        }
        _enemywave.Clear();
        _enemywave = _generatedenemies;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject _mobprefab;
    public float _cost;
}