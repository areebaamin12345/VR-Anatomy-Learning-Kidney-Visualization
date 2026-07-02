using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SimpleSplash : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider loadingBar;
    public float loadTime = 4.0f;

    [Header("1. Hide at Start (Everything)")]
    public List<GameObject> allObjectsToHide = new List<GameObject>();

    [Header("2. Show after Loading (Only Basics)")]
    // Only put the Floor, Lights, and NavBar_Back here!
    public List<GameObject> objectsToReactivate = new List<GameObject>();

    void Awake()
    {
        // Hide every object in the first list
        foreach (GameObject obj in allObjectsToHide)
        {
            if (obj != null) obj.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    void Start()
    {
        StartCoroutine(RunLoadingSequence());
    }

    IEnumerator RunLoadingSequence()
    {
        float timer = 0;
        while (timer < loadTime)
        {
            timer += Time.deltaTime;
            if (loadingBar != null) loadingBar.value = timer / loadTime;
            yield return null;
        }

        // ONLY reactivate the basics (Floor, Lights, NavBar)
        // This keeps the Kidney/Skeleton hidden as you intended!
        foreach (GameObject obj in objectsToReactivate)
        {
            if (obj != null) obj.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}