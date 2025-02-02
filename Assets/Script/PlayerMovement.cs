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
        // 서버에서 플레이어의 움직임을 수신하는 이벤트 리스너 설정
        socket.On("playerMoved", (data) =>
        {
            // JSON 데이터를 Dictionary 형태로 변환
            var playerData = data.GetValue<Dictionary<string, object>>();

            // playerId와 position 정보를 가져옴
            string playerId = playerData["id"].ToString();
            // JSON 문자열을 PositionData 객체로 변환
            PositionData positionData = JsonUtility.FromJson<PositionData>(playerData["position"].ToString());
            // Vector3로 변환
            Vector3 position = new Vector3(positionData.x, positionData.y, positionData.z);
            // 플레이어의 위치를 업데이트하는 함수 호출
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
        // 플레이어의 위치를 JSON 형태로 만들어서 서버에 전송
        var data = new { position = new { x = movement.x, y = movement.y, z = movement.z } };
        socket.Emit("playerMovement", data);
    }


    // 플레이어의 위치를 업데이트하는 함수
    private void UpdatePlayerPosition(string playerId, Vector3 position)
    {
        // 플레이어 오브젝트를 찾고 위치를 업데이트하는 로직 추가
        Debug.Log($"플레이어 {playerId}의 위치를 업데이트: {position}");
        // 여기서 실제로 플레이어 오브젝트의 위치를 업데이트하는 코드를 작성합니다.
    }

}
[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}
