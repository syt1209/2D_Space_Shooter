using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] // 0 = Triple Shot; 1 = Speed; 2 = Shield
    private int _powerupID;

    private Player _player;

    private Transform _playerTrans;

    [SerializeField]
    private AudioClip _audioClip;

    private bool _collected;

    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerTrans = GameObject.Find("Player").GetComponent<Transform>();
        _collected = false;

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if (_playerTrans == null)
        { Debug.LogError("Player transform is NULL."); }
    }

    // Update is called once per frame
    void Update()
    {
        if (_collected is false && Input.GetKeyDown(KeyCode.C)) 
        {
            AutoCollect();
        }

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            switch (_powerupID)
            {
                case 0:
                    _player.TripleShotActive();
                    break;
                case 1:
                    _player.SpeedBoostActive();
                    break;
                case 2:
                    _player.ShieldActive();
                    break;
                case 3:
                    _player.AmmoCollected();
                    break;
                case 4:
                    _player.DisableThrusterCollected();
                    break;
                case 5:
                    _player.LifeCollected();
                    break;
                case 6:
                    _player.MultiShotCollected();
                    break;
                default:
                    Debug.Log("Default case");
                    break;
            }

            Destroy(this.gameObject, 0.2f);
        }
    }

    private void AutoCollect()
    {
        while (_collected is false)
        { 
            transform.position = Vector3.MoveTowards(transform.position, _playerTrans.position, Time.deltaTime);
            if (transform.position == _playerTrans.position)
            {
                _collected = true;
            }
        }
    }
}
