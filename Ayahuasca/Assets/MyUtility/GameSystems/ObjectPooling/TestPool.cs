using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    [SerializeField]
    private ObjectPool pool;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {

        pool.GetObject();
        yield return new WaitForSeconds(1);
        StartCoroutine("Spawn");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
