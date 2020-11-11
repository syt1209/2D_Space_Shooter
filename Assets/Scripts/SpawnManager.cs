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

    private int _numberOfPowerups = 5; //normal powerups
    
    // Start is called before the first frame update
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
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
            int randomPowerup = Random.Range(0,_numberOfPowerups);
            Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(10.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 6.0f, 0);
            Instantiate(_powerUps[6], posToSpawn, Quaternion.identity);


            yield return new WaitForSeconds(Random.Range(20.0f, 30.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void ActivateLifePowerup()
    {
        _numberOfPowerups = 6;
    }

    public void DeactivateLifePowerup()
    {
        _numberOfPowerups = 5;
    }
}
