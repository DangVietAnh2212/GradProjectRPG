using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkingButton : MonoBehaviour
{
    string testDepo;
    Image image;

    void OnEnable()
    {
        image = GetComponent<Image>();
        StartBlinking();
    }


    IEnumerator Blink()
    {
        float alpha = 1;
        while (true)
        {
            alpha -= 0.2f;
            if(alpha <= 0f)
                alpha = 1f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void StartBlinking()
    {
        StartCoroutine(nameof(Blink));
    }

    void StopBlinking()
    {
        StopCoroutine(nameof(Blink));
    }

    private void OnDisable()
    {
        StopBlinking();
    }
}