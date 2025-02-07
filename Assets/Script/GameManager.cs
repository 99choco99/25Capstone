using System;
using UnityEngine;
using SocketIOClient;
using System.Collections.Generic;
using SocketIOClient.Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SocketIOUnity socket;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // Add Client Events    // On Connected
    }

    void Start()
    {
        socket.Connect();
        socket.On("connect", e => {
            Debug.Log("서버에 연결됨");
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();
        socket.On("newPlayer", (e) => {
            Debug.Log(socket.Id);
            socket.Emit("Complete"); 
        });
    }




}
