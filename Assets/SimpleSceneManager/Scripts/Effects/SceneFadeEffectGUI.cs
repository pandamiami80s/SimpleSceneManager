using UnityEngine;
using System.Collections;

/// <summary>
/// 2023 03 05
/// Scene transition fade effect
///     * (WARNING) This script is using deprecated "OnGUI" but still works with Unity 2019 and 2020 and 2022
///     * This script is a little bit complicated because of "OnGUI" usage. Unity can not use functions outside OnGUI
/// 
/// Setup:
///     * Attach to gameObject with events
///     * Set main camera tag to "MainCamera"
/// </summary>

public class SceneFadeEffectGUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] FadeType fadeType;
    [SerializeField] enum FadeType
    {
        simple,
        horizontal,
        vertical
    }
    // Texture
    Texture2D texture;
    [SerializeField] Color textureColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    // Have effect, also order in same script matters
    [SerializeField] int textureOrder = -1000;
    // Effect parameter used in OnGUI (Fade ammount)
    float textureTransitionValue = 0.0f;
    // Enable/Disable drawing in OnGUI to save resources
    bool isDrawing = false;
    // Main camera to apply effect to
    Camera cameraMain;                                 



    void Start()
    {
        // Get main camera
        cameraMain = Camera.main;
        if (cameraMain == null)
        {
            Debug.LogError("No MainCamera tag found!");
        }

        // SupportsTextureFormatNative is not allowed to be called from a MonoBehaviour constructor
        texture = new Texture2D(1, 1);
    }

    public void Fade(float from, float to, float duration)
    {
        // Get textureTransitionValue for fade amount
        // Can not use in ONGUI because need to do this periodically in separate thread
        StartCoroutine(FadeCoroutine(from, to, duration));
    }

    IEnumerator FadeCoroutine(float from, float to, float duration)
    {
        // Transition parameters
        float transitionValue = 0.0f;
        // Same as Time.deltaTime / duration
        float transitionRate = 1 / duration;           

        isDrawing = true;

        while (transitionValue < 1.0f)
        {
            transitionValue += Time.unscaledDeltaTime * transitionRate;
            textureTransitionValue = Mathf.Lerp(from, to, transitionValue);

            yield return 0;
        }

        // Maybe usefull in some cases
        //yield return new WaitForEndOfFrame();             

        // Stop drawing only for FadeIn case (Else in fadeOut case drawing stops and no effect is visible after transition ends)
        if (to < from)          
        {
            isDrawing = false;
        }
    }

    // OnGUI runs twice per frame
    void OnGUI()           
    {
        if (isDrawing == true)
        {
            // Set texture color dynamically
            texture.SetPixel(0, 0, textureColor);
            texture.Apply();

            // Draw
            GUI.depth = textureOrder;
            if (fadeType == FadeType.simple)
            {
                GUI.DrawTexture(new Rect(0, 0, cameraMain.pixelWidth, cameraMain.pixelHeight), texture);
            }
            else if (fadeType == FadeType.horizontal)
            {
                GUI.DrawTexture(new Rect(0, 0, cameraMain.pixelWidth / 2 * textureTransitionValue, cameraMain.pixelHeight), texture);
                GUI.DrawTexture(new Rect(cameraMain.pixelWidth / 2 + cameraMain.pixelWidth / 2 * (1 - textureTransitionValue), 0, cameraMain.pixelWidth / 2, cameraMain.pixelHeight), texture);
            }
            else if (fadeType == FadeType.vertical)
            {
                GUI.DrawTexture(new Rect(0, 0, cameraMain.pixelWidth, cameraMain.pixelHeight / 2 * textureTransitionValue), texture);
                GUI.DrawTexture(new Rect(0, cameraMain.pixelHeight / 2 + cameraMain.pixelHeight / 2 * (1 - textureTransitionValue), cameraMain.pixelWidth, cameraMain.pixelHeight), texture);
            }
            textureColor.a = textureTransitionValue;
        }
    }
}
