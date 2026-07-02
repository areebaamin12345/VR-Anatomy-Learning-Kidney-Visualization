using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    public Slider loadingBar;
    public GameObject navBarToEnable; // Drag your NavBar_Back here
    public float splashDuration = 5.0f; // How long to show splash

    void Start()
    {
        // Start hidden, then begin the sequence
        navBarToEnable.SetActive(false);
        StartCoroutine(RunSplashScreen());
    }

    IEnumerator RunSplashScreen()
    {
        float timer = 0;
        while (timer < splashDuration)
        {
            timer += Time.deltaTime;
            if (loadingBar != null)
            {
                // Update the bar from 0 to 1
                loadingBar.value = timer / splashDuration;
            }
            yield return null;
        }

        // Sequence Finished
        navBarToEnable.SetActive(true);
        gameObject.SetActive(false); // Hide the splash screen
        Debug.Log("<color=green>[SYSTEM]</color> Splash finished, Main Menu loaded.");
    }
}