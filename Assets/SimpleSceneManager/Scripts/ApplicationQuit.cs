using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2022 11 08
/// Application quit with transition effect (UnityEvent)
///     * Separated for different effect
/// 
/// Setup:
///     * Attach to gameObject 
/// Usage:
///     * Use 'onTransition' for transition effect
/// </summary>

public class ApplicationQuit : MonoBehaviour
{
    [Header("Settings")]
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



    // OnApplicationQuit exists in unity
    public void OnAppQuit()
    {
        if (isTransitionWorking == false)
        {
            StartCoroutine(OnAppQuitCoroutine());
        }
    }

    IEnumerator OnAppQuitCoroutine()
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

        Application.Quit();
        // Android: Back will exit app
        //Input.backButtonLeavesApp = true;         
        // Destroy process
        //System.Diagnostics.Process.GetCurrentProcess().Kill();            

        isTransitionWorking = false;
    }
}