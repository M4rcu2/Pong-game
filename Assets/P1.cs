using System.Collections;
using UnityEngine;

public class P1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashDistance = 4f;
    public float dashCooldown = 0.5f;
    public float dashDuration = 0.10f;

    [Header("Y Limits")]
    public float maxY = 4.0f;
    public float minY = -4.0f;

    private float _lastDashTime;
    private bool _isDashing;
    public AudioClip dashSound;
    public AudioSource audioSource;

    private void Update()
    {
        var isPressingUp = Input.GetKey(KeyCode.W);
        var isPressingDown = Input.GetKey(KeyCode.S);
        var justPressedShift = Input.GetKeyDown(KeyCode.LeftShift);
        var currentTime = Time.time;

        if (!_isDashing)
        {
            if (isPressingUp)
                transform.Translate(Vector2.up * (moveSpeed * Time.deltaTime));
            if (isPressingDown)
                transform.Translate(Vector2.down * (moveSpeed * Time.deltaTime));
        }

        if (justPressedShift && currentTime - _lastDashTime > dashCooldown)
        {
            if (isPressingUp)
            {
                Dash(Vector2.up);
                _lastDashTime = currentTime;
            }
            else if (isPressingDown)
            {
                Dash(Vector2.down);
                _lastDashTime = currentTime;
            }
        }

        var clampedPos = transform.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        transform.position = clampedPos;
    }

    private void Dash(Vector2 direction)
    {
        if (_isDashing) return;
        audioSource.PlayOneShot(dashSound);
        StartCoroutine(DashCoroutine(direction));
    }

    private IEnumerator DashCoroutine(Vector2 direction)
    {
        _isDashing = true;

        var elapsedTime = 0f;
        var startY = transform.position.y;
        var endY = startY + direction.y * dashDistance;

        while (elapsedTime < dashDuration)
        {
            var newY = Mathf.Lerp(startY, endY, elapsedTime / dashDuration);
            var p = transform.position;
            transform.position = new Vector3(p.x, newY, p.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        var finalPos = transform.position;
        transform.position = new Vector3(finalPos.x, endY, finalPos.z);
        _isDashing = false;
    }
}
