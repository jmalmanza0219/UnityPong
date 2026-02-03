using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour, ICollidable
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;
    private Vector2 direction = new Vector2(1f, 1f);

    public float Speed
    {
        get { return speed; }
        set { speed = Mathf.Max(0f, value); }
    }

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value.normalized; }
    }

    
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        Direction = direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
      if (collision.gameObject.CompareTag("Paddle"))
        {
            // Reverse the horizontal direction
            Direction = new Vector2(-Direction.x, Direction.y);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Reverse the vertical direction
            Direction = new Vector2(Direction.x, -Direction.y);
        }
    }
    
}
