using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LeftPaddleController : PaddleController
{
    [SerializeField] private Vector3 startPosition = new Vector3(-7.5f, 0f, 0f);
    protected override float GetInputAxis()
    {
        if (!IsOwner) return 0f;

        return Input.GetAxis("LeftPaddle");
    }

    public override void OnNetworkSpawn()
    {

        if (IsOwner)return;
        
            transform.position = startPosition;
        
    }
}
