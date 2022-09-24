using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Level : MonoBehaviour
{
    public GameObject levelUpPref;
    public GameObject levelUI;
    public GameObject unallocatedPointText;
    public GameObject[] unallocatedPointBtns;
    public GameObject levelUpButtonRef;
    [HideInInspector]
    public int pointToAllocate = 0;
    public GameObject expUI;
    public GameObject expText;
    Image expFill;
    public event Action LevelEvent;
    public const int MaxLevel = 50;
    public int currentLevel = 0;
    public const int baseExpBeforeLvup = 100;
    public int currentExp = 0;
    [HideInInspector]
    public int nextLevelExp;
    public float expMulEachLv = 1.25f;

    private void Start()
    {
        nextLevelExp = GetNextLevelExp();
        expFill = expUI.GetComponent<Image>();
        GetComponent<MainStats>().MainStatsEvent += UpdateLevelText;
        LevelEvent += UpdateLevelText;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            currentExp += 20;

        UpdateLevel();

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
    }

    private void LateUpdate()
    {
        if (expFill != null)
        {
            expFill.fillAmount = (float)currentExp / nextLevelExp;
            expText.GetComponent<TextMeshProUGUI>().text = $"{currentExp}/{nextLevelExp}";
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

    public void UpdateLevel()
    {
        if (currentExp >= nextLevelExp)
        {
            Instantiate(levelUpPref, transform.position + Vector3.up * 0.5f, Quaternion.identity).transform.parent = transform;
            FindObjectOfType<AudioManager>().Play("LevelUp");
            currentLevel++;
            LevelEvent.Invoke();
            pointToAllocate += 5;
            nextLevelExp = GetNextLevelExp();
            currentExp = 0;
        }      
    }
}
