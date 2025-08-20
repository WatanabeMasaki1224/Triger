using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrionItem : MonoBehaviour
{
    public int cure = 20;
    public bool destroyOnCollect = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CureTrion(cure);
            }

            if (destroyOnCollect)
                Destroy(gameObject);
        }
    
    }
}
