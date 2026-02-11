using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class PaddleController : NetworkBehaviour, ICollidable
{
    protected Rigidbody2D rb;

    [SerializeField] protected float movespeed = 8f;

    public NetworkVariable<float> NetworkYPosition = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!IsSpawned) return;
        if (IsOwner)
        {
            float input = GetInputAxis();
            rb.velocity = new Vector2(0f, input * movespeed);

            NetworkYPosition.Value = transform.position.y;
        }
        else
        {
            Vector3 position = transform.position;
            position.y = NetworkYPosition.Value;
            transform.position = position;
        }
       
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
