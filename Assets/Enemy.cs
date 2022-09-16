using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    protected float startDespawntime;
    protected float despawnTime = 4f;
    protected bool isDead = false;
    public float currentDefPoint = 50f;
    public float maxHealth = 300f;
    public float currentHealth;
    public float baseDamage = 100f;
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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = $"{currentHealth:0}/{maxHealth:0}";
        healthBar.SetMaxHealth((int)maxHealth);
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        StartCoroutine(RandomPoint());
        isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent != null)
        {
            if (Vector3.Distance(player.transform.position, agent.transform.position) <= detectionRadius)
            {
                StopCoroutine(RandomPoint());
                isPatrolling = false;
                FollowPlayer(player.transform);
                FaceTarget();
            }

            if (Vector3.Distance(player.transform.position, agent.transform.position) > detectionRadius)
            {
                if (!isPatrolling)
                {
                    StartCoroutine(RandomPoint());
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

    protected IEnumerator RandomPoint()
    {
        while (true)
        {
            patrolPoint = new Vector3(Random.Range(transform.position.x - 10f,transform.position.x + 10f), 
                0f, 
                Random.Range(transform.position.z - 10f,transform.position.z + 10f));
            yield return new WaitForSeconds(5f);
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

    public void TakeDamage(float dmgTaken)
    {
        currentHealth -= UtilityClass.DamageAfterDef(dmgTaken, currentDefPoint);
        if (currentHealth < 0)
            currentHealth = 0;
    }
}
