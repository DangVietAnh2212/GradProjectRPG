using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLightningBehaviour : SpellBehaviour
{
    const float baseAOE = 2f;
    float timeBetweenPulse = 0.5f;
    protected override void Start()
    {
        localMinDamage = minDamage;
        localMaxDamage = maxDamage;
        localSpeed = speed;
        localDuration = lifeDuration;
        localAOE = AOE;
        startTime = Time.time;
        StartCoroutine(PulseDamage());
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = Vector3.one * localAOE / baseAOE;
        }
    }
    protected override void OnLifeEnd()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        for (int i = 0; i < explosion.transform.childCount; i++)
        {
            explosion.transform.GetChild(i).transform.localScale = Vector3.one * localAOE / baseAOE;
        }
        AOEDamage(transform.position, AOE, localMinDamage, localMaxDamage);
        StopCoroutine(PulseDamage());   
        Destroy(gameObject);
    }

    IEnumerator PulseDamage()
    {
        while (true)
        {
            AOEDamage(transform.position, AOE, localMinDamage, localMaxDamage);
            yield return new WaitForSeconds(timeBetweenPulse);
        }
    }
}
