using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingSpeed;

    public AudioClip[] bounceSounds;
    private AudioSource _audioSource;

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
        if (bounceSounds.Length <= 0 || _audioSource == null) return;
        
        var randomClip = bounceSounds[Random.Range(0, bounceSounds.Length)];
        _audioSource.PlayOneShot(randomClip);
    }

    private void OnBecameInvisible()
    {
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