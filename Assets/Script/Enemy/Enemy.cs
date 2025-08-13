using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;
    public int score = 1;

    void Start()
    {
        currentHP = maxHP;
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("HPŒ¸‚Á‚½");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("die");
        Destroy(gameObject);
        GameManager.Instance.AddScore(score);
    }
}
