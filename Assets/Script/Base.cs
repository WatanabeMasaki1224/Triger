using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Build.Content;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDmage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Destroyed();
        }

    }

    void Destroyed()
    {
        Debug.Log("‹’“_”j‰ó‚³‚ê‚½");

        if(GameManager.Instance != null)
        {
            GameManager.Instance.ReduceScore(1000);
        }

        Destroy(gameObject);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

}
