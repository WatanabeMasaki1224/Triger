using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundBullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private Transform target;
    public float range = 5f;
    public float lifeTime = 3f;

    public void Setup(Vector3 dir,float spd,int dmg)
    {
        speed = spd;
        damage = dmg;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        List<Transform> candidate = new List<Transform>();

        foreach (var hit in hits)
        {
            if(hit.CompareTag("Enemy"))
            {
                candidate.Add(hit.transform);
            }
        }

        int pickCount = Mathf.Min(3, candidate.Count);

        for (int i = 0; i < pickCount; i++)
        {
            Transform chosenTarget = candidate[Random.Range(0,candidate.Count)];
            GameObject bullet = Instantiate(gameObject, transform.position, Quaternion.identity);
            bullet.GetComponent<HoundBullet>().SetTarget(chosenTarget,speed,damage);
        }
        Destroy(gameObject);
    }

    public void SetTarget(Transform t, float spd, int dmg)
    {
        target = t;
        speed = spd;
        damage = dmg;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
        }

        else
        {
            transform.position = transform.up * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
