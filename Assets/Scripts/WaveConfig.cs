using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Movement Configuration")]
public class WaveConfig : ScriptableObject {
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _pathPrefab;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _numOfEnemies = 5;
    [SerializeField] private float _spawnInterval = 0.5f;

    public List<Transform> GetWayPoints() 
    {
        var waveWayPoints = new List<Transform>();
        foreach (Transform child in _pathPrefab.transform)
        {
            waveWayPoints.Add(child);
        }
        return waveWayPoints;
    }

    public GameObject GetEnemyPrefab() { return _enemyPrefab; }
    public GameObject GetPathPrefab() { return _pathPrefab; }
    public float GetSpeed() { return _speed; }
    public int GetNumOfEnemies() { return _numOfEnemies; }
    public float GetSpawnInterval() { return _spawnInterval; }
}
