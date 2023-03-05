using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2023 03 05
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
    [SerializeField] float delay = 0.5f;
    [SerializeField] float duration = 1.0f;
    // Prevent click-spam (Can be used on a button)
    bool isWorking = false;           

    [Header("Event")]
    [SerializeField] OnApplicationQuit onApplicationQuit;
    [System.Serializable] [SerializeField] class OnApplicationQuit : UnityEvent<float, float, float>
    {
        // Transition: from, to, duration
    }
    [SerializeField] UnityEvent onApplicationQuitEnd;



    // OnApplicationQuit exists in unity
    public void AppQuit()
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(AppQuitCoroutine());
    }

    IEnumerator AppQuitCoroutine()
    {
        isWorking = true;

        yield return new WaitForSeconds(delay);

        onApplicationQuit.Invoke(0.0f, 1.0f, duration);

        yield return new WaitForSeconds(duration);

        // Handle event end
        if (0 < onApplicationQuitEnd.GetPersistentEventCount())
        {
            onApplicationQuitEnd.Invoke();
        }

        yield return new WaitForEndOfFrame();

        Application.Quit();
        // Android: Back will exit app
        //Input.backButtonLeavesApp = true;         
        // Destroy process
        //System.Diagnostics.Process.GetCurrentProcess().Kill();            

        isWorking = false;
    }
}