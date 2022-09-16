using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    public static float minDamage;
    protected float localMinDamage;
    public static float maxDamage;
    protected float localMaxDamage;
    public static float speed;
    protected float localSpeed;
    public static float lifeDuration;
    protected float localDuration;
    public static float AOE;
    protected float localAOE;
    protected float startTime;

    public GameObject explosionPrefab;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        localMinDamage = minDamage;
        localMaxDamage = maxDamage;
        localSpeed = speed;
        localDuration = lifeDuration;
        localAOE = AOE;
        startTime = Time.time;
    }

    void Update()
    {
        transform.Translate(localSpeed * Time.deltaTime * transform.forward, Space.World);
        if (Time.time - startTime > localDuration)
        {
            OnLifeEnd();
        }
    }
    public static float RollDamage(float minDamage, float maxDamage)
    {
        return Random.Range(minDamage, maxDamage);
    }

    public static void AOEDamage(Vector3 center, float AOE, float minDamage, float maxDamage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, AOE / 2f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                float dmg = RollDamage(minDamage, maxDamage);
                enemy.TakeDamage(UtilityClass.DamageAfterDef(dmg, enemy.currentDefPoint));
            }
        }
    }
    protected abstract void OnLifeEnd();
}
