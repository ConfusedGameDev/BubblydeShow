using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display_ReplayText : MonoBehaviour
{
    public TMP_Text text;
    private bool isVisible = true;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            isVisible = !isVisible;
            timer = 0f;
        }

        text.gameObject.SetActive(isVisible);
    }
}
