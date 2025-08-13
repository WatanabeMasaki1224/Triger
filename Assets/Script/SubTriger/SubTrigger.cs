using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubTrigger
{
    public string name;
    public TriggerType type;
    public GameObject seardPrefab;
    public GameObject supaiderPrefab;
    public int trionCost;
    public int damage;

    public void Use(Vector3 firePoint, Vector3 direction)
    {
        switch (type)
        {
            case TriggerType.Seard:
                {
                    break;
                }
            case TriggerType.Spider:
                {
                    break;
                }
        }
    }

    public enum TriggerType
    {
       Seard,
       Spider,
    }
}
