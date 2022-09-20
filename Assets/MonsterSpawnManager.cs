using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public GameObject monsterToSpawn;
    public float difficulty;
    public int spawnNumber;
    public float spawnRadius;
    float startTime;
    float waitTimeBeforeSpawn;
    Vector2 minMaxHealth;
    Vector2 minMaxDef;
    Vector2 minMaxBaseDmg;
    [HideInInspector]
    public Camera mainCam;
    GameObject playerRef;
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void Start()
    {
        waitTimeBeforeSpawn = Random.Range(7f, 14f);
        mainCam = Camera.main;
        playerRef = FindObjectOfType<PlayerController>().gameObject;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnNumber > 0 && Time.time - startTime > waitTimeBeforeSpawn)
        {
            spawnNumber--;
            startTime = Time.time;
            float xRandom = Random.Range(-spawnRadius, spawnRadius);
            float zRandom = Random.Range(-spawnRadius, spawnRadius);
            Vector3 spawnPos = new Vector3(transform.position.x + xRandom, transform.position.y, transform.position.z + zRandom);
            GameObject newSpawn = Instantiate(monsterToSpawn, spawnPos, Quaternion.identity);
            minMaxHealth = newSpawn.GetComponent<Enemy>().minMaxHealth;
            minMaxDef = newSpawn.GetComponent<Enemy>().minMaxDef;
            minMaxBaseDmg = newSpawn.GetComponent<Enemy>().minMaxBaseDmg;
            newSpawn.GetComponent<Enemy>().maxHealth = Mathf.Lerp(minMaxHealth.x, minMaxHealth.y, difficulty);
            newSpawn.GetComponent<Enemy>().currentDefPoint = Mathf.Lerp(minMaxDef.x, minMaxDef.y, difficulty);
            newSpawn.GetComponent<Enemy>().baseDamage = Mathf.Lerp(minMaxBaseDmg.x, minMaxBaseDmg.y, difficulty);
            newSpawn.GetComponent<Enemy>().spawner = this;
            newSpawn.GetComponent<Enemy>().player = playerRef;
            newSpawn.GetComponentInChildren<BillBoard>().cam = mainCam;
        }
    }
}
