using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public enum MonsterType
{
    None,
    Zombie,
    Boss
}
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public MonsterType monsterType;
    public GroundItem itemSpawnPref;
    public ItemDatabaseSO itemDatabase;
    public Vector2 minMaxHealth = new Vector2(100f, 300f);
    public Vector2 minMaxDef = new Vector2(10f, 75f);
    public Vector2 minMaxBaseDmg = new Vector2(50f, 150f);
    public MonsterSpawnManager spawner;
    protected float startDespawntime;
    protected float despawnTime = 4f;
    protected bool isDead = false;
    public float currentDefPoint = 50f;
    public float maxHealth = 300f;
    public float currentHealth;
    public float baseDamage = 100f;
    [HideInInspector]
    public float attackRange = 0.3f;
    protected Animator animator;
    protected float detectionRadius = 15f;
    protected float stoppingDistance = 1.2f;
    protected Vector3 patrolPoint;
    protected bool isPatrolling = false;
    public GameObject player;
    protected NavMeshAgent agent;
    public EnemyHealthBar healthBar;
    public TextMeshProUGUI healthText;
    float baseExp = 50f;

    // Start is called before the first frame update
    void Start()
    {
        monsterType = MonsterType.Zombie;
        currentHealth = maxHealth;
        healthText.text = $"{currentHealth:0}/{maxHealth:0}";
        healthBar.SetMaxHealth((int)maxHealth);
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        attackRange = 0.3f;
        StartCoroutine(RandomPoint(10f, 5f));
        isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent != null)
        {
            if (Vector3.Distance(player.transform.position, agent.transform.position) <= detectionRadius)
            {
                StopCoroutine(RandomPoint(10f, 5f));
                isPatrolling = false;
                FollowPlayer(player.transform);
                FaceTarget();
            }

            if (Vector3.Distance(player.transform.position, agent.transform.position) > detectionRadius)
            {
                if (!isPatrolling)
                {
                    StartCoroutine(RandomPoint(10f, 5f));
                    isPatrolling = true;
                }
                StopFollowingPlayer();
                MoveTo(patrolPoint);
            }

            if (Vector3.Distance(player.transform.position, agent.transform.position) <= stoppingDistance)
            {
                agent.isStopped = true;
                FaceTarget();
                animator.SetTrigger("isAttacking");
            }

            if (Vector3.Distance(player.transform.position, agent.transform.position) > stoppingDistance)
            {
                animator.SetTrigger("toNormal");
            }
        }

        if(!isDead && currentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            FindObjectOfType<AudioManager>().Play("ZombieDie");
            if ((int)spawner.difficulty < 1)
                SpawnItem(1);
            if ((int)spawner.difficulty == 1)
                SpawnItem(2);
            player.GetComponent<Level>().currentExp += (int)(baseExp * (1 + spawner.difficulty));
            isDead = true;
            startDespawntime = Time.time;
            agent.isStopped = true;
            agent.updatePosition = false;
            agent = null;
            StopAllCoroutines();
            animator.SetBool("isDead", true);
        }

        if(isDead && Time.time - startDespawntime >= despawnTime)
        {
            spawner.spawnNumber++;
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if(currentHealth >= 0)
        {
            healthBar.SetHealth((int)currentHealth);
            healthText.text = $"{currentHealth:0}/{maxHealth:0}";
        }
    }

    protected IEnumerator RandomPoint(float rangeRadius, float timeBetweenPoint)
    {
        while (true)
        {
            patrolPoint = new Vector3(Random.Range(transform.position.x - rangeRadius, transform.position.x + rangeRadius),
                transform.position.y, 
                Random.Range(transform.position.z - rangeRadius, transform.position.z + rangeRadius));
            yield return new WaitForSeconds(timeBetweenPoint);
        }
    }
    public void MoveTo(Vector3 point)
    {
        agent.isStopped = false;
        agent.SetDestination(point);
    }
    public void FollowPlayer(Transform playerTransform)
    {
        agent.stoppingDistance = stoppingDistance;
        agent.updateRotation = false;
        MoveTo(playerTransform.position);
    }

    public void StopFollowingPlayer()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
    }

    public void FaceTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        //using new Vector3 to avoid looking up and down
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        //5f is the speed of the rotation (smooth the rotation)
    }
    /// <summary>
    /// Take dmg then return true if this enemy is dead or false if not
    /// </summary>
    /// <param name="dmgTaken"></param>
    /// <returns></returns>
    public bool TakeDamage(float dmgTaken)
    {
        currentHealth -= UtilityClass.DamageAfterDef(dmgTaken, currentDefPoint);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
        }
        return false;
    }

    protected void SpawnItem(int spawnNumb)
    {
        for (int i = 0; i < spawnNumb; i++)
        {
            int randomID = Random.Range(0, (int)(itemDatabase.itemSOs.Length * 1.3f));
            if (randomID < itemDatabase.itemSOs.Length)
            {
                GroundItem groundItem = Instantiate(itemSpawnPref, transform.position + Vector3.up * 1.5f, Quaternion.identity).GetComponent<GroundItem>();
                groundItem.item = itemDatabase.itemSOs[randomID];
                groundItem.gameObject.GetComponent<BillBoard>().cam = spawner.mainCam;
            }
        }
    }
}
