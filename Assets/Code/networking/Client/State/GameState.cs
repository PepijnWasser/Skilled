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

    public delegate void MakeKeypadTask(KeypadTask task, int keypadID);
    public static event MakeKeypadTask makeKeypadTask;

    public delegate void MakeTwoWayLeverTask(TwoWayLeverTask task, int leverID);
    public static event MakeTwoWayLeverTask makeTwoWayleverTask;

    public delegate void MakeThreeWayLeverTask(ThreeWayLeverTask task, int leverID);
    public static event MakeThreeWayLeverTask makeThreeWayLeverTask;


    public delegate void UpdateTwoWayLeverPos(UpdateTwoWayLeverPositionMessage message);
    public static event UpdateTwoWayLeverPos updateTwoWayLeverPos;

    public delegate void UpdateThreeWayLeverPos(UpdateThreeWayLeverPositionMessage message);
    public static event UpdateThreeWayLeverPos updateThreeWayLeverPos;

    public delegate void UpdateKeypadStatus(UpdateKeypadStatusMessage message);
    public static event UpdateKeypadStatus updateKeypadStatus;


    public delegate void TwoWayLeverCompleted(int leverID);
    public static event TwoWayLeverCompleted twoWayLeverCompleted;


    protected override void Awake()
    {
        base.Awake();
        MapGenerator.OnCompletion += SendMapMadeMessage;
        StationHealth.updateStationHealth += SendStationHealth;
        TaskManager.taskHasError += SendNewTask;

        TwoWayLever.leverPulled += SendTwoWayLeverPos;
        ThreeWayLever.leverPulled += SendThreeWayLeverPos;
        Keypad.keypadUsed += SendKeypadStatus;

        Task.taskCompleted += SendTaskCompleted;
    }

    private void OnDestroy()
    {
        MapGenerator.OnCompletion += SendMapMadeMessage;
        StationHealth.updateStationHealth -= SendStationHealth;
        TaskManager.taskHasError -= SendNewTask;

        TwoWayLever.leverPulled += SendTwoWayLeverPos;
        ThreeWayLever.leverPulled -= SendThreeWayLeverPos;
        Keypad.keypadUsed -= SendKeypadStatus;

        Task.taskCompleted -= SendTaskCompleted;
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
                else if(tempOBJ is UpdateTwoWayLeverPositionMessage)
                {
                    UpdateTwoWayLeverPositionMessage message = tempOBJ as UpdateTwoWayLeverPositionMessage;
                    HandleUpdateTwoWayLeverPosition(message);
                }
                else if (tempOBJ is UpdateThreeWayLeverPositionMessage)
                {
                    UpdateThreeWayLeverPositionMessage message = tempOBJ as UpdateThreeWayLeverPositionMessage;
                    HandleUpdateThreeWayLeverPosition(message);
                }
                else if(tempOBJ is UpdateKeypadStatusMessage)
                {
                    UpdateKeypadStatusMessage message = tempOBJ as UpdateKeypadStatusMessage;
                    HandleUpdateKeypadStatus(message);
                }
                else if(tempOBJ is TwoWayLeverCompletedMessage)
                {
                    TwoWayLeverCompletedMessage message = tempOBJ as TwoWayLeverCompletedMessage;
                    HandleTwoWayLeverCompleted(message);
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
        makeKeypadTask?.Invoke(message.task, 0);
    }

    void HandleAddTwoWayLeverTask(AddTwoWayLeverTask message)
    {
        makeTwoWayleverTask?.Invoke(message.task, message.leverID);
    }

    void HandleAddThreeWayLeverTask(AddThreeWayLeverTask message)
    {
        makeThreeWayLeverTask?.Invoke(message.task, 0);
    }

    void HandleUpdateTwoWayLeverPosition(UpdateTwoWayLeverPositionMessage message)
    {
        updateTwoWayLeverPos?.Invoke(message);
    }

    void HandleUpdateThreeWayLeverPosition(UpdateThreeWayLeverPositionMessage message)
    {
        updateThreeWayLeverPos?.Invoke(message);
    }

    void HandleUpdateKeypadStatus(UpdateKeypadStatusMessage message)
    {
        updateKeypadStatus?.Invoke(message);
    }

    void HandleTwoWayLeverCompleted(TwoWayLeverCompletedMessage message)
    {
        twoWayLeverCompleted?.Invoke(message.leverID);
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

    void SendNewTask(Task task, int taskID)
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
            AddTwoWayLeverTask message = new AddTwoWayLeverTask(twoWayLeverTask, twoWayLeverTask.lever.leverID);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
        else if(task is ThreeWayLeverTask)
        {
            ThreeWayLeverTask threeWayLeverTask = task as ThreeWayLeverTask;
            AddThreeWayLeverTask message = new AddThreeWayLeverTask(threeWayLeverTask);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
    }

    void SendTwoWayLeverPos(int ID, int newPos)
    {
        UpdateTwoWayLeverPositionMessage message = new UpdateTwoWayLeverPositionMessage(newPos, ID);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void SendThreeWayLeverPos(int ID, int newPos)
    {
        UpdateThreeWayLeverPositionMessage message = new UpdateThreeWayLeverPositionMessage(newPos, ID);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }
    void SendKeypadStatus(int ID, bool inUse)
    {
        UpdateKeypadStatusMessage message = new UpdateKeypadStatusMessage(ID, inUse);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void SendTaskCompleted(Task task)
    {
        if(task is TwoWayLeverTask)
        {
            TwoWayLeverTask twoWayLeverTask = task as TwoWayLeverTask;
            TwoWayLeverCompletedMessage message = new TwoWayLeverCompletedMessage(twoWayLeverTask, twoWayLeverTask.lever.leverID);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
    }
}
