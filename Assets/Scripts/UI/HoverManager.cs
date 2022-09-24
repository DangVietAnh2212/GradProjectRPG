using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class HoverManager : MonoBehaviour
{
    public static Action OnMouseLoseFocus;
    public static Action<string, Vector2> OnMouseHover;
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);
        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x , mousePos.y + tipWindow.sizeDelta.y);
        //new Vector2(mousePos.x + tipWindow.sizeDelta.x * 2, mousePos.y);
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
