using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 2023 03 19
/// 
/// Usage:
///     * Change scene transition effect
///     * Use event for different ChangeScene function (When used PUN)
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



    public void ChangeSceneTransition(int sceneIndex)
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(ChangeSceneTransitionCoroutine(sceneIndex));
    }

    IEnumerator ChangeSceneTransitionCoroutine(int sceneIndex)
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
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }

        yield return new WaitForEndOfFrame();

        isWorking = false;
    }
}
