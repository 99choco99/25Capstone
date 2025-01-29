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
    public Uri id;

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
    }

    void Start()
    {
        var uri = new Uri("http://localhost:3333");
        socket = new SocketIOUnity(uri);
        id = uri;

        Instantiate(playerPrefab);
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("서버에 연결됨");
        };

        socket.Connect();
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

    }

}
