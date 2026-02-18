using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour, ICollidable
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;
    private Vector2 direction = new Vector2(1f, 0f);

    public float Speed
    {
        get => speed;
        set => speed = Mathf.Max(0f, value);
    }

    public Vector2 Direction
    {
        get => direction;
        set => direction = value.normalized;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        // Do NOT launch here. GameManager controls serving.
    }

    private void FixedUpdate()
{
    if (!IsServer) return;

    if (GameManager.Instance != null &&
        (!GameManager.Instance.gameStarted.Value || GameManager.Instance.gameOver.Value))
    {
        rb.velocity = Vector2.zero;
        return;
    }

    rb.velocity = Direction * Speed;
}

    // Called by GameManager (server only)
    public void Serve(Vector2 serveDirection, float serveSpeed)
    {
        if (!IsServer) return;

        rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;

        Speed = serveSpeed;
        Direction = serveDirection.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null)
            collidable.OnHit(collision);

        OnHit(collision);
    }

    public void OnHit(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Paddle"))
        {
            Direction = new Vector2(-Direction.x, Direction.y);
            Speed += 2f;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Direction = new Vector2(Direction.x, -Direction.y);
        }
    }

    public void StopBall()
    {
        if (!IsServer) return;

        Speed = 0f;
        Direction = Vector2.zero;
        rb.velocity = Vector2.zero;
    }
}
