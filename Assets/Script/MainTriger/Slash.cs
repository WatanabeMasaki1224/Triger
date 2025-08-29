using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float range = 1f;
    public float lifeTime = 0.1f;
    private int damage;

    public void Activate(Vector3 direction, int dmg)
    {
        damage = dmg;
        transform.position += direction.normalized * range * 0.5f;
        // Œü‚«‚ð‘µ‚¦‚é
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        Destroy(gameObject,lifeTime);

        //‚±‚¤‚°‚«”»’è
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(var hit in hits)
        {
            if(hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>()?.TakeDamage(damage);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
