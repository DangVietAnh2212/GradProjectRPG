
using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    [HideInInspector]
    public static GameObject parent;
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
                bool isKilled = enemy.TakeDamage(UtilityClass.DamageAfterDef(dmg, enemy.currentDefPoint));
                /*if (isKilled)
                    parent.GetComponent<MainStats>().lastMonsterKill = enemy.monsterType;*/
                if (isKilled)
                    parent.GetComponent<QuestManager>().UpdateKillQuest(enemy.monsterType);
            }
        }
    }
    protected abstract void OnLifeEnd();
}
