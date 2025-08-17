using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 6;
    public int damage = 5;
    public float lifeTime = 3f;
    private Vector3 direction;

    public void Setup(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject,lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        else if (other.CompareTag("Base"))
        {
            BaseController baseObj = other.GetComponent<BaseController>();
            if (baseObj != null)
            {
                baseObj.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
