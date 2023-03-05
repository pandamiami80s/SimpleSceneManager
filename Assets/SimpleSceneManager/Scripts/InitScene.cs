using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2023 03 05
/// Scene Init with transition effect (UnityEvent)
///     * Separated for different effect
///     
/// Setup:
///     * Attach to gameObject
/// Usage: 
///     * Use 'onTransition' for transition effect
///     * Use 'onTransitionEnd' to run main logic after transition (Avoid collision)
/// </summary>

public class InitScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float initDuration = 1.0f;
    bool isWorking = false;

    [Header("Events")]
    [SerializeField] OnInit onInit;
    [System.Serializable] [SerializeField] class OnInit : UnityEvent<float, float, float> { }
    [SerializeField] UnityEvent onInitEnd;



    void Start()
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(SceneInitCoroutine());
    }

    IEnumerator SceneInitCoroutine()
    {
        isWorking = true;

        onInit.Invoke(1.0f, 0.0f, initDuration);

        yield return new WaitForSeconds(initDuration);

        // Handle event end
        if (0 < onInitEnd.GetPersistentEventCount())
        {
            onInitEnd.Invoke();
        }

        isWorking = false;
    }
}