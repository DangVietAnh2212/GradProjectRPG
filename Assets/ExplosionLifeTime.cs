using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLifeTime : MonoBehaviour
{
    float lifeTime;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 2f;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > lifeTime)
            Destroy(gameObject);    
    }
}
