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
            PoolingHandler bulletPool = bulletPoolObj.GetComponent<PoolingHandler>();
            if (bulletPool) { 
                GameObject bullet = bulletPool.GetPooledObject();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<Bullet>().OriginObj = shipGameObject;
                bullet.GetComponent<Bullet>().Damage = damage;

                bullet.SetActive(true);
            }
        }
    }
}
