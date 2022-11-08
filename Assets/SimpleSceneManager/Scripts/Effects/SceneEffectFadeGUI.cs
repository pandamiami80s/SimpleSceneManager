using UnityEngine;
using System.Collections;

/// <summary>
/// 2022 11 08
/// Scene transition fade effect
///     * (WARNING) This script is using deprecated "OnGUI" but still works with Unity 2019 and 2020 and 2022
///     * This script is a little bit complicated because of "OnGUI" usage. Unity can not use functions outside OnGUI
/// 
/// Setup:
///     * Attach to gameObject with events
///     * Set main camera tag to "MainCamera"
/// </summary>

public class SceneEffectFadeGUI : MonoBehaviour
{
    [Header("Settings")]
    // Transition
    [SerializeField] TransitionType transitionType;
    [SerializeField] enum TransitionType
    {
        fade,
        fadeHorizontal,
        fadeVertical
    }
    // Texture
    Texture2D texture;
    [SerializeField] Color textureColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    // Have effect, also order in same script matters
    [SerializeField] int textureOrder = -1000;
    // Effect parameter used in OnGUI (Fade ammount)
    float textureTransitionValue = 0.0f;
    // Enable/Disable drawing in OnGUI to save resources
    bool isTextureDrawing = false;
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

    public void Transition(float from, float to, float duration)
    {
        // Get textureTransitionValue for fade amount
        // Can not use in ONGUI because need to do this periodically in separate thread
        StartCoroutine(TransitionCoroutine(from, to, duration));
    }

    IEnumerator TransitionCoroutine(float from, float to, float duration)
    {
        // Transition parameters
        float transitionValue = 0.0f;
        // Same as Time.deltaTime / duration
        float transitionRate = 1 / duration;           

        isTextureDrawing = true;

        while (transitionValue < 1.0f)
        {
            transitionValue += Time.deltaTime * transitionRate;
            textureTransitionValue = Mathf.Lerp(from, to, transitionValue);

            yield return 0;
        }

        // Maybe usefull in some cases
        //yield return new WaitForEndOfFrame();             

        // Stop drawing only for FadeIn case (Else in fadeOut case drawing stops and no effect is visible after transition ends)
        if (to < from)          
        {
            isTextureDrawing = false;
        }
    }

    // OnGUI runs twice per frame
    void OnGUI()           
    {
        if (isTextureDrawing == true)
        {
            // Set texture color dynamically
            texture.SetPixel(0, 0, textureColor);
            texture.Apply();

            // Draw
            GUI.depth = textureOrder;
            if (transitionType == TransitionType.fade)
            {
                GUI.DrawTexture(new Rect(0, 0, cameraMain.pixelWidth, cameraMain.pixelHeight), texture);
            }
            else if (transitionType == TransitionType.fadeHorizontal)
            {
                GUI.DrawTexture(new Rect(0, 0, cameraMain.pixelWidth / 2 * textureTransitionValue, cameraMain.pixelHeight), texture);
                GUI.DrawTexture(new Rect(cameraMain.pixelWidth / 2 + cameraMain.pixelWidth / 2 * (1 - textureTransitionValue), 0, cameraMain.pixelWidth / 2, cameraMain.pixelHeight), texture);
            }
            else if (transitionType == TransitionType.fadeVertical)
            {
                GUI.DrawTexture(new Rect(0, 0, cameraMain.pixelWidth, cameraMain.pixelHeight / 2 * textureTransitionValue), texture);
                GUI.DrawTexture(new Rect(0, cameraMain.pixelHeight / 2 + cameraMain.pixelHeight / 2 * (1 - textureTransitionValue), cameraMain.pixelWidth, cameraMain.pixelHeight), texture);
            }
            textureColor.a = textureTransitionValue;
        }
    }
}