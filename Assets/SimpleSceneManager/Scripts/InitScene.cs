using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2023 03 18
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
    [SerializeField] OnInitScene onInitScene;
    [System.Serializable] [SerializeField] class OnInitScene : UnityEvent<float, float, float> { }
    [SerializeField] UnityEvent onInitSceneEnd;



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

        onInitScene.Invoke(1.0f, 0.0f, initDuration);

        yield return new WaitForSeconds(initDuration);

        // Handle event end
        if (0 < onInitSceneEnd.GetPersistentEventCount())
        {
            onInitSceneEnd.Invoke();
        }

        isWorking = false;
    }
}
