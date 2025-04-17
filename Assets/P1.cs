using UnityEngine;

public class P1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Y Limits")]
    public float maxY = 4.0f;
    public float minY = -4.0f;

    private void Update()
    {
        var isPressingUp = Input.GetKey(KeyCode.W);
        var isPressingDown = Input.GetKey(KeyCode.S);
        
        if (isPressingUp)
            transform.Translate(Vector2.up   * (moveSpeed * Time.deltaTime));
        if (isPressingDown)
            transform.Translate(Vector2.down * (moveSpeed * Time.deltaTime));
        
        var pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}