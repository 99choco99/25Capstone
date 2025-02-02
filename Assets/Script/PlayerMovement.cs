using System;
using System.Net.Sockets;
using UnityEngine;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using System.Collections.Generic;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour { 
    PlayerInput playerInput;
    Rigidbody rb;
    SocketIOUnity socket;

    public float moveSpeed;
    Vector3 moveDistance;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        socket = GameManager.instance.socket;
        // �������� �÷��̾��� �������� �����ϴ� �̺�Ʈ ������ ����
        socket.On("playerMoved", (data) =>
        {
            // JSON �����͸� Dictionary ���·� ��ȯ
            var playerData = data.GetValue<Dictionary<string, object>>();

            // playerId�� position ������ ������
            string playerId = playerData["id"].ToString();
            // JSON ���ڿ��� PositionData ��ü�� ��ȯ
            PositionData positionData = JsonUtility.FromJson<PositionData>(playerData["position"].ToString());
            // Vector3�� ��ȯ
            Vector3 position = new Vector3(positionData.x, positionData.y, positionData.z);
            // �÷��̾��� ��ġ�� ������Ʈ�ϴ� �Լ� ȣ��
           // UpdatePlayerPosition(playerId,position);
        });
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        moveDistance = new Vector3(playerInput.moveHorizontal, 0, playerInput.moveVertical);
        moveDistance = rb.transform.position + moveDistance.normalized * moveSpeed * Time.deltaTime;
        SendPlayerMovement(moveDistance);
        rb.MovePosition(moveDistance);
    }

    private void SendPlayerMovement(Vector3 movement)
    {
        // �÷��̾��� ��ġ�� JSON ���·� ���� ������ ����
        var data = new { position = new { x = movement.x, y = movement.y, z = movement.z } };
        socket.Emit("playerMovement", data);
    }


    // �÷��̾��� ��ġ�� ������Ʈ�ϴ� �Լ�
    private void UpdatePlayerPosition(string playerId, Vector3 position)
    {
        // �÷��̾� ������Ʈ�� ã�� ��ġ�� ������Ʈ�ϴ� ���� �߰�
        Debug.Log($"�÷��̾� {playerId}�� ��ġ�� ������Ʈ: {position}");
        // ���⼭ ������ �÷��̾� ������Ʈ�� ��ġ�� ������Ʈ�ϴ� �ڵ带 �ۼ��մϴ�.
    }

}
[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}
