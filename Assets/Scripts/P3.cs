using UnityEngine;

public class P3 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Y Limits")]
    public float maxY = 4.0f;
    public float minY = -4.0f;

    private Transform _ballTransform;

    private void Update()
    {
        // Always check if the ball exists, and try to find it if not
        if (_ballTransform == null)
        {
            var ballObj = GameObject.FindGameObjectWithTag("Ball");
            if (ballObj != null)
            {
                _ballTransform = ballObj.transform;
            }
        }

        // Still null? Skip this frame.
        if (_ballTransform == null) return;

        // Move toward the ball's Y position
        var step = moveSpeed * Time.deltaTime;
        var targetY = _ballTransform.position.y;
        var newY = Mathf.MoveTowards(transform.position.y, targetY, step);

        // Apply movement
        var newPosition = new Vector3(transform.position.x, newY, transform.position.z);

        // Clamp Y position
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}