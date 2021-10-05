using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

public class GameState : State
{
    public MapGenerator mapGenerator;
    public GameManager gameManager;
    
    public delegate void HealthUpdated(int health);
    public static event HealthUpdated stationHealthUpdated;

    public delegate void MakeKeypadTask(KeypadTask task);
    public static event MakeKeypadTask makeKeypadTask;

    public delegate void MakeTwoWayLeverTask(TwoWayLeverTask task);
    public static event MakeTwoWayLeverTask makeTwoWayleverTask;

    public delegate void MakeThreeWayLeverTask(ThreeWayLeverTask task);
    public static event MakeThreeWayLeverTask makeThreeWayLeverTask;


    protected override void Awake()
    {
        base.Awake();
        MapGenerator.OnCompletion += SendMapMadeMessage;
        StationHealth.updateStationHealth += SendStationHealth;
        TaskManager.taskHasError += SendNewTask;
    }

    private void OnDestroy()
    {
        MapGenerator.OnCompletion += SendMapMadeMessage;
        StationHealth.updateStationHealth -= SendStationHealth;
        TaskManager.taskHasError -= SendNewTask;
    }

    private void Start()
    {
        SendGameLoadedMessage();

        int i = 0;
        bool finishedInitialization = false;

        while (finishedInitialization == false && i < 20)
        {
            try
            {
                client = new UdpClient(40004 + i);
                Debug.Log("listening on " + Extensions.GetLocalIPAddress() + " port " + 40004 + i);

                PlayerInfo playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
                playerInfo.udpReceivePort = 40004 + i;

                finishedInitialization = true;
            }
            catch (Exception e)
            {
                e.ToString();
                i++;
            }
        }
        client.BeginReceive(new AsyncCallback(recv), null);
        SendPlayerInfo();
    }

    protected override void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = client.EndReceive(res, ref RemoteIP);

        UDPPacket packet = new UDPPacket(received);
        var TempOBJ = packet.ReadObject();
        if(TempOBJ is UpdatePlayerPositionUDP)
        {
            UpdatePlayerPositionUDP message = TempOBJ as UpdatePlayerPositionUDP;
            HandleUpdatePlayerPosition(message);
        }

        client.BeginReceive(new AsyncCallback(recv), null);
    }

    protected override void Update()
    {
        try
        {
            if (tcpClientNetwork != null && tcpClientNetwork.tcpClient.Connected && tcpClientNetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(tcpClientNetwork.tcpClient.GetStream());
                TCPPacket inPacket = new TCPPacket(inBytes);

                var tempOBJ = inPacket.ReadObject();

                if (tempOBJ is MakeGameMapMessage)
                {
                    MakeGameMapMessage message = tempOBJ as MakeGameMapMessage;
                    HandleMakeGameMapMessage(message);
                }
                else if (tempOBJ is MakenewPlayerCharacterMessage)
                {
                    MakenewPlayerCharacterMessage message = tempOBJ as MakenewPlayerCharacterMessage;
                    HandleMakePlayerCharacterMessage(message);
                }
                else if(tempOBJ is MakeTaskManager)
                {
                    MakeTaskManager message = tempOBJ as MakeTaskManager;
                    HandleMakeTaskManager(message);
                }
                else if(tempOBJ is UpdateStationHealthResponse)
                {
                    UpdateStationHealthResponse message = tempOBJ as UpdateStationHealthResponse;
                    HandleUpdateStationHealthResponse(message);
                }
                else if(tempOBJ is AddKeypadTaskMessage)
                {
                    AddKeypadTaskMessage message = tempOBJ as AddKeypadTaskMessage;
                    HandleAddKeypadTask(message);
                }
                else if(tempOBJ is AddTwoWayLeverTask)
                {
                    AddTwoWayLeverTask message = tempOBJ as AddTwoWayLeverTask;
                    HandleAddTwoWayLeverTask(message);
                }
                else if(tempOBJ is AddThreeWayLeverTask)
                {
                    AddThreeWayLeverTask message = tempOBJ as AddThreeWayLeverTask;
                    HandleAddThreeWayLeverTask(message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (tcpClientNetwork.tcpClient.Connected)
            {
                tcpClientNetwork.tcpClient.Close();
            }
        }
    }
    //handle receiving
    void HandleMakeGameMapMessage(MakeGameMapMessage message)
    {
        Extensions.SetSeed(message.worldSeed);
        mapGenerator.Initizlize(message.amountOfSectors);
    }

    void HandleMakePlayerCharacterMessage(MakenewPlayerCharacterMessage message)
    {
        gameManager.MakePlayerCharacter(message.isPlayer, message.characterPosition, message.playerName, message.playerID);
    }

    void HandleUpdatePlayerPosition(UpdatePlayerPositionUDP message)
    {
        try
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                // Will run on main thread, hence issue is solved
                gameManager.MovePlayer(message.playerID, message.playerPosition, message.playerRotation, message.playerNoseRotation);
            });           
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void HandleMakeTaskManager(MakeTaskManager message)
    {
        gameManager.MakeTaskmanager(message.playerIsLeader);
    }

    void HandleUpdateStationHealthResponse(UpdateStationHealthResponse message)
    {
        stationHealthUpdated?.Invoke(message.stationHealth);
    }

    void HandleAddKeypadTask(AddKeypadTaskMessage message)
    {
        makeKeypadTask?.Invoke(message.task);
    }

    void HandleAddTwoWayLeverTask(AddTwoWayLeverTask message)
    {
        makeTwoWayleverTask?.Invoke(message.task);
    }

    void HandleAddThreeWayLeverTask(AddThreeWayLeverTask message)
    {
        makeThreeWayLeverTask?.Invoke(message.task);
    }

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void SendPlayerInfo()
    {
        UpdateClientInfoMessage message = new UpdateClientInfoMessage(Extensions.GetLocalIPAddress(), playerInfo.udpSendPort, playerInfo.udpReceivePort);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    public void SendPlayerPosition(Vector3 position, Vector3 rotation, Vector3 noseRotation)
    {
        
        UpdatePlayerPositionUDP message = new UpdatePlayerPositionUDP(position, rotation, noseRotation);
        udpClientNetwork.SendObjectThroughUDP(message);       
    }

    void SendMapMadeMessage()
    {
        MapMadeMessage message = new MapMadeMessage();
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void SendStationHealth(int newHealth)
    {
        UpdateStationHealthRequest request = new UpdateStationHealthRequest(newHealth);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    void SendNewTask(Task task)
    {
        if(task is KeypadTask)
        {
            KeypadTask keypadTask = task as KeypadTask;
            AddKeypadTaskMessage message = new AddKeypadTaskMessage(keypadTask);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
        else if(task is TwoWayLeverTask)
        {
            TwoWayLeverTask twoWayLeverTask = task as TwoWayLeverTask;
            AddTwoWayLeverTask message = new AddTwoWayLeverTask(twoWayLeverTask);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
        else if(task is ThreeWayLeverTask)
        {
            ThreeWayLeverTask threeWayLeverTask = task as ThreeWayLeverTask;
            AddThreeWayLeverTask message = new AddThreeWayLeverTask(threeWayLeverTask);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
    }
}
