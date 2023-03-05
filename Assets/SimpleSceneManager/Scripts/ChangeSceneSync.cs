using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 2023 03 05
/// Scene change (sync) with transition effect (UnityEvent)
///     * Separated for different effect
/// 
/// Setup:
///     * Attach to gameObject
/// Usage: 
///     * Use 'onTransition' for transition effect
///     * Use 'onTransitionEnd' to run main logic after transition (Avoid collision)
/// </summary>

public class ChangeSceneSync : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float delay = 0.5f;
    [SerializeField] float duration = 1.0f;
    bool isWorking = false;          

    [Header("Event")]
    [SerializeField] OnChangeScene onChangeScene;
    [System.Serializable] [SerializeField] class OnChangeScene : UnityEvent<float, float, float> { }
    [SerializeField] UnityEvent onChangeSceneEnd;



    public void ChangeScene(int sceneIndex)
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(ChangeSceneCoroutine(sceneIndex));
    }

    IEnumerator ChangeSceneCoroutine(int sceneIndex)
    {
        isWorking = true;

        yield return new WaitForSeconds(delay);

        onChangeScene.Invoke(0.0f, 1.0f, duration);

        yield return new WaitForSeconds(duration);

        // Handle event end
        if (0 < onChangeSceneEnd.GetPersistentEventCount())
        {
            onChangeSceneEnd.Invoke();
        }

        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene(sceneIndex);

        isWorking = false;
    }
}