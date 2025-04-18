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
        
        if (collision.gameObject.CompareTag("BorderUp"))
        {
            const float directionY = -1f;
            var forceAmount = startingSpeed * 0.1f;
            rb.AddForce(new Vector2(0f, directionY * forceAmount), ForceMode2D.Impulse);
        } else if (collision.gameObject.CompareTag("BorderDown"))
        {
            const float directionY = 1f;
            var forceAmount = startingSpeed * 0.1f;
            rb.AddForce(new Vector2(0f, directionY * forceAmount), ForceMode2D.Impulse);
        }
        
        if (!collision.gameObject.CompareTag("Paddle")) return;
        
        var paddleRb = collision.gameObject.GetComponent<Rigidbody2D>();
        
        if (paddleRb == null) return;
        
        var paddleYVelocity = paddleRb.velocity.y;
        const float influenceFactor = 0.9f;
        var newVelocity = rb.velocity;
        newVelocity.y += paddleYVelocity * influenceFactor;
        rb.velocity = newVelocity;
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