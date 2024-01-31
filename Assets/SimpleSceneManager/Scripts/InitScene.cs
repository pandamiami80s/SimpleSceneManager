using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 2024 01 31
/// Init scene transition effect
///     * Run main game logic after transition effect (Avoid collisions with main thread)
/// </summary>

public class InitScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float initDuration = 1.0f;
    bool isWorking = false;

    [Header("Events")]
    [SerializeField] OnTransition onTransition;
    [System.Serializable] [SerializeField] class OnTransition : UnityEvent<float, float, float> { }
    [SerializeField] UnityEvent onInitSceneEnd;



    void OnEnable()
    {
        if (isWorking)
        {
            return;
        }

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene scene, Scene loadSceneMode)
    {
        StartCoroutine(InitSceneCoroutine());
    }

    IEnumerator InitSceneCoroutine()
    {
        isWorking = true;

        onTransition.Invoke(1.0f, 0.0f, initDuration);

        yield return new WaitForSeconds(initDuration);

        // Handle event end
        onInitSceneEnd.Invoke();

        isWorking = false;
    }
}
