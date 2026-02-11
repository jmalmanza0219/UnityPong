using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour, ICollidable
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;
    private Vector2 direction = new Vector2(1f, 1f);

    public float Speed
    {
        get { return speed; }
        set { speed = Mathf.Max(1f, value); }
    }

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value.normalized; }
    }

    
    void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
       
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
           LaunchBall();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsServer)
        {
        rb.velocity = Direction * Speed;
        }
    }

    private void LaunchBall()
    {
      direction = new Vector2(Random.value < 0.5f ? -1f : 1f, Random.Range(-0.5f, 0.5f)).normalized;
       rb.velocity = Direction * Speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnHit(collision);
        }

        OnHit(collision);
    }


    public void OnHit(Collision2D collision)
    {
        if (!IsServer) return;
      if (collision.gameObject.CompareTag("Paddle"))
        {
            // Reverse the horizontal direction
            Direction = new Vector2(-Direction.x, Direction.y);
            Speed += 2f; 
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Reverse the vertical direction
            Direction = new Vector2(Direction.x, -Direction.y);
        }
    }
    
}
