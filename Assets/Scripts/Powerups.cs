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

    [SerializeField]
    private AudioClip _audioClip;

    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
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
                default:
                    Debug.Log("Default case");
                    break;
            }

            Destroy(this.gameObject, 0.2f);
        }
    }
}
