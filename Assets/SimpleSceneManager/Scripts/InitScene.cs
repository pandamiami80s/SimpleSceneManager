using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2023 03 18
/// 
/// Usage:
///     * Init scene transition effect
///     * Run main game logic after transition effect (Avoid collisions with main thread)
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

        StartCoroutine(InitSceneCoroutine());
    }

    IEnumerator InitSceneCoroutine()
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
