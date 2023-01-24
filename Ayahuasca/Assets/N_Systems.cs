using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class N_Systems : MonoBehaviour
{
    public static N_Systems Instance;
    [Header("Object pools")]
    [SerializeField] List<ObjectPoolStruct> ObjectPools;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
    }

    public ObjectPool GetObjectPool(string PoolName)
    {
        return ObjectPools.First(d => d.PoolName == PoolName).Pool;
    }
}
[Serializable]
public struct ObjectPoolStruct
{
    public string PoolName;
    public ObjectPool Pool;
}
