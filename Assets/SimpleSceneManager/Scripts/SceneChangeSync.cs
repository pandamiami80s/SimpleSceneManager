using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 2022 11 08
/// Scene change (sync) with transition effect (UnityEvent)
///     * Separated for different effect
/// 
/// Setup:
///     * Attach to gameObject
/// Usage: 
///     * Use 'onTransition' for transition effect
///     * Use 'onTransitionEnd' to run main logic after transition (Avoid collision)
/// </summary>

public class SceneChangeSync : MonoBehaviour
{
    [Header("Settings")]
    // If needed
    [SerializeField] float transitionDelay = 0.5f;
    [SerializeField] float transitionDuration = 1.0f;
    // Prevent click-spam (Can be used on a button)
    bool isTransitionWorking = false;          

    [Header("Event")]
    [SerializeField] OnTransition onTransition;
    [System.Serializable] [SerializeField] class OnTransition : UnityEvent<float, float, float>
    {
        // Transition: from, to, duration
    }
    [SerializeField] UnityEvent onTransitionEnd;



    public void OnChangeScene(int sceneIndex)
    {
        if (isTransitionWorking == false)
        {
            StartCoroutine(OnChangeSceneCoroutine(sceneIndex));
        }
    }

    IEnumerator OnChangeSceneCoroutine(int sceneIndex)
    {
        isTransitionWorking = true;

        yield return new WaitForSeconds(transitionDelay);

        onTransition.Invoke(0.0f, 1.0f, transitionDuration);

        yield return new WaitForSeconds(transitionDuration);

        // Handle event end
        if (0 < onTransitionEnd.GetPersistentEventCount())
        {
            onTransitionEnd.Invoke();
        }

        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene(sceneIndex);

        isTransitionWorking = false;
    }
}