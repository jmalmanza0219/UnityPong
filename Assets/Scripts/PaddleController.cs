using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
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

    protected virtual float GetInputAxis()
    {
        return 0f;
    }

   
}
