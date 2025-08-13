using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainTrigger
{
    public string name;
    public TriggerType type;
    public GameObject bulletPrefab;  // �e�v���n�u�i���U���p�j
    public GameObject slashPrefab;       // ���U���p�G�t�F�N�g�┻��p
    public float speed;
    public int trionCost;
    public int damage;

    public void Use(Vector3 firePoint,Vector3 direction)
    {
        switch (type)
        {
            case TriggerType.Asteroid:
                GameObject asteroid = GameObject.Instantiate(bulletPrefab,firePoint,Quaternion.identity);
                asteroid.GetComponent<AsteroidBullet>().Setup(direction.normalized,speed,damage);
                break;

            case TriggerType.Hound:
                GameObject hound = GameObject.Instantiate(bulletPrefab, firePoint, Quaternion.identity);
                hound.GetComponent<HoundBullet>().Setup(direction.normalized, speed, damage);
                break;

            case TriggerType.Meteor:
                GameObject meteor = GameObject.Instantiate(bulletPrefab, firePoint, Quaternion.identity);
                meteor.GetComponent<MeteorBullet>().Setup(direction.normalized, speed, damage);
                break;

            case TriggerType.Slash:
                GameObject slash = GameObject.Instantiate(slashPrefab,firePoint,Quaternion.identity);
                slash.GetComponent<Slash>().Activate(direction, damage);
                break;
        }
    }
}
public enum TriggerType
{
    Asteroid, // ���i�e
    Hound,    // �ǔ��e
    Meteor,   // �����e
    Slash,    // ��
}
