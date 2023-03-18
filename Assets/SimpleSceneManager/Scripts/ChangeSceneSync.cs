using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 2023 03 18
/// 
/// Usage:
///     * Change scene transition effect
///     * Change scene after transition effect (Different scene load function when used with PUN)
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



    public void ChangeSceneTransition()
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(ChangeSceneTransitionCoroutine());
    }

    IEnumerator ChangeSceneTransitionCoroutine()
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

        isWorking = false;
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
