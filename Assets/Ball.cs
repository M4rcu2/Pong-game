using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingSpeed;
    
    private void Start()
    {
        var isRight = Random.value >= 0.5;
        var xVelocity = -1.0f;
        var yVelocity = Random.Range(-1.0f, 1.0f);

        if (isRight)
        {
            xVelocity = 1.0f;
        }

        rb.freezeRotation = true;
        rb.velocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }
}
