using UnityEngine;

/// <summary>
/// 2024 01 31
/// Change scene async loading screen effect
/// </summary>

public class SceneEffectLoadingAnimation : MonoBehaviour
{
    [SerializeField] Texture2D texture2D;
    // Have effect, also order in same script matters
    [SerializeField] int textureOrder = -1001;
    float progressValue;
    [SerializeField] Vector2 position = new Vector2(16.0f, 16.0f);
    Rect rect;
    [SerializeField] float scale = 2.0f;
    [SerializeField] int frameCount = 4;
    int currentFrame = 0;
    [SerializeField] float delayTime = 0.1f;
    float delayTimeElapsed;
    // Define single frame size for sliding window (In float by udefault)
    float frameCoefficient;

    // Test position with simple texture
    //Texture2D texture;



    void Start()
    {
        // Define position from BR (Default is from TL)
        float width = texture2D.width / frameCount * scale;
        float height = texture2D.height * scale;
        float x = Screen.width - width - position.x;
        float y = Screen.height - height - position.y;
        rect = new Rect(x, y, width, height);
        frameCoefficient = 1.0f / frameCount;

        //texture = new Texture2D(1, 1);
        //texture.SetPixel(0, 0, Color.white);
        //texture.Apply();
    }

    void Update()
    {
        // Only when progress value exists
        if (progressValue <= 0)
        {
            return;
        }

        // Delay
        if (0 < delayTimeElapsed)
        {
            delayTimeElapsed -= Time.unscaledTime;

            return;
        }
        delayTimeElapsed = delayTime;

        // Reached end
        if (frameCount - 1 <= currentFrame)
        {
            currentFrame = 0;

            return;
        }

        currentFrame++;
    }

    public void Progress(float value)
    {
        progressValue = value;
    }

    void OnGUI()
    {
        // Draw
        if (progressValue <= 0)
        {
            return;
        }

        GUI.depth = textureOrder;
        GUI.DrawTextureWithTexCoords(rect, texture2D, new Rect(frameCoefficient * currentFrame, 0.0f, frameCoefficient, 1.0f));
        //GUI.DrawTexture(rect, texture);
    }
}