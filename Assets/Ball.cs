using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingSpeed;
    private readonly GameManager _gm = GameManager.Instance;
    
    private void Start()
    {
        var isRight = Random.value >= 0.5;
        var xVelocity = isRight ? 1.0f : -1.0f;
        var yVelocity = Random.Range(-1.0f, 1.0f);

        xVelocity = _gm.scorer switch
        {
            "left" => 1.0f,
            "right" => -1.0f,
            _ => xVelocity
        };

        rb.freezeRotation = true;
        rb.velocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    private void OnBecameInvisible()
    {
        HandleExit();
    }

    private void HandleExit()
    {
        if (transform.position.x < 0)
            GameManager.Instance.AddPointToRight();
        else
            GameManager.Instance.AddPointToLeft();
        Destroy(gameObject);
    }
}
