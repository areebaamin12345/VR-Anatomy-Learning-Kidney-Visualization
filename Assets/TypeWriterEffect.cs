using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float speed = 0.05f; // Seconds per letter
    private string fullText;

    void Awake()
    {
        fullText = textComponent.text;
        textComponent.text = ""; // Start empty
    }

    void OnEnable() // Starts typing every time the Splash Screen turns on
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(speed);
        }
    }
}