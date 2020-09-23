using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 6.0f, 0);
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
            
            Destroy(this.gameObject);
        }

        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                player.UpdateScore(10);
            }

            Destroy(this.gameObject);
        }

    }

}
