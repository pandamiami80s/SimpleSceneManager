using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

// OLD SRIPT FROM 2022

/// <summary>
/// 2024 01 31
/// 
/// Application Splash Scene script
///     * (WARNING) This script is using deprecated "OnGUI" but still works with Unity 2019, 2020, 2021, 2022
///     * This script is a little bit complicated because of "OnGUI" usage. Unity can not use functions outside OnGUI
/// 
/// Setup:
///     * Create separate splash scene
///     * Attach to gameObject
///     * Prepare sprite(s) at least 800x600 (And sound if needed)
///     * Set main camera tag to "MainCamera"
///     * Check use conditions
/// Usage:
///     * Call ON_SplashScreen_show to start
///     * Use 'event_sprite_transition_effect' for splash sprites transition effects
///     * Use 'event_transition_end' to change scene after splash sprites shown
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class SplashScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int sprite_offset = 32;
    // Sprites
    [SerializeField] List<class_splashScene_sprite> list_splashScene_sprites = new List<class_splashScene_sprite>();
    [System.Serializable]
    [SerializeField]
    class class_splashScene_sprite
    {
        // Image
        public Sprite sprite;
        public AudioClip audioClip;
        // Timings
        public float delay_start;
        public float fadeIn_speed;
        public float duration;
        public float fadeOut_speed;
        public float delay_end;
    }
    int sprite_current = 0;                 // Image index used in OnGUI
    bool sprite_isDrawing = false;          // Enable/Disable drawing in OnGUI to save resources
    // Audio
    AudioSource audioSource;
    // Camera
    Camera camera_main;                     // Main camera to apply effect to
    [SerializeField] Color camera_main_background_color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    [Header("Events")]
    [SerializeField] Event_sprite_transition_effect event_sprite_transition_effect;
    [System.Serializable]
    [SerializeField]
    class Event_sprite_transition_effect : UnityEvent<float, float, float>
    {
        // Transition: From, to, duration
    }
    [SerializeField] int scene_index;
    [SerializeField] Event_transition_end event_transition_end;
    [System.Serializable]
    [SerializeField]
    class Event_transition_end : UnityEvent<int>
    {
        // Scene index
    }



    void Awake()
    {
        // Get main camera
        camera_main = Camera.main;

        // Set camera background
        camera_main.backgroundColor = camera_main_background_color;

        // Get audio source
        audioSource = gameObject.GetComponent<AudioSource>();

        bool result = CheckUseConditions();
        if (result == false)
        {
            return;
        }
    }
    bool CheckUseConditions()
    {
        if (camera_main.clearFlags != CameraClearFlags.SolidColor)
        {
            Debug.LogError("Set main camera 'environment background' type to 'solid color'");

            return false;
        }
        if (list_splashScene_sprites.Count == 0)
        {
            Debug.LogError("SplashScreen sprites list is empty");

            return false;
        }

        return true;
    }

    void Start()
    {
        // Do not sleep during splash
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void ON_SplashScreen_show()
    {
        StartCoroutine(IE_ON_SplashScreen_show());
    }
    IEnumerator IE_ON_SplashScreen_show()
    {
        // Draw for each splashScreen sprite
        for (int i = 0; i < list_splashScene_sprites.Count; i++)
        {
            //Debug.Log("SplashScreen: Start drawing sprite '" + i + "'");

            // Set current sprite for ONGUI draw
            sprite_current = i;

            yield return new WaitForSeconds(list_splashScene_sprites[i].delay_start);

            // Draw srite
            sprite_isDrawing = true;

            // Play sound
            if (list_splashScene_sprites[i].audioClip != null)
            {
                audioSource.PlayOneShot(list_splashScene_sprites[i].audioClip);
            }

            // Start transition
            event_sprite_transition_effect.Invoke(1.0f, 0.0f, list_splashScene_sprites[i].fadeIn_speed);

            yield return new WaitForSeconds(list_splashScene_sprites[i].fadeIn_speed);

            yield return new WaitForSeconds(list_splashScene_sprites[i].duration);

            // Start transition
            event_sprite_transition_effect.Invoke(0.0f, 1.0f, list_splashScene_sprites[i].fadeOut_speed);

            yield return new WaitForSeconds(list_splashScene_sprites[i].fadeOut_speed);

            // Hide sprite
            sprite_isDrawing = false;

            yield return new WaitForSeconds(list_splashScene_sprites[i].delay_end);

            //Debug.Log("SplashScreen: Stop drawing sprite '" + i + "'");
        }

        event_transition_end.Invoke(scene_index);
    }

    void OnGUI()            // OnGUI runs twice per frame
    {
        // Draw sprite
        if (sprite_isDrawing == true)
        {
            GUI.DrawTexture(new Rect(sprite_offset, sprite_offset, camera_main.pixelWidth - sprite_offset * 2, camera_main.pixelHeight - sprite_offset * 2),
                list_splashScene_sprites[sprite_current].sprite.texture,
                ScaleMode.ScaleToFit);          // Scale to screen size automatically
        }
    }
}