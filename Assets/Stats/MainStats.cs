using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(Level))]
public class MainStats : MonoBehaviour
{
    public GameObject[] mainAttributeChanges;
    PlayerInventoryManager playerInventory;
    public GameObject healthText;
    public GameObject manaText;
    public GameObject healthUI;
    public GameObject manaUI;
    Image healthFill;
    Image manaFill;
    public Level level;
    public int maxHealth;
    public int maxMana;
    public int currentHealth;
    public int currentMana;
    public const int baseHealth = 100;
    public const int baseMana = 100;
    const int statsPerLevel = 5;
    public const int BaseMovemenSpeed = 5;
    public int currentMoveSpeed;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int currentAtkPoint;
    public int currentDefPoint;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHealth -= 10;
            currentMana -= 10;
        }
        healthFill.fillAmount = (float)currentHealth / maxHealth;
        manaFill.fillAmount = (float)currentMana / maxMana;
        healthText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString();
        manaText.GetComponent<TextMeshProUGUI>().text = currentMana.ToString();
    }
    private void SetAttribute()
    {
        strength = baseStr + playerInventory.attributeTypes[0].value.ModifiedValue;
        dexterity = baseDex + playerInventory.attributeTypes[1].value.ModifiedValue;
        intelligence = baseInt + playerInventory.attributeTypes[2].value.ModifiedValue;
    }
    public void ResetStats()
    {
        SetAttribute();
        CalculatePlayerStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
    public void CalculatePlayerStats()
    {
        maxHealth = (int)((baseHealth + statsPerLevel * level.currentLevel) * Mathf.Pow(UtilityClass.percentIncreasedPerStr, strength))
            + playerInventory.attributeTypes[3].value.ModifiedValue;
        maxMana = (int)((baseMana + statsPerLevel * level.currentLevel) * Mathf.Pow(UtilityClass.percentIncreasedPerInt, intelligence))
            + playerInventory.attributeTypes[4].value.ModifiedValue;
        currentMoveSpeed = (int)(BaseMovemenSpeed * Mathf.Pow(UtilityClass.percentIncreasedPerDex, dexterity));
        currentAtkPoint = playerInventory.attributeTypes[5].value.ModifiedValue;
        currentDefPoint = playerInventory.attributeTypes[6].value.ModifiedValue;
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
    public static float percentIncreasedPerStr = 1.02f;
    public static float percentIncreasedPerDex = 1.01f;
    public static float percentIncreasedPerInt = 1.01f;
}