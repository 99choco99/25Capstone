using System.Net.Sockets;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public string local;
    PlayerMovement temp;

    private void Start()
    {
        temp = GetComponent<PlayerMovement>();
        temp.socket.On("newSpawn", (id) =>
        {
            local = temp.socket.Id;
            Debug.Log("½ÇÇà");

        });
    }
}
