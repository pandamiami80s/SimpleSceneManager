using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 2023 03 05
///     * To play animations during pause use Animator -> Update Mode -> Unscaled Time
/// </summary>

public class PauseScene : MonoBehaviour
{
    bool isPaused = false;

    [Header("Event")]
    [SerializeField] UnityEvent onPause;
    [SerializeField] UnityEvent onContinue;



    public void Pause()
    {
        if (isPaused)
        {
            return;
        }

        onPause.Invoke();
        Time.timeScale = 0;
        isPaused = true;
    }
   
    public void Continue()
    {
        if (!isPaused)
        {
            return;
        }

        onContinue.Invoke();
        Time.timeScale = 1;
        isPaused = false;
    }
}
