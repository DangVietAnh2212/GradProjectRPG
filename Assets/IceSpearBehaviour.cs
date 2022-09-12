using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpearBehaviour : SpellBehaviour
{
    const float baseAngle = 90f;
    float currentAngle = 90f;
    const float baseAOE = 2f;
    int projectileNumber = 7;
    public bool haveForked = false;
    static float childMinDamage;
    static float childMaxDamage;
    static float childSpeed;
    static float childDuration;
    static float childAOE;
    protected override void Start()
    {
        if (!haveForked)
        {
            childMinDamage = localMinDamage = minDamage;
            childMaxDamage = localMaxDamage = maxDamage;
            childSpeed = localSpeed = speed;
            childDuration = localDuration = lifeDuration;
            childAOE = localAOE = AOE;
        }
        if (haveForked)
        {
            localMinDamage = childMinDamage;
            localMaxDamage = childMaxDamage;
            localSpeed = childSpeed;
            localDuration = childDuration;
            localAOE = childAOE;
        }
        startTime = Time.time;
    }
    protected override void OnLifeEnd()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        if (!haveForked)
        {
            currentAngle = baseAngle * localAOE / baseAOE;
            GameObject firstSpear = Instantiate(gameObject, transform.position, Quaternion.LookRotation(transform.forward));
            firstSpear.transform.Rotate(Vector3.up * -currentAngle / 2);
            firstSpear.GetComponent<IceSpearBehaviour>().haveForked = true;
            Vector3 startDirection = firstSpear.transform.forward;
            for (int i = 1; i < projectileNumber; i++)
            {
                GameObject spear = Instantiate(gameObject, transform.position, Quaternion.LookRotation(startDirection));
                spear.transform.Rotate(currentAngle * i * Vector3.up/projectileNumber);
                spear.GetComponent<IceSpearBehaviour>().haveForked = true;
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            OnLifeEnd();
        }

        if (other.CompareTag("Enemy"))
        {
            OnLifeEnd();
        }
    }
}
