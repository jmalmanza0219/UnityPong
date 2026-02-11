using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class NetworkedObject : NetworkBehaviour
{
    public abstract void Initialize();

    public abstract string GetNetworkID();
}
