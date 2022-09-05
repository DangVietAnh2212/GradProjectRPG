using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Level : MonoBehaviour
{
    public GameObject expUI;
    public GameObject expText;
    Image expFill;
    public event Action LevelEvent;
    public const int MaxLevel = 50;
    public int currentLevel = 0;
    public const int baseExpBeforeLvup = 100;
    public int currentExp = 0;
    private int nextLevelExp;
    public float expMulEachLv = 1.25f;

    private void Start()
    {
        nextLevelExp = GetNextLevelExp();
        expFill = expUI.GetComponent<Image>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            currentExp += 20;
        if (expFill != null)
        {
            expFill.fillAmount = (float)currentExp / nextLevelExp;
            expText.GetComponent<TextMeshProUGUI>().text = $"{currentExp}/{nextLevelExp}";
        }
        if(currentExp >= nextLevelExp)
        {
            currentLevel++;
            LevelEvent.Invoke();
            nextLevelExp = GetNextLevelExp();
            currentExp = 0;
        }
       
    }

    public int GetNextLevelExp()
    {
        return (int)(baseExpBeforeLvup * Mathf.Pow(expMulEachLv, currentLevel));
    }
}
