using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public void Shoot(int damage, GameObject shipGameObject)
    {
        GameObject bulletPoolObj = GameObject.Find("BulletPool");
        if (bulletPoolObj)
        {
            PoolingHandler bulletPool;
            bulletPoolObj.TryGetComponent(out bulletPool);
            if (bulletPool != null) { 
                GameObject bullet = bulletPool.GetPooledObject();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                bullet.TryGetComponent(out Bullet bulletInstace);
                bulletInstace.OriginObj = shipGameObject;
                bulletInstace.Damage = damage;
                bullet.SetActive(true);
            }
        }
    }
}
