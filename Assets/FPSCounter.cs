using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private int smoothingWindow = 100; // N (higher = smoother)
    [SerializeField] private int printEveryKFrames = 100; // K

    private float emaDeltaTime = 0f;
    private int frameCounter = 0;

    void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        float smoothingFactor = 2f / (smoothingWindow + 1);

        if (frameCounter == 0)
            emaDeltaTime = deltaTime;
        else
            emaDeltaTime += smoothingFactor * (deltaTime - emaDeltaTime);

        frameCounter++;

        if (frameCounter % printEveryKFrames == 0)
        {
            float fps = 1f / emaDeltaTime;
            Debug.Log($"FPS: {fps:F2}");
        }
    }
}
