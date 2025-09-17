using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainTrigger
{
    public string name;
    public TriggerType type;
    public GameObject bulletPrefab;  // 弾プレハブ（球攻撃用）
    public GameObject slashPrefab;       // 剣攻撃用エフェクトや判定用
    public float speed;
    public int trionCost;
    public int damage;
    public float coolTime = 1f;

    [HideInInspector] public float timer = 0f; // 現在のクールタイム

    public bool CanUse()
    {
        return timer <= 0f;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (timer > 0f)
            timer -= deltaTime;
    }

    public void Use(Vector3 firePoint,Vector3 direction)
    {
        if (!CanUse()) return; // クールタイム中なら発射できない

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
        timer = coolTime; // 発射後にタイマーをセット
    }
}
public enum TriggerType
{
    Asteroid, // 直進弾
    Hound,    // 追尾弾
    Meteor,   // 爆発弾
    Slash,    // 剣
}
