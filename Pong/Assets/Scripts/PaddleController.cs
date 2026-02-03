using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PaddleController : MonoBehaviour, ICollidable
{
    protected Rigidbody2D rb;

    [SerializeField] protected float movespeed = 8f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        float input = GetInputAxis();
        rb.velocity = new Vector2(0f, input * movespeed);
    }

   protected abstract float GetInputAxis();

   public virtual void OnHit(Collision2D collision)
    {
       if(collision.gameObject.CompareTag("Ball"))
        {
         Debug.Log($"{gameObject.name} was hit by the ball!");
         
        }
    }
}
