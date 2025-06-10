using UnityEngine;

public class ProjectileAutoDestroy : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Please ensure your camera is tagged as 'MainCamera'.");
            enabled = false; // Disable this script if no main camera is found.
            return;
        }

        // Get the screen boundaries in world coordinates
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Attempt to get the size of the object for more accurate boundary checks
        // For 2D, a SpriteRenderer or Collider2D might give us dimensions
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            objectWidth = sr.bounds.size.x / 2;
            objectHeight = sr.bounds.size.y / 2;
        }
        else
        {
            // Fallback if no SpriteRenderer, assume a small default size
            objectWidth = 0.1f;
            objectHeight = 0.1f;
        }
    }

    void Update()
    {
        // Check if the projectile is outside the camera's view
        // We add objectWidth/Height to screenBounds to destroy it once it's completely out of view.
        bool isOutsideX = transform.position.x < -screenBounds.x - objectWidth || transform.position.x > screenBounds.x + objectWidth;
        bool isOutsideY = transform.position.y < -screenBounds.y - objectHeight || transform.position.y > screenBounds.y + objectHeight;

        if (isOutsideX || isOutsideY)
        {
            Destroy(gameObject);
            // Debug.Log($"Projectile {gameObject.name} destroyed: Out of camera view.");
        }
    }
}