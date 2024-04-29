using UnityEngine;

public class AspectRatioAdjuster : MonoBehaviour
{
    public Camera mainCamera;
    private float targetAspectRatio = 16f / 9f;  // Adjust as needed

    void Update()
    {
        float currentAspectRatio = Screen.width / Screen.height;
        float scaleFactor = currentAspectRatio / targetAspectRatio;

        // Adjust position based on your needs (example: center with some offset)
        float offset = 0.2f; // Adjust offset as needed
        transform.position = new Vector3(mainCamera.orthographicSize * scaleFactor * 0.5f + offset, transform.position.y, transform.position.z);
    }
}
