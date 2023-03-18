using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// 2023 03 18
/// 
/// Usage:
///     * Scene transition effect
///     * Run main logic after transition effect (Avoid collisions with main thread)
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
