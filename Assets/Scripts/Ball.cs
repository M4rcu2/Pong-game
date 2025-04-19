using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingSpeed;

    public AudioClip[] bounceSounds;
    private AudioSource _audioSource;
    private bool _hasSpawnedExit = false;

    private GameManager _gm;

    private void Start()
    {
        _gm = GameManager.Instance;
        _audioSource = GetComponent<AudioSource>();

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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounceSounds.Length > 0 && _audioSource != null)
        {
            var randomClip = bounceSounds[Random.Range(0, bounceSounds.Length)];
            _audioSource.PlayOneShot(randomClip);
        }

        // Bounce off top/bottom borders
        if (collision.gameObject.CompareTag("BorderUp") || collision.gameObject.CompareTag("BorderDown"))
        {
            var directionY = collision.gameObject.CompareTag("BorderUp") ? -1f : 1f;
            var forceAmount = startingSpeed * 0.1f;
            rb.AddForce(new Vector2(0f, directionY * forceAmount), ForceMode2D.Impulse);
            return;
        }

        // Paddle collision
        if (!collision.gameObject.CompareTag("Paddle")) return;

        var paddle = collision.gameObject;
        var paddleRb = paddle.GetComponent<Rigidbody2D>();
        var paddleCollider = paddle.GetComponent<Collider2D>();

        if (paddleRb == null || paddleCollider == null) return;

        // Where did the ball hit on the paddle (0 = center, -1 = bottom, +1 = top)
        Vector3 contactPoint = collision.GetContact(0).point;
        var paddleCenterY = paddleCollider.bounds.center.y;
        var paddleHeight = paddleCollider.bounds.size.y;
        var offset = (contactPoint.y - paddleCenterY) / (paddleHeight / 2f);

        // Create angle and apply it to ball
        var bounceAngle = offset * 75f; // degrees max
        var speed = rb.velocity.magnitude;
        var directionX = Mathf.Sign(rb.velocity.x);

        // Convert angle to radians and use directionX to determine left/right
        var angleRad = bounceAngle * Mathf.Deg2Rad;
        var newDir = new Vector2(directionX * Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;

        rb.velocity = newDir * speed;

        // Add influence from paddle movement
        rb.velocity += new Vector2(0, paddleRb.velocity.y * 0.3f);
    }
    
    private void OnBecameInvisible()
    {
        if (_hasSpawnedExit) return;
        _hasSpawnedExit = true;
        HandleExit();
    }

    private void HandleExit()
    {
        if (transform.position.x < 0)
            _gm.AddPointToRight();
        else
            _gm.AddPointToLeft();

        Destroy(gameObject);
    }
}