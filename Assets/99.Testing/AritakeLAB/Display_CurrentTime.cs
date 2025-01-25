using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display_CurrentTime : MonoBehaviour
{
    public TMP_Text timeText;
    private bool isColonVisible = true;
    private float timer = 0f;

    void Update()
    {
        DateTime now = DateTime.Now;

        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            isColonVisible = !isColonVisible;
            timer = 0f;
        }

        string timeFormat = isColonVisible ? "HH:mm" : "HH mm";
        timeText.text = now.ToString(timeFormat);
    }
}
