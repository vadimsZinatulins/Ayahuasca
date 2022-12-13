using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("QueueBack",2);
    }

    void QueueBack()
    {
        gameObject.ReturnToPool();


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
