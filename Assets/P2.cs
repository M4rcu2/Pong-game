using UnityEngine;

public class P2 : MonoBehaviour
{
    public float moveSpeed;
    private void Update()
    {
        var isPressingUp = Input.GetKey(KeyCode.UpArrow);
        var isPressingDown = Input.GetKey(KeyCode.DownArrow);

        if (isPressingUp)
        {
            transform.Translate(Vector2.up * (moveSpeed * Time.deltaTime));
        }
        
        if (isPressingDown)
        {
            transform.Translate(Vector2.down * (moveSpeed * Time.deltaTime));
        }
    }
}
