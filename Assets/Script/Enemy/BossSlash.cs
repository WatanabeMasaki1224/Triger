using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlash : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5;
    public float lifeTime = 0.5f;
    private Vector3 direction;

    public void Setup(Vector3 dir,int dmg)
    {
        direction = dir.normalized;
        damage =dmg;
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
        }
        else if (other.CompareTag("Base"))
        {
            BaseController baseObj = other.GetComponent<BaseController>();
            if (baseObj != null)
            {
                baseObj.TakeDamage(damage);
            }
        }
    }
}
