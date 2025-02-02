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
        socket = new SocketIOUnity("http://localhost:3333");

        socket.Connect();

    }

    void Start()
    {
        socket.JsonSerializer = new NewtonsoftJsonSerializer();
        socket.On("newPlayer", (id) =>
        {
            Debug.Log("½ÇÇà");
        });

    }
    


}
