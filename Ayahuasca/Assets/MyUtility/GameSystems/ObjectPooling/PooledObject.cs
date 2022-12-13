using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public ObjectPool owner;
    
}
public static class PooledGameObjectExtensions
{
    public static void ReturnToPool(this GameObject gameObject)
    {
        var pooledObject = gameObject.GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            pooledObject.owner.ReturnObject(gameObject);
        }

    }



}
