using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutantBoss : Enemy
{
    float baseBossExp = 2000f;
    bool detectPlayer = false;
    float startTimeTilNextSkill;
    float timeTilNextSkill = 6f;
    public float stompRadius;
    public float roarRadius;
    IEnumerator patrolPointCoroutine;

    private void Awake()
    {
        minMaxHealth = new Vector2(1500f, 2500f);
        minMaxBaseDmg = new Vector2(200f, 300f);
        minMaxDef = new Vector2(200f, 300f);
    }
    void Start()
    {
        monsterType = MonsterType.Boss;
        currentHealth = maxHealth;
        healthText.text = $"{currentHealth:0}/{maxHealth:0}";
        healthBar.SetMaxHealth((int)maxHealth);
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5f;
        stoppingDistance = 2f;
        attackRange = 1.3f;
        stompRadius = 5f;
        roarRadius = 10f;
        patrolPointCoroutine = RandomPoint(8f, 5f);
        StartCoroutine(BossBehaviour());
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(agent != null)
        {
            if(distanceToPlayer > detectionRadius && !isPatrolling &&
                distanceToPlayer <= updatedAnimationRadius &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("MainLayer.Roar") &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("MainLayer.Stomp"))
            {
                StopFollowingPlayer();
                StartCoroutine(patrolPointCoroutine);
                isPatrolling = true;
            }

            if(distanceToPlayer <= detectionRadius)
            {
                StopCoroutine(patrolPointCoroutine);
                isPatrolling = false;
                FollowPlayer(player.transform);
                FaceTarget();
                if (distanceToPlayer <= stoppingDistance)
                {
                    animator.SetBool("isAttacking", true);
                }
                if (distanceToPlayer > stoppingDistance)
                {
                    animator.SetBool("isAttacking", false);
                }
                if(Time.time - startTimeTilNextSkill >= timeTilNextSkill)
                {
                    startTimeTilNextSkill = Time.time;
                    //agent.updatePosition = false;
                    int randomNextSkill = Random.Range(0, 2);
                    switch (randomNextSkill)
                    {
                        case 0:
                            animator.SetTrigger("doRoar");
                            break;
                        case 1:
                            animator.SetTrigger("doStomp");
                            break;
                    }
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("MainLayer.Roar") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("MainLayer.Stomp"))
                agent.isStopped = true;
            else
                agent.isStopped = false;
        }

        if (!isDead && currentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            FindObjectOfType<AudioManager>().Play("MutantDie");
            SpawnItem(3);
            player.GetComponent<Level>().currentExp += (int)(baseBossExp * (1 + spawner.difficulty));
            isDead = true;
            startDespawntime = Time.time;
            agent.isStopped = true;
            agent.updatePosition = false;
            agent = null;
            StopAllCoroutines();
            animator.SetBool("isDead", true);
        }

        if (isDead && Time.time - startDespawntime >= despawnTime)
        {
            if (spawner.difficulty <= 1f)
                spawner.difficulty += 0.25f;
            spawner.spawnNumber++;
            Destroy(gameObject);
        }
    }

    IEnumerator BossBehaviour()
    {
        while (!isDead)
        {
            while (isPatrolling)
            {
                MoveTo(patrolPoint);
                yield return null;
            }
            if (detectPlayer)
            {
                startTimeTilNextSkill = Time.time;
                detectPlayer = false;
                print("Start time: " + startTimeTilNextSkill);
            }
                
            yield return null;
        } 
    }
}
