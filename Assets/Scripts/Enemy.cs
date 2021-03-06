﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField]
    private Player _player;
    private Transform _playerPos;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private GameObject _enemyShield;
   
    private WaveConfig _waveZigZag;
    private List<Transform> _wayPoints;
    private int _wayPointIndex = 0;


    private Animator _anim;
    private AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool _aggressive = false;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _speed = _waveZigZag.GetSpeed();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

       _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL.");
        }

        _wayPoints = _waveZigZag.GetWayPoints();
        transform.position = _wayPoints[_wayPointIndex].transform.position;

        if (_enemyShield != null)
        {
            GameObject enemyShield = Instantiate(_enemyShield, transform.position, Quaternion.identity);
            _enemyShield.SetActive(true);
            enemyShield.transform.SetParent(this.gameObject.transform);
        }
    }

    void Update()
    {
        if (transform.tag == "Aggressive" && _aggressive is true)
        {
            AggresiveAttack();
        }
        else
        { MovementOnPath(); }
        

        if (Time.time > _canFire)
        {

            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (Laser laser in lasers)
            {
                laser.AssignEnemy();
            }
        }

    }

    public void SetWaveZigZag(WaveConfig waveZigZag) 
    {
        this._waveZigZag = waveZigZag;
    }

    private void MovementOnPath()
    {
        var targetPosition = _wayPoints[_wayPointIndex+1].transform.position;
        var movementThisFrame = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);

        if (transform.position == targetPosition)
        {
                _wayPointIndex++;
        }

        if (_wayPointIndex == _wayPoints.Count-1)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);

            if (_player != null)
            {
                _player.UpdateScore(10);
            }

            if (_enemyShield == null)
            {
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0; // freeze at the position to avoid further colliding with player 
                _audioSource.Play();

                Destroy(GetComponent<Collider2D>());

                Destroy(this.gameObject, 2.8f);
            }

            if (_enemyShield != null)
            {
                Destroy(this.gameObject.transform.GetChild(0).gameObject);
                _enemyShield = null;
                return;
            }
        
        }

        if (collision.tag == "Player" && transform.tag != "Aggressive")
        {
            Player player = collision.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                player.UpdateScore(10);
            }
            
            if (_enemyShield == null)
            {
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f); 
            }

            if (_enemyShield != null)
            {
                Destroy(this.gameObject.transform.GetChild(0).gameObject);
                _enemyShield = null;
                return;
            }

        }

        if (collision.tag == "Player" && transform.tag == "Aggressive" && this.transform.childCount > 0)
        {
            
             _aggressive = true;
            Destroy(this.transform.GetChild(0).gameObject);

        }

        if (collision.tag == "Player" && transform.tag == "Aggressive" && this.transform.childCount == 0)
        {
            Debug.Log("Destory self and damage player");
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _player.Damage();
            _player.UpdateScore(10);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

    }

    private void AggresiveAttack() 
    {
        if (_player != null)
        {
            Transform target = _player.transform;
            float attackSpeed = 30.0f;
            transform.LookAt(target);
            transform.Translate(Vector3.forward *attackSpeed* Time.deltaTime);
        }
    }

}
