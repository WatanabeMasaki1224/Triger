using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumster : MonoBehaviour
{
    public int speed = 2;
    private Transform player;
    public int damage = 1;

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
            else
            {
                // Player���܂�������Ȃ��Ȃ珈�������I��
                return;
            }
        }


        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerCtrl = other.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                playerCtrl.TakeDamage(damage);
            }

            // �����i�X�R�A�͓���Ȃ��j
            Destroy(gameObject);
        }
    }
}
