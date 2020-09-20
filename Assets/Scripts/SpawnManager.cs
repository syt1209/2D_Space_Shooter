using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning;
    
    // Start is called before the first frame update
    void Start()
    {
        _stopSpawning = false;
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false) 
        {
            Vector3 enemy_spawning_position = new Vector3(Random.Range(-8.0f, 8.0f), 6.0f, 0);
            
            GameObject newEnemy = Instantiate(_enemyPrefab, enemy_spawning_position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }   
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
