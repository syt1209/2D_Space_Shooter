using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private Player _player;

    private Animator _anim;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

       _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL.");
        }


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

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0; // freeze at the position to avoid further colliding with player 
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
            Destroy(this.gameObject, 2.8f);
        }

    }

}
