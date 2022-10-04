using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;

[RequireComponent(typeof(Level))]
public class MainStats : MonoBehaviour
{
    string savePath = "/Stats.save";
    [SerializeField]
    [HideInInspector]
    SavePlayerInfo savedInfo;
    [HideInInspector]
    public bool isDead = false;
    public GameObject statsSheetRef;
    bool hasStatsSheetInitiated = false;
    public event Action MainStatsEvent;
    public event Action OnPlayerDeath;
    public GameObject[] mainAttributeChanges;
    PlayerInventoryManager playerInventory;
    public GameObject healthText;
    public GameObject manaText;
    public GameObject healthUI;
    public GameObject manaUI;
    Image healthFill;
    Image manaFill;
    Level level;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public float maxMana;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentMana;
    [HideInInspector]
    public float healthRegen;
    [HideInInspector]
    public float manaRegen;
    public const float baseHealth = 100;
    public const float baseMana = 100;
    const int statsPerLevel = 12;
    public const int BaseMovemenSpeed = 5;
    [HideInInspector]
    public float currentMoveSpeed;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int currentAtkPoint;
    public int currentDefPoint;
    QuestManager questManager;

    public void LoadStats()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            savedInfo = (SavePlayerInfo)formatter.Deserialize(stream);
            stream.Close();
            baseStr = savedInfo.baseStr;
            baseDex = savedInfo.baseDex;
            baseInt = savedInfo.baseInt;
            if (level != null)
            {
                level.currentLevel = savedInfo.currentLevel;
                level.currentExp = savedInfo.currentExp;
                level.nextLevelExp = savedInfo.nextLevelExp;
                level.pointToAllocate = savedInfo.pointToAllocate;
            }
            if (questManager != null)
            {
                questManager.receivedQuests = savedInfo.savedQuests;
                QuestUI questUI = FindObjectOfType<QuestUI>();
                if (questUI != null)
                {
                    questUI.UpdateQuestUI();
                }
            }
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.nextPosition = new Vector3(savedInfo.x, savedInfo.y, savedInfo.z);
            ResetStats();
        }
    }

    public void SaveStats()
    {
        savedInfo = new SavePlayerInfo();
        savedInfo.baseStr = baseStr;
        savedInfo.baseDex = baseDex;
        savedInfo.baseInt = baseInt;
        savedInfo.currentLevel = level.currentLevel;
        savedInfo.currentExp = level.currentExp;
        savedInfo.nextLevelExp = level.nextLevelExp;
        savedInfo.pointToAllocate = level.pointToAllocate;
        savedInfo.x = transform.position.x;
        savedInfo.y = transform.position.y;
        savedInfo.z = transform.position.z;
        if (questManager != null)
            savedInfo.savedQuests = questManager.receivedQuests;
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, savedInfo);
        stream.Close();
        //Application.persistentDataPath = %userprofile%\AppData\LocalLow\<companyname>\<productname>
    }
    IEnumerator Regeneration()
    {
        while (true) 
        {
            currentHealth += healthRegen * Time.deltaTime;
            currentMana += manaRegen * Time.deltaTime;
            //print($"current health: {healthRegen}; current mana: {manaRegen}");
            yield return null;
        }
       
    }
    public void SetBeginningAttributes()
    {
        savedInfo = new SavePlayerInfo();
        savedInfo.baseStr = 6;
        savedInfo.baseDex = 6;
        savedInfo.baseInt = 12;
        savedInfo.currentLevel = 0;
        savedInfo.currentExp = 0;
        savedInfo.nextLevelExp = 0;
        savedInfo.pointToAllocate = 0;
    }
    private void Start()
    {
        level = GetComponent<Level>();
        questManager = GetComponent<QuestManager>();
        playerInventory = GetComponent<PlayerInventoryManager>();
        for (int i = 0; i < playerInventory.attributeTypes.Length; i++)
        {
            //print($"Register mod event: {playerInventory.attributeTypes[i].attributeType}");
            playerInventory.attributeTypes[i].value.RegisterModEvent(ResetStats);
        }
        ResetStats();
        level.LevelEvent += ResetStats;
        healthFill = healthUI.GetComponent<Image>();
        manaFill = manaUI.GetComponent<Image>();
        StartCoroutine(Regeneration());
    }

    private void Update()
    {
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentMana > maxMana)
            currentMana = maxMana;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            gameObject.GetComponentInChildren<Animator>().SetBool("isDead", true);
            StopAllCoroutines();
            if (OnPlayerDeath != null)
                OnPlayerDeath.Invoke();
        }

        if (currentMana <= 0)
            currentMana = 0;
    }

    private void LateUpdate()
    {
        if (!hasStatsSheetInitiated)
        {
            //Load player's base stats
            if (UtilityClass.loadSavedFiles)
            {
                LoadStats();
            }
            statsSheetRef.SetActive(false);
            hasStatsSheetInitiated = true;  
        }
        healthFill.fillAmount = currentHealth / maxHealth;
        manaFill.fillAmount = currentMana / maxMana;
        healthText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString("n0");
        manaText.GetComponent<TextMeshProUGUI>().text = currentMana.ToString("n0");
    }
    private void SetAttribute()
    {
        strength = baseStr + playerInventory.attributeTypes[0].value.ModifiedValue;
        dexterity = baseDex + playerInventory.attributeTypes[1].value.ModifiedValue;
        intelligence = baseInt + playerInventory.attributeTypes[2].value.ModifiedValue;
    }
   
    public void CalculatePlayerStats()
    {
        maxHealth = (baseHealth + statsPerLevel * level.currentLevel) + playerInventory.attributeTypes[3].value.ModifiedValue;
        healthRegen = UtilityClass.baseRegen * Mathf.Pow(UtilityClass.percentIncreasedPerStr, strength);
        maxMana = (baseMana + statsPerLevel * level.currentLevel) + playerInventory.attributeTypes[4].value.ModifiedValue;
        manaRegen = UtilityClass.baseRegen * Mathf.Pow(UtilityClass.percentIncreasedPerInt, intelligence);
        currentMoveSpeed = (int)(BaseMovemenSpeed * Mathf.Pow(UtilityClass.percentIncreasedPerDex, dexterity));
        GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
        currentAtkPoint = playerInventory.attributeTypes[5].value.ModifiedValue;
        currentDefPoint = playerInventory.attributeTypes[6].value.ModifiedValue;
    }

    public void ResetStats()
    {
        SetAttribute();
        CalculatePlayerStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
        if(MainStatsEvent != null)
            MainStatsEvent.Invoke();
    }

    public void AddBaseAttributes()
    {
        for (int i = 0; i < mainAttributeChanges.Length; i++)
        {
            if (mainAttributeChanges[i] == null)
            {
                Debug.Log("Null reference!!!");
                return;
            }
        }
        baseStr += Convert.ToInt32(mainAttributeChanges[0].GetComponent<TextMeshProUGUI>().text);
        baseDex += Convert.ToInt32(mainAttributeChanges[1].GetComponent<TextMeshProUGUI>().text);
        baseInt += Convert.ToInt32(mainAttributeChanges[2].GetComponent<TextMeshProUGUI>().text);
        ResetStats();
    }//Apply attribute bonus from allocate points

    public float DamageAfterStrBonus(float damage)
    {
        float finalDamage = (damage + currentAtkPoint) * Mathf.Pow(UtilityClass.percentIncreasedPerStr, strength);
        return finalDamage;
    }

    public float DurationAfterDexBonus(float duration)
    {
        return duration * Mathf.Pow(UtilityClass.percentIncreasedPerDex, dexterity);
    }

    public float AOEAfterIntBonus(float AOE)
    {
        return AOE * Mathf.Pow(UtilityClass.percentIncreasedPerInt, intelligence);
    }

    public void TakeDamage(float dmgTaken)
    {
        currentHealth -= UtilityClass.DamageAfterDef(dmgTaken, currentDefPoint);
        if(currentHealth < 0)
            currentHealth = 0;
    }

    public void UseMana(float manaUsed)
    {
        if (IsEnoughMana(manaUsed))
            currentMana -= manaUsed;
    }

    public bool IsEnoughMana(float manaUsed)
    {
        if (currentMana >= manaUsed)
            return true;
        FindObjectOfType<AudioManager>().Play("Mana");
        return false;
    }

    private void OnDestroy()
    {
        level.LevelEvent -= ResetStats;
        for (int i = 0; i < playerInventory.attributeTypes.Length; i++)
        {
            //print($"Unregister mod event: {playerInventory.attributeTypes[i].attributeType}");
            playerInventory.attributeTypes[i].value.UnregisterModEvent(ResetStats);
        }
    }
}
[System.Serializable]
public class SavePlayerInfo
{
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int currentLevel;
    public int currentExp;
    public int nextLevelExp;
    public int pointToAllocate;
    public List<Quest> savedQuests;
    public float x;
    public float y;
    public float z;
}

public static class UtilityClass
{
    public static float baseRegen = 3f;
    public static float percentIncreasedPerStr = 1.03f;
    public static float percentIncreasedPerDex = 1.006f;
    public static float percentIncreasedPerInt = 1.02f;
    public static float DamageAfterDef(float dmg, float def)
    {
        return dmg * (1 / (1 + def * 0.01f));
    }

    public static int activeNumber = 0;
    public static bool loadSavedFiles = false;
}