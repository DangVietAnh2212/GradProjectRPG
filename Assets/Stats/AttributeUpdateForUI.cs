using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeUpdateForUI : MonoBehaviour
{
    public GameObject playerObject;
    public string attributeName;

    private void OnEnable()
    {
        if(playerObject.GetComponent<MainStats>() == null)
        {
            Debug.Log("Cannot find player!!!");
        }
    }

    private void Start()
    {
        switch (attributeName)
        {
            case "str":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIStr;
                break;
            case "dex":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIDex;
                break;
            case "int":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIInt;
                break;
            case "hp":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIHp;
                break;
            case "mana":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIMana;
                break;
            case "ms":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIMs;
                break;
            case "atk":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIAtk;
                break;
            case "def":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIDef;
                break;
            case "hpReg":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIHpReg;
                break;
            case "manaReg":
                playerObject.GetComponent<MainStats>().MainStatsEvent += UpdateUIManaReg;
                break;
        }        
    }

    public void UpdateUIManaReg()
    {
        GetComponent<TextMeshProUGUI>().text = $"Mana Regeneration per second: {playerObject.GetComponent<MainStats>().manaRegen:n1}";
    }
    public void UpdateUIHpReg()
    {
        GetComponent<TextMeshProUGUI>().text = $"Health Regeneration per second: {playerObject.GetComponent<MainStats>().healthRegen:n1}";
    }

    public void UpdateUIStr()
    {
        GetComponent<TextMeshProUGUI>().text = $"Strength: {playerObject.GetComponent<MainStats>().strength}";
    }

    public void UpdateUIDex()
    {
        GetComponent<TextMeshProUGUI>().text = $"Dexterity: {playerObject.GetComponent<MainStats>().dexterity}";
    }

    public void UpdateUIInt()
    {
        GetComponent<TextMeshProUGUI>().text = $"Intellect: {playerObject.GetComponent<MainStats>().intelligence}";
    }

    public void UpdateUIHp()
    {
        GetComponent<TextMeshProUGUI>().text = $"Maximum Health: {playerObject.GetComponent<MainStats>().maxHealth}";
    }

    public void UpdateUIMana()
    {
        GetComponent<TextMeshProUGUI>().text = $"Maximum Mana: {playerObject.GetComponent<MainStats>().maxMana}";
    }

    public void UpdateUIMs()
    {
        GetComponent<TextMeshProUGUI>().text = $"Movement Speed: {playerObject.GetComponent<MainStats>().currentMoveSpeed}";
    }

    public void UpdateUIAtk()
    {
        GetComponent<TextMeshProUGUI>().text = $"Attack Point: {playerObject.GetComponent<MainStats>().currentAtkPoint}";
    }

    public void UpdateUIDef()
    {
        GetComponent<TextMeshProUGUI>().text = $"Defense Point: {playerObject.GetComponent<MainStats>().currentDefPoint}";
    }
}
