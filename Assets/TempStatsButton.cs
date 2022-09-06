using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class TempStatsButton : MonoBehaviour
{
    public GameObject targetFieldRef;
    public void ModifyField()
    {
        if (targetFieldRef.GetComponent<TextMeshProUGUI>())
        {
            int count = Convert.ToInt32(targetFieldRef.GetComponent<TextMeshProUGUI>().text);
            count++;
            targetFieldRef.GetComponent<TextMeshProUGUI>().text = count.ToString();
        }
    }
    public void ResetField()
    {
        targetFieldRef.GetComponent<TextMeshProUGUI>().text = "0";
    }
}
