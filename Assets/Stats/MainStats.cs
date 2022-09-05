using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Level))]
public class MainStats : MonoBehaviour
{
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

    private void SetBaseAttribute()
    {
        strength = 6;
        dexterity = 6;
        intelligence = 12;
    }
    private void Start()
    {
        level = GetComponent<Level>();
        SetBaseAttribute();
        ResetStats();
        level.LevelEvent += ResetStats;
        healthFill = healthUI.GetComponent<Image>();
        manaFill = manaUI.GetComponent<Image>();
        Debug.Log($"Max Health: {maxHealth}; Max Mana: {maxMana};");
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
    public void ResetStats()
    {
        CalculatePlayerStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
    public void CalculatePlayerStats()
    {
        maxHealth = (int)((baseHealth + statsPerLevel * level.currentLevel) * Mathf.Pow(UtilityClass.percentIncreasedPerStr, strength));
        maxMana = (int)((baseMana + statsPerLevel * level.currentLevel) * Mathf.Pow(UtilityClass.percentIncreasedPerInt, intelligence));
        currentMoveSpeed = (int)(BaseMovemenSpeed * Mathf.Pow(UtilityClass.percentIncreasedPerDex, dexterity));
    }
}

public static class UtilityClass
{
    public static float percentIncreasedPerStr = 1.1f;
    public static float percentIncreasedPerDex = 1.05f;
    public static float percentIncreasedPerInt = 1.05f;
}