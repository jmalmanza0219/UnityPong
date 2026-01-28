using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
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
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
            Direction = new Vector2(-direction.x, direction.y);

        if (collision.gameObject.CompareTag("Wall"))
            Direction = new Vector2(direction.x, -direction.y);
    }
}