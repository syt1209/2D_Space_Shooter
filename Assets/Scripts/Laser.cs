using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    
    // Update is called once per frame
    void Update()
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
}
