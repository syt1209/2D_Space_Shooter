using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private List<WaveConfig> _waveZigZags; // waves with increasing number of enemies moving in ZigZag
    [SerializeField]
    private int _startingWaveIndex = 0;
    
    private bool _stopSpawning = false;

    private int _numberOfPowerups = 5; //normal powerups
    
    // Start is called before the first frame update
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnAllZigZagWaves());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }

    private IEnumerator SpawnAllZigZagWaves()
    {
        while (_stopSpawning == false)
        {
            for (int waveIndex = _startingWaveIndex; waveIndex < _waveZigZags.Count; waveIndex++)
            {
                var currentWave = _waveZigZags[waveIndex];
                yield return StartCoroutine(SpawnEnemyZigZagRoutine(currentWave));
            }
        }
    }
    private IEnumerator SpawnEnemyZigZagRoutine(WaveConfig waveZigZag)
    {
        yield return new WaitForSeconds(3.0f);
        
        for (int enemyCount = 0; enemyCount < waveZigZag.GetNumOfEnemies(); enemyCount++)
        {
             var newEnemy = Instantiate(
                    waveZigZag.GetEnemyPrefab(),
                    waveZigZag.GetWayPoints()[0].transform.position,
                    Quaternion.identity
                    );
             newEnemy.GetComponent<Enemy>().SetWaveZigZag(waveZigZag);
             yield return new WaitForSeconds(waveZigZag.GetSpawnInterval());
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
