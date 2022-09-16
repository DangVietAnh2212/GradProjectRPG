using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehaviour : SpellBehaviour
{
    // Update is called once per frame

    const float baseAOE = 2f;
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

    protected override void OnLifeEnd()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //explosion.transform.GetChild(0).localScale = Vector3.one * 10;
        AOEDamage(transform.position, AOE, localMinDamage, localMaxDamage);
        for (int i = 0; i < explosion.transform.childCount; i++)
        {
            explosion.transform.GetChild(i).transform.localScale = Vector3.one * localAOE / baseAOE;
        }
        Destroy(gameObject);
    }
}
