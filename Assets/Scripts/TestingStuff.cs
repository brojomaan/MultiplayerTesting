using Managers;
using UnityEngine;
using Unity.Netcode;

public class TestingStuff : NetworkBehaviour
{
    [Rpc(SendTo.Server)]
    private void HelloServerRpc()
    {
        Debug.Log("Hello Server!");
    }
    private void OnMouseDown()
    {
        HelloServerRpc();
    }
}
