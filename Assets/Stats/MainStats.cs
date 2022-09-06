using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(Level))]
public class MainStats : MonoBehaviour
{
    public GameObject statsSheetRef;
    bool hasStatsSheetInitiated = false;
    public event Action MainStatsEvent;
    public GameObject[] mainAttributeChanges;
    PlayerInventoryManager playerInventory;
    public GameObject healthText;
    public GameObject manaText;
    public GameObject healthUI;
    public GameObject manaUI;
    Image healthFill;
    Image manaFill;
    public Level level;
    public float maxHealth;
    public float maxMana;
    public float currentHealth;
    public float currentMana;
    public float healthRegen;
    public float manaRegen;
    public const float baseHealth = 100;
    public const float baseMana = 100;
    const int statsPerLevel = 12;
    public const int BaseMovemenSpeed = 5;
    public float currentMoveSpeed;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int currentAtkPoint;
    public int currentDefPoint;

    IEnumerator Regeneration()
    {
        while (true) 
        {
            currentHealth += healthRegen * Time.deltaTime;
            currentMana += manaRegen * Time.deltaTime;
            print($"current health: {healthRegen}; current mana: {manaRegen}");
            yield return null;
        }
       
    }
    public void SetBeginningAttributes()
    {
        baseStr = 6;
        baseDex = 6;
        baseInt = 12;
    }
    private void Start()
    {
        SetBeginningAttributes();
        playerInventory = GetComponent<PlayerInventoryManager>();
        level = GetComponent<Level>();
        for (int i = 0; i < playerInventory.attributeTypes.Length; i++)
        {
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
        if (currentHealth <= 0)
            currentHealth = 0;
        if (currentMana > maxMana)
            currentMana = maxMana;
        if (currentMana <= 0)
            currentMana = 0;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHealth -= 100;
            currentMana -= 100;
        }
        healthFill.fillAmount = currentHealth / maxHealth;
        manaFill.fillAmount = currentMana / maxMana;
        healthText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString("n0");
        manaText.GetComponent<TextMeshProUGUI>().text = currentMana.ToString("n0");
    }

    private void LateUpdate()
    {
        if (!hasStatsSheetInitiated)
        {
            //Load player's base stats
            statsSheetRef.SetActive(false);
            hasStatsSheetInitiated = true;  
        }
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
    /* public void UpdateStats()
     {
         strength = playerInventory.attributeTypes[0].value.ModifiedValue;
         dexterity = playerInventory.attributeTypes[1].value.ModifiedValue;
         intelligence = playerInventory.attributeTypes[2].value.ModifiedValue;
         maxHealth = (int)((baseHealth + statsPerLevel * level.currentLevel) * Mathf.Pow(UtilityClass.percentIncreasedPerStr, strength)) +
             playerInventory.attributeTypes[3].value.ModifiedValue;
         maxMana = (int)((baseMana + statsPerLevel * level.currentLevel) * Mathf.Pow(UtilityClass.percentIncreasedPerInt, intelligence)) + 
             playerInventory.attributeTypes[4].value.ModifiedValue;
         currentAtkPoint = playerInventory.attributeTypes[5].value.ModifiedValue;
         currentDefPoint = playerInventory.attributeTypes[6].value.ModifiedValue;
     }*/

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
}

public static class UtilityClass
{
    public static float baseRegen = 3f;
    public static float percentIncreasedPerStr = 1.05f;
    public static float percentIncreasedPerDex = 1.01f;
    public static float percentIncreasedPerInt = 1.03f;
}