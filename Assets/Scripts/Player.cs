using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.3f;
    private float _nextFire = -1.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        //take the current position and start at a new position
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            Shooting();
        }
    }

    void CalculateMovement()
    {
        // Get moving direction input from the User, using Input manager
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3 (horizontalInput, verticalInput, 0);

        // Translate the object using transform.Translate
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 0), 0);

        // Wrap the player game object in the X direction
        if (transform.position.x > 9.6f)
        {
            transform.position = new Vector3 (-9.6f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.6f)
        {
            transform.position = new Vector3(9.6f, transform.position.y, 0);
        }
    }

    void Shooting()
    {
       
        _nextFire = Time.time + _fireRate; // Laser shooting cool down
        Instantiate(_laserPrefab, transform.position + new Vector3(0,0.8f,0), Quaternion.identity);
        
    }

    public void Damage()
    {
        _lives-=1;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

}
