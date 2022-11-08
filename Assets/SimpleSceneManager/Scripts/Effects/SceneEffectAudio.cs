using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 2022 11 08
/// Scene transition audio effect
/// 
/// Setup:
///     * Attach to gameObject with events
///     * Set audio clip
///     * Assign Audio Mixer Group
///         * Create new audio mixer (Window -> Audio -> Audio Mixer -> "+")
///         * Create new item (Groups -> Master -> "+" and name it "Effects")
/// Usage: 
///     * Use Audio Mixer Group for better auido effects handling (Enable/disable in options)
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