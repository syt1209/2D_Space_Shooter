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
}
