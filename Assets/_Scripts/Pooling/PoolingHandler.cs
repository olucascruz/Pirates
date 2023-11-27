using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingHandler : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxObjects;
    private List<GameObject> pooledObjects;
    private bool isOk = false;
    public bool IsOk { get { return isOk; } }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject instance;
        for (int i = 0; i < maxObjects; i++)
        {
            instance = Instantiate(prefab);
            instance.SetActive(false);
            pooledObjects.Add(instance);
        }
        isOk = true;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < maxObjects; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
