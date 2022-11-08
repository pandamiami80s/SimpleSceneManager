using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2022 11 08
/// Scene Init with transition effect (UnityEvent)
///     * Separated for different effect
///     
/// Setup:
///     * Attach to gameObject
/// Usage: 
///     * Use 'onTransition' for transition effect
///     * Use 'onTransitionEnd' to run main logic after transition (Avoid collision)
/// </summary>

public class SceneInit : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float transitionDuration = 1.0f;

    [Header("Events")]
    [SerializeField] OnTransition onTransition;
    [System.Serializable] [SerializeField] class OnTransition : UnityEvent<float, float, float>
    {
        // Transition: from, to, duration
    }
    [SerializeField] UnityEvent onTransitionEnd;



    void Start()
    {
        OnSceneInit(transitionDuration);
    }

    void OnSceneInit(float duration)
    {
        StartCoroutine(OnSceneInitCoroutine(duration));
    }

    IEnumerator OnSceneInitCoroutine(float duration)
    {
        onTransition.Invoke(1.0f, 0.0f, duration);

        yield return new WaitForSeconds(duration);

        // Handle event end
        if (0 < onTransitionEnd.GetPersistentEventCount())
        {
            onTransitionEnd.Invoke();
        }
    }
}