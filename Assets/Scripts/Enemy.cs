using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private WaveConfig _waveConfig;
    private List<Transform> _wayPoints;
    private int _wayPointIndex = 0;


    private Animator _anim;
    private AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

       _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL.");
        }

        _wayPoints = _waveConfig.GetWayPoints();
        transform.position = _wayPoints[_wayPointIndex].transform.position;
    }

    void Update()
    {
        MovementOnPath();

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

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0; // freeze at the position to avoid further colliding with player 
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject, 2.8f);
        }

        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                player.UpdateScore(10);
            }
           _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject, 2.8f);
        }

    }

}
