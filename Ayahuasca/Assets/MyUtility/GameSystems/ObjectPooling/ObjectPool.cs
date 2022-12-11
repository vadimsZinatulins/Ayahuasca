using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolNotifier
{

    void OnEnqueuedToPool();
    void OnPoolObjectCreated();
    void OnDequeuedFromPool();


}
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private Queue<GameObject> inactiveObjects = new Queue<GameObject>();
    // Start is called before the first frame update

    public GameObject GetObject()
    {
        if (inactiveObjects.Count > 0)
        {
            var dequeuedObject = inactiveObjects.Dequeue();
            dequeuedObject.transform.parent = null; // puts the object in the root of the hierarchy.
            dequeuedObject.SetActive(true);
            var notifiers = dequeuedObject.GetComponents<IObjectPoolNotifier>();

            foreach( var notifier in notifiers)
            {
                notifier.OnDequeuedFromPool(); // notify all the components of that object that it has been enqueued.
            }

            return dequeuedObject;
        }
        else
        {
            var newObject = Instantiate(prefab);
            var poolTag = newObject.AddComponent<PooledObject>();
            poolTag.owner = this;

            // needs to implement PooledObject tag 
            var notifiers = newObject.GetComponents<IObjectPoolNotifier>();
            foreach (var notifier in notifiers)
            {
                notifier.OnPoolObjectCreated(); // notify all the components of that object that it has been enqueued.
            }
            return newObject;

        }

    }
    public void ReturnObject(GameObject pooledObject)
    {
        var notifiers = pooledObject.GetComponents<IObjectPoolNotifier>();

        foreach (var notifier in notifiers)
        {
            notifier.OnEnqueuedToPool();
        }
        pooledObject.SetActive(false);
        pooledObject.transform.parent = gameObject.transform;
        inactiveObjects.Enqueue(pooledObject);

    }
    


    }
