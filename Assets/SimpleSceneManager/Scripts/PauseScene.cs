using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 2023 03 05 
/// </summary>

public class PauseScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float fadeAmount = 0.5f;
    bool isWorking = false;
    bool isPaused = false;

    [Header("Event")]
    [SerializeField] OnPause onPause;
    [System.Serializable] [SerializeField] class OnPause : UnityEvent<float, float, float> { }



    public void Pause()
    {
        if (isPaused)
        {
            return;
        }
        
        if (isWorking)
        {
            return;
        }

        StartCoroutine(PauseCoroutine());
    }
    IEnumerator PauseCoroutine()
    {
        isWorking = true;

        Time.timeScale = 0;
        isPaused = true;
        onPause.Invoke(0.0f, fadeAmount, fadeDuration);

        yield return new WaitForSecondsRealtime(fadeDuration);

        isWorking = false;
    }

    public void Continue()
    {
        if (!isPaused)
        {
            return;
        }

        if (isWorking)
        {
            return;
        }

        StartCoroutine(ContinueCoroutine());
    }
    IEnumerator ContinueCoroutine()
    {
        isWorking = true;

        Time.timeScale = 1;
        isPaused = false;
        onPause.Invoke(fadeAmount, 0.0f, fadeDuration);
        
        yield return new WaitForSecondsRealtime(fadeDuration);

        isWorking = false;
    }
}