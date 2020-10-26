using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerUps;
    
    private bool _stopSpawning = false;

    private int _numberOfPowerups = 4;
    
    // Start is called before the first frame update
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false) 
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 6.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }   
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false) 
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 6.0f, 0);
            Debug.Log(_numberOfPowerups);
            int randomPowerup = Random.Range(0,_numberOfPowerups);
            Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void ActivateLifePowerup()
    {
        _numberOfPowerups = 5;
    }

    public void DeactivateLifePowerup()
    {
        _numberOfPowerups = 4;
    }
}
