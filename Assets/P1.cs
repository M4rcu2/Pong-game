using UnityEngine;

public class P1 : MonoBehaviour
{
    public float moveSpeed;
    
    private void Update()
    {
        var isPressingUp = Input.GetKey(KeyCode.W);
        var isPressingDown = Input.GetKey(KeyCode.S);

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
