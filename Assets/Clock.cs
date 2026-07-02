using UnityEngine;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text dateText; // New slot for the date

    void Update()
    {
        if (timeText != null)
        {
            timeText.text = DateTime.Now.ToString("HH:mm");
        }

        if (dateText != null)
        {
            // "dd MMM yyyy" shows as 26 Jan 2026
            // This will show: 26 JAN 2026
            dateText.text = DateTime.Now.ToString("dd MMM yyyy").ToUpper();
        }
    }
}