using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellBehaviour : MonoBehaviour
{
    MainStats mainStats;
    public GameObject explosionPrefab;
    public float speed;
    public float lifeDuration;
    float startTime;
    float baseDamage;
    // Start is called before the first frame update
    void Start()
    {
        mainStats = FindObjectOfType<MainStats>();
        speed = 10f;
        lifeDuration = 1f;
        startTime = Time.time;
        baseDamage = 5f;
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
            Enemy enemy = other.GetComponent<Enemy>();
            float dmg = mainStats.DamageAfterStrBonus(baseDamage);
            enemy.TakeDamage(UtilityClass.DamageAfterDef(dmg, enemy.currentDefPoint));
            OnLifeEnd();
        }
           
    }

    private void OnLifeEnd()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
