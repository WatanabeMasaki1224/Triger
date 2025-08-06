using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector3 direction;
    
    public void Setup(Vector3 dir,float spd,int dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
