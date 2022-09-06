using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Level : MonoBehaviour
{
    public GameObject levelUI;
    public GameObject unallocatedPointText;
    public GameObject[] unallocatedPointBtns;
    public GameObject levelUpButtonRef;
    int pointToAllocate = 0;
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
        LevelEvent += UpdateLevelText;
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
            pointToAllocate += 5;
            nextLevelExp = GetNextLevelExp();
            currentExp = 0;
        }

        if (pointToAllocate > 0)
        {
            levelUpButtonRef.SetActive(true);
            for (int i = 0; i < unallocatedPointBtns.Length; i++)
            {
                unallocatedPointBtns[i].SetActive(true);
            }
        }
        else if (pointToAllocate <= 0)
        {
            levelUpButtonRef.SetActive(false);
            for (int i = 0; i < unallocatedPointBtns.Length; i++)
            {
                unallocatedPointBtns[i].SetActive(false);
            }
        }
        UpdatePointToAllocateText();
    }

    public int GetNextLevelExp()
    {
        return (int)(baseExpBeforeLvup * Mathf.Pow(expMulEachLv, currentLevel));
    }

    public void UpdatePointToAllocate()
    {
        pointToAllocate--;
    }

    public void UpdatePointToAllocateText()
    {
        if (unallocatedPointText.GetComponent<TextMeshProUGUI>())
            unallocatedPointText.GetComponent<TextMeshProUGUI>().text =
                $"You have {pointToAllocate} unallocated points";
    }

    public void UpdateLevelText()
    {
        if (levelUI.GetComponent<TextMeshProUGUI>())
            levelUI.GetComponent<TextMeshProUGUI>().text =
                $"Your hero is at level {currentLevel}";
    }
    
    public void ResetTempUnallocatedPoint()
    {
        for (int i = 0; i < unallocatedPointBtns.Length; i++)
        {
            int unallocatedPoint = 
                Convert.ToInt32(unallocatedPointBtns[i].GetComponent<TempStatsButton>().targetFieldRef.GetComponent<TextMeshProUGUI>().text);
            pointToAllocate += unallocatedPoint;
        }
    }
}
