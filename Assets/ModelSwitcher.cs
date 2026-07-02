using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModelSwitcher : MonoBehaviour
{
    [Header("Dual Animation Settings")]
    public GameObject nephronAnimModel;  // Drag Nephron Animation model here
    public AudioSource nephronAudio;     // Drag Nephron AudioSource here
    public GameObject urineAnimModel;    // Drag Kidney Urine Animation model here
    public AudioSource urineAudio;       // Drag Urine AudioSource here

    private Coroutine animationSequenceTask;
    [Header("Model Groups")]
    public GameObject skeletonMuscleGroup;
    public GameObject kidneyGroup;
    public GameObject nephronGroup;
    public GameObject animationMenuPanel;
    public GameObject animationModel; // Drag your animated model from the hierarchy here

    [Header("Disease UI")]
    public GameObject diseaseButtonsGroup;

    [Header("Audio Settings")]
    public AudioSource skeletonVoice;
    private Coroutine audioTask;

    [Header("Specific UI Buttons")]
    public GameObject nephronButton;
    private Button nephronBtnComp;

    [Header("Camera Reset & Targets")]
    public Transform xrOrigin;
    public Transform insideViewTarget;
    public Vector3 startPosition = new Vector3(0, 0, -2);
    public Vector3 startRotation = new Vector3(0, 0, 0);

    [Header("Disease Models")]
    public GameObject kidneyStonesModel;
    public GameObject pkdModel;

    private Coroutine movementTask;
    private SkinnedMeshRenderer pkdRenderer;
    private Coroutine pkdAnimationTask;
    private Coroutine stoneAnimationTask;

    [Header("Disease Audio")]
    public AudioSource pkdVoice; // Reference for the PKD audio clip
    public AudioSource stoneVoice;
    void Start()
    {
        if (nephronButton != null)
        {
            nephronBtnComp = nephronButton.GetComponent<Button>();
            if (nephronBtnComp != null) nephronBtnComp.interactable = false;
        }
        HideAllContent();
    }

  

    private void HideAllContent()
    {
        // 1. STOP ALL BACKGROUND TASKS (This kills delayed skeleton audio)
        StopAllCoroutines();
        animationSequenceTask = null;
        pkdAnimationTask = null;
        stoneAnimationTask = null;
        audioTask = null;

        // 2. GLOBAL MUTE: Force stop every audio source in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }

        // 3. HIDE ALL MODELS
        if (skeletonMuscleGroup != null) skeletonMuscleGroup.SetActive(false);
        if (kidneyGroup != null) kidneyGroup.SetActive(false);
        if (nephronGroup != null) nephronGroup.SetActive(false);
        if (diseaseButtonsGroup != null) diseaseButtonsGroup.SetActive(false);
        if (animationMenuPanel != null) animationMenuPanel.SetActive(false);
        if (kidneyStonesModel != null) kidneyStonesModel.SetActive(false);
        if (pkdModel != null) pkdModel.SetActive(false);
        if (animationModel != null) animationModel.SetActive(false);
        if (nephronAnimModel != null) nephronAnimModel.SetActive(false);
        if (urineAnimModel != null) urineAnimModel.SetActive(false);
    }
    public void ShowSkeletonView()
    {
        if (audioTask != null) StopCoroutine(audioTask);
        ResetCamera();
        HideAllContent();

        if (skeletonVoice != null) skeletonVoice.Stop();
        if (nephronBtnComp != null) nephronBtnComp.interactable = false;
        if (skeletonMuscleGroup != null) skeletonMuscleGroup.SetActive(true);

        var explorer = xrOrigin.GetComponent<CameraExplorer>();
        if (explorer != null) explorer.StartExploring();

        audioTask = StartCoroutine(PlayAudioAfterDelay(5.0f));
    }

    IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (skeletonVoice != null) skeletonVoice.Play();
    }

    public void ShowKidneyView()
    {
        ResetCamera();
        HideAllContent();
        if (skeletonVoice != null) skeletonVoice.Stop();
        if (nephronBtnComp != null) nephronBtnComp.interactable = true;

        if (kidneyGroup != null)
        {
            kidneyGroup.SetActive(true);
            ModelResetter resetter = kidneyGroup.GetComponent<ModelResetter>();
            if (resetter != null) resetter.ResetToOriginal();
        }
    }

    public void ShowNephronView()
    {
        ResetCamera();
        HideAllContent();
        if (skeletonVoice != null) skeletonVoice.Stop();

        if (nephronGroup != null)
        {
            nephronGroup.SetActive(true);
            ModelResetter nephronReset = nephronGroup.GetComponent<ModelResetter>();
            if (nephronReset != null) nephronReset.ResetToOriginal();
        }
    }

    public void ShowDiseasesUI()
    {
        ResetCamera();
        HideAllContent();
        if (skeletonVoice != null) skeletonVoice.Stop();
        if (diseaseButtonsGroup != null) diseaseButtonsGroup.SetActive(true);
        if (nephronBtnComp != null) nephronBtnComp.interactable = false;
    }

  
    public void ShowKidneyStones()
    {
        HideAllContent(); // This now ensures the Skeleton audio stops first
        ResetCamera();

        if (kidneyStonesModel != null)
        {
            kidneyStonesModel.SetActive(true);
            if (stoneVoice != null) stoneVoice.Play();

            Animator anim = kidneyStonesModel.GetComponent<Animator>();
            if (anim == null) anim = kidneyStonesModel.GetComponentInChildren<Animator>();

            if (anim != null)
            {
                // SET SPEED HERE: 0.1f is very slow (10% of normal speed)
                anim.speed = 0.1f;
                anim.Play("Scene", -1, 0f);
            }
        }
    }
    public void ShowPKD()
    {
        ResetCamera();
        HideAllContent();

        if (pkdModel != null)
        {
            pkdModel.SetActive(true);
            if (pkdVoice != null) pkdVoice.Play();

            // Try to find the renderer on the object or its children
            pkdRenderer = pkdModel.GetComponentInChildren<SkinnedMeshRenderer>();

            if (pkdRenderer != null)
            {
                // Verify the index exists in the console
                int index = pkdRenderer.sharedMesh.GetBlendShapeIndex("PKD_morph");
                Debug.Log("PKD Morph Index: " + index);

                if (pkdAnimationTask != null) StopCoroutine(pkdAnimationTask);
                pkdAnimationTask = StartCoroutine(AnimatePKD());
            }
            else
            {
                Debug.LogError("No SkinnedMeshRenderer found on " + pkdModel.name);
            }
        }
    }

    IEnumerator AnimatePKD()
    {
        int index = pkdRenderer.sharedMesh.GetBlendShapeIndex("PKD_morph");
        if (index == -1) index = 0; // Fallback to first shape key if name is wrong

        float time = 0;
        float duration = 5f;

        while (time < duration)
        {
            // Calculate 0 to 100 weight
            float weight = Mathf.Lerp(0, 100, time / duration);
            pkdRenderer.SetBlendShapeWeight(index, weight);

            time += Time.deltaTime;
            yield return null;
        }

        pkdRenderer.SetBlendShapeWeight(index, 100); // Ensure it ends fully grown
    }

    private void ResetCamera()
    {
        if (xrOrigin != null)
        {
            if (movementTask != null) StopCoroutine(movementTask);
            var explorer = xrOrigin.GetComponent<CameraExplorer>();
            if (explorer != null) explorer.StopAllCoroutines();
            xrOrigin.position = startPosition;
            xrOrigin.eulerAngles = startRotation;
        }
    }
    public void ShowAnimations()
    {
        ResetCamera();
        HideAllContent();

        // DISABLE the Nephron button here
        if (nephronBtnComp != null) nephronBtnComp.interactable = false;
        // If a sequence is already running, stop it so they don't overlap
        if (animationSequenceTask != null) StopCoroutine(animationSequenceTask);
        

        // Start the new chain reaction
        animationSequenceTask = StartCoroutine(PlayAnimationSequence());
    }
    IEnumerator PlayAnimationSequence()
    {
        // --- PART 1: NEPHRON ---
        // Remove HideAllContent() from here! It's already called in ShowAnimations().
        if (nephronAnimModel != null)
        {
            nephronAnimModel.SetActive(true);
            if (nephronAudio != null)
            {
                nephronAudio.Play();
                yield return new WaitForSeconds(nephronAudio.clip.length);
            }
            nephronAnimModel.SetActive(false); // Hide Nephron when done
        }

        // --- PART 2: URINE/KIDNEY ---
        // Do NOT call HideAllContent() here either, or it will stop the audio you just started.
        if (urineAnimModel != null)
        {
            urineAnimModel.SetActive(true);
            if (urineAudio != null)
            {
                urineAudio.Play();
                yield return new WaitForSeconds(urineAudio.clip.length);
            }
            // Keep it visible or hide it based on your preference
            // urineAnimModel.SetActive(false); 
        }

        animationSequenceTask = null;
    }
}