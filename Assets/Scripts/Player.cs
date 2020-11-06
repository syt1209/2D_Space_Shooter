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
    private GameObject _multiShotPrefab;
    [SerializeField]
    private GameObject[] _engineDamage;
    

    [SerializeField]
    private float _fireRate = 0.3f;
    private float _nextFire = -1.0f;

    [SerializeField]
    private int _lives = 3, _score, _ammoCount = 15;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private CameraShake _cameraShake;

    [SerializeField]
    private bool _isTripleShotActive = false, _isSpeedBoosted = false, _isShieldActive = false, _isAmmoCollected = false, _isLifeCollected = false, 
        _isMultiShotCollected=false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldStrength = 3;

    [SerializeField]
    private AudioClip _laserAudio;

    private AudioSource _audioSource;

    private Renderer _renderer;


    // Start is called before the first frame update
    void Start()
    {
        //take the current position and start at a new position
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _cameraShake = GameObject.Find("Main_Camera").GetComponent<CameraShake>();
        _audioSource = GetComponent<AudioSource>();
        _renderer = _shieldVisualizer.GetComponent<Renderer>();

        if (_spawnManager == null) 
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if (_uiManager == null) 
        {
            Debug.LogError("UI Manager is NULL.");
        }

        if (_cameraShake == null)
        {
            Debug.LogError("Camera Shake is NULL.");
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
            if (_ammoCount > 0)
            { 
                Shooting(); 
            }
            UpdateAmmo();
        }

        _uiManager.CurrentAmmo(_ammoCount);

        if (_isSpeedBoosted == false)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _speed <= 7.0f)
            {
                Thrusters();
            }
            else if (_speed > 3.5f)
            {
                ThrusterCoolDown();
            }
        }

        LifeVisualUpdate();
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
        if (_isTripleShotActive == true && _isMultiShotCollected == false)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else if (_isMultiShotCollected == true)
        {
            Instantiate(_multiShotPrefab, transform.position, Quaternion.identity);
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
            
            _shieldStrength--;

            switch (_shieldStrength) 
            {
                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
                case 1:
                    _renderer.material.color = Color.red;
                    break;
                case 2:
                    _renderer.material.color = Color.yellow;
                    break;
            }

            return;
        }

        StartCoroutine(_cameraShake.ShakeCamera(0.5f));
        _lives-=1;
        _spawnManager.ActivateLifePowerup();
    }

    private void LifeVisualUpdate()
    {
        if (_lives < 3 && _isLifeCollected == true)
        {
            _lives += 1;
            _isLifeCollected = false;
        }
        switch (_lives)
        {
            case 3:
                _engineDamage[0].SetActive(false);
                _engineDamage[1].SetActive(false);
                _spawnManager.DeactivateLifePowerup();
                break;
            case 2:
                _engineDamage[0].SetActive(true);
                _engineDamage[1].SetActive(false);
                break;
            case 1:
                _engineDamage[1].SetActive(true);
                break;
            default:
                _lives = 0;
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
                break;

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
        _renderer.material.color = Color.cyan;
        _shieldStrength = 3;

    }

    public void AmmoCollected()
    {
        _isAmmoCollected = true;
        _ammoCount = 15;
    }

    private void UpdateAmmo()
    {
        if (_ammoCount > 0)
        {
            _ammoCount--;
        }
        else 
        {
            _ammoCount = 0;
        }
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
        _uiManager.SetSlider(_speed);
    }

    private void ThrusterCoolDown()
    {
        _speed = _speed - _acceleration * Time.deltaTime;
        _uiManager.SetSlider(_speed);
    }


    public void LifeCollected()
    {
        _isLifeCollected = true;   
    }

    public void MultiShotCollected()
    {
        _isMultiShotCollected = true;
        StartCoroutine(MultiShotPowerDown());
    }

    IEnumerator MultiShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _ammoCount = 15;
        _isMultiShotCollected = false;
    }
}
