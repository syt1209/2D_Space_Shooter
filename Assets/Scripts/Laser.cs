using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 12.0f;

    private bool _isEnemy = false;
    
    // Update is called once per frame
    void Update()
    {
        if (_isEnemy == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // Destroy the laser clone once it travels out of the screen
        if (transform.position.y >= 5.25f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Destroy the laser clone once it travels out of the screen
        if (transform.position.y <= -5.25f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemy()
    {
        _isEnemy = true;
        Debug.Log(_isEnemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _isEnemy == true)
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
