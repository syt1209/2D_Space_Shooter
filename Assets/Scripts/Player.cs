using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.0f;
    private float _acceleration = 1f;

    [SerializeField]
    private GameObject _laserPrefab; 
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject[] _engineDamage;

    [SerializeField]
    private float _fireRate = 0.3f;
    private float _nextFire = -1.0f;

    [SerializeField]
    private int _lives = 3, _score;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField]
    private bool _isTripleShotActive = false, _isSpeedBoosted = false, _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldStrength = 3;

    [SerializeField]
    private AudioClip _laserAudio;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //take the current position and start at a new position
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null) 
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if (_uiManager == null) 
        {
            Debug.LogError("UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio source on player is NULL.");
        }
        else 
        {
            _audioSource.clip = _laserAudio;
        }


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            Shooting();
        }

        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedBoosted == false)
        {
            Thrusters();
        }
        else if (_isSpeedBoosted == false)
        {
            _speed = 3.5f;
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

        // Tripleshot
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play(0);
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            Renderer renderer = _shieldVisualizer.GetComponent<Renderer>();
            _shieldStrength--;

            switch (_shieldStrength) 
            {
                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
                case 1:
                    renderer.material.color = Color.red;
                    break;
                case 2:
                    renderer.material.color = Color.yellow;
                    break;
            }

            return;
        }
        
        _lives-=1;
        
       

        if (_lives == 2)
        {
            _engineDamage[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            _engineDamage[1].SetActive(true);
        }
        else if (_lives < 1)
        {
            _lives = 0;
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

        _uiManager.CurrentLife(_lives);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoosted = true;
        _speed *= _speedMultiplier;

        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown() 
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoosted = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);

    }

    public void UpdateScore(int points)
    {
        _score += points;
        _uiManager.CurrentScore(_score);
    }

    //PhaseI-Thruster, apply acceleration when pressing left shift key
    private void Thrusters()
    {
        _speed = _speed + _acceleration * Time.deltaTime;
    }

}
