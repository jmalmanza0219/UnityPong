using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D), typeof(NetworkObject))]
public class RightPaddleController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 8f;

    private Rigidbody2D rb;

    // Networked Y position for client syncing
    public NetworkVariable<float> NetworkYPosition = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private Vector3 startPos = new Vector3(7.5f, 0f, 0f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            transform.position = startPos;
        }

        if (IsOwner)
        {
            NetworkYPosition.Value = transform.position.y;
        }
    }

    private void FixedUpdate()
    {
        if (!IsSpawned) return;

        if (IsOwner)
        {
            float input = GetInputAxis();
            rb.velocity = new Vector2(0f, input * moveSpeed);

            // Update networked Y position for others
            NetworkYPosition.Value = transform.position.y;
        }
        else
        {
            // Non-owner follows the networked Y smoothly
            Vector2 targetPos = new Vector2(rb.position.x, NetworkYPosition.Value);
            rb.MovePosition(Vector2.Lerp(rb.position, targetPos, Time.fixedDeltaTime * 10f));
        }
    }

    private float GetInputAxis()
    {
        if (!IsOwner) return 0f;

        float input = 0f;

        // Right paddle controlled by arrow keys
        if (Input.GetKey(KeyCode.UpArrow)) input = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) input = -1f;

        return input;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Right Paddle hit by Ball!");
        }
    }
}
