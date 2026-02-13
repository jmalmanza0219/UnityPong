using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D), typeof(NetworkObject))]
public class PaddleController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Paddle movement speed

    private Rigidbody2D rb;

    // Starting positions for left and right paddles
    private Vector3 leftStartPos = new Vector3(-15f, 0f, 0f);
    private Vector3 rightStartPos = new Vector3(15f, 0f, 0f);

    public NetworkVariable<float> NetworkYPosition = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        // Assign start position only on the server
        if (IsServer)
        {
            transform.position = OwnerClientId == 0 ? leftStartPos : rightStartPos;
        }

        // Initialize NetworkVariable to match current position
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

            // Update networked Y position
            NetworkYPosition.Value = transform.position.y;
        }
        else
        {
            // Non-owners follow the networked Y position
            Vector2 position = rb.position;
            position.y = NetworkYPosition.Value;
            rb.position = position;
        }
    }

    // Determine input based on OwnerClientId
    private float GetInputAxis()
    {
        if (!IsOwner) return 0f;

        float input = 0f;

        if (OwnerClientId == 0) // Left paddle → W/S
        {
            if (Input.GetKey(KeyCode.W)) input = 1f;
            if (Input.GetKey(KeyCode.S)) input = -1f;
        }
        else // Right paddle → Arrow keys
        {
            if (Input.GetKey(KeyCode.UpArrow)) input = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) input = -1f;
        }

        return input;
    }

    // Optional: debug collisions with the ball
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log($"{gameObject.name} was hit by the ball!");
        }
    }
}