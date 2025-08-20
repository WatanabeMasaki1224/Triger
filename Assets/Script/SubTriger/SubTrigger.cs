using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubTrigger
{
    public string name;
    public TriggerType type;
    public GameObject baseShieldPrefab;
    public GameObject playerShieldPrefab;
    public int trionCost;
    public float coolTime = 1f;
    private GameObject activeShield; // åªç›ê∂ê¨Ç≥ÇÍÇƒÇ¢ÇÈÉVÅ[ÉãÉh



    public void UseContinuous(Vector3 firePoint)
    {
        if (activeShield == null)
        {
            switch (type)
            {
                case TriggerType.BaseShield:
                    activeShield = GameObject.Instantiate(baseShieldPrefab, firePoint, Quaternion.identity);
                    activeShield.transform.SetParent(GameObject.FindWithTag("Base").transform);
                    break;
                case TriggerType.PlayerShield:
                    activeShield = GameObject.Instantiate(playerShieldPrefab, firePoint, Quaternion.identity);
                    activeShield.transform.SetParent(GameObject.FindWithTag("Player").transform);
                    break;
            }
        }
    }

    public void Stop()
    {
        if (activeShield != null)
        {
            GameObject.Destroy(activeShield);
            activeShield = null;
        }
    }

    public enum TriggerType
    {
       BaseShield,
       PlayerShield,
    }
}
