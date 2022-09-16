using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject roarEffectPrefab;
    public GameObject slamEffectPrefab;
    public Transform stompPointPos;
    public Transform attackPointPos;
    public Enemy enemyStat;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPointPos.position, enemyStat.attackRange);
        if (enemyStat is MutantBoss)
        {
            Gizmos.DrawWireSphere(transform.position, ((MutantBoss)enemyStat).roarRadius);
            Gizmos.DrawWireSphere(stompPointPos.position, ((MutantBoss)enemyStat).stompRadius);
        }
        
    }
    public void MeleeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPointPos.position, enemyStat.attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                float playerDef = collider.GetComponent<MainStats>().currentDefPoint;
                collider.GetComponent<MainStats>().TakeDamage(UtilityClass.DamageAfterDef(enemyStat.baseDamage, playerDef));
            }
        }
    }

    public void RoarAttack()
    {
        Instantiate(roarEffectPrefab, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(enemyStat.gameObject.transform.position, ((MutantBoss)enemyStat).roarRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                float playerDef = collider.GetComponent<MainStats>().currentDefPoint;
                collider.GetComponent<MainStats>().TakeDamage(UtilityClass.DamageAfterDef(enemyStat.baseDamage, playerDef));
                print("Player get rooted status");
            }
        }
    }

    public void StompAttack()
    {
        Instantiate(slamEffectPrefab, stompPointPos.position + Vector3.up*0.1f, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(stompPointPos.position, ((MutantBoss)enemyStat).stompRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                float playerDef = collider.GetComponent<MainStats>().currentDefPoint;
                collider.GetComponent<MainStats>().TakeDamage(UtilityClass.DamageAfterDef(enemyStat.baseDamage * 5, playerDef));
            }
        }
    }
}
