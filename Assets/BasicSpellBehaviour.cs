using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellBehaviour : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float speed;
    public float lifeDuration;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
        lifeDuration = 2f;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        if (Time.time - startTime > lifeDuration)
        {
            OnLifeEnd();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            OnLifeEnd();
        }

        if (other.CompareTag("Enemy"))
        {
            
        }
           
    }

    private void OnLifeEnd()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
