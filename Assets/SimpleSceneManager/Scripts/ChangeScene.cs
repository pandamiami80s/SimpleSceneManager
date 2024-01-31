using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 2024 01 31
///     * Change scene Sync and Async with effect (Also with loading progress)
/// </summary>

public class ChangeScene : MonoBehaviour
{
    [Header("Settings")]
    [Range(0.0f, 2.0f)]
    [SerializeField] float delay = 0.1f;
    [Range(0.1f, 2.0f)]
    [SerializeField] float duration = 0.5f;
    // Fake
    [Range(0.1f, 4.0f)]
    [SerializeField] float loadingTime = 1.0f;
    float loadingTimeElapsed;
    bool isWorking = false;

    [Header("Event")]
    [SerializeField] OnTranstion onTranstion;
    [System.Serializable] [SerializeField] class OnTranstion : UnityEvent<float, float, float> { }

    [SerializeField] OnProgress onProgress;
    [System.Serializable] [SerializeField] class OnProgress : UnityEvent<float> { }
    //[SerializeField] UnityEvent onChangeSceneEnd; 



    public void ChangeSceneSync(int sceneIndex)
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(ChangeSceneSyncCoroutine(sceneIndex));
    }

    IEnumerator ChangeSceneSyncCoroutine(int sceneIndex)
    {
        isWorking = true;

        yield return new WaitForSeconds(delay);

        onTranstion.Invoke(0.0f, 1.0f, duration);

        yield return new WaitForSeconds(duration);

        //onChangeSceneEnd.Invoke();

        SceneManager.LoadScene(sceneIndex);

        yield return new WaitForEndOfFrame();

        isWorking = false;
    }

    public void ChangeSceneAsync(int sceneIndex)
    {
        if (isWorking)
        {
            return;
        }

        StartCoroutine(ChangeSceneAsyncCoroutine(sceneIndex));
    }

    IEnumerator ChangeSceneAsyncCoroutine(int sceneIndex)
    {
        isWorking = true;

        yield return new WaitForSeconds(delay);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        bool isLoading = true;

        onTranstion.Invoke(0.0f, 1.0f, duration);

        yield return new WaitForSeconds(duration);

        //onChangeSceneEnd.Invoke();

        while (isLoading)
        {
            loadingTimeElapsed += Time.unscaledDeltaTime;
            float progress = asyncOperation.progress;
            onProgress.Invoke(progress);

            // Both loaded and fake time passed (Scene loaded at 0.9f always by default)
            if (0.9f <= progress && loadingTime <= loadingTimeElapsed)
            {
                Debug.Log(loadingTime + " " + loadingTimeElapsed);

                asyncOperation.allowSceneActivation = true;

                isLoading = false;
            }

            yield return null;
        }

        //yield return new WaitForEndOfFrame();

        isWorking = false;
    }
}