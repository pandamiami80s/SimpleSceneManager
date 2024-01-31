using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 2024 01 31
/// Scene transition audio effect
///     * Use Audio Mixer Group for better audio effects handling (Enable/disable in options)
///         * Create new audio mixer (Window -> Audio -> Audio Mixer -> "+")
///         * Create new item (Groups -> Master -> "+" and name it "Effects")
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class SceneEffectAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    [Header("Audio Mixer")]
    // Fast assign here (Instead of audioSource)  
    [SerializeField] AudioMixerGroup audioMixerGroupOutput;              



    void Start()
    {
        if (IsComponentsMissing())
        {
            return;
        }
        
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerGroupOutput;
    }

    bool IsComponentsMissing()
    {
        if (audioClip == null)
        {
            Debug.LogError("AudioClip not set");

            return false;
        }

        if (audioMixerGroupOutput == null)
        {
            Debug.LogError("Audio Mixer Group not set");

            return false;
        }

        return true;
    }

    // Empty event parameters but can be used in the future
    public void Transition(float from, float to, float duration)         
    {
        audioSource.PlayOneShot(audioClip);
    }
}