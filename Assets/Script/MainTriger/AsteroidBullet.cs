using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector3 direction;
    public float lifeTime = 1f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(Vector3 dir,float spd,int dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        Destroy(gameObject, lifeTime);
        directionBullet();
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

    void directionBullet()
    {
        if(direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg + 180f; 
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
