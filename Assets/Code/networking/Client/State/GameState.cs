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

    UdpClient client = new UdpClient();

    //events that trigger when certain messages are received from the server

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

    public delegate void SetKeypadOutcomeMessage(int id, bool correct);
    public static event SetKeypadOutcomeMessage setKeypadOutcomeMessage;

    public delegate void UpdateKeypadScreen(int id, string message);
    public static event UpdateKeypadScreen updateKeypadScreen;


    public delegate void TwoWayLeverCompleted(int leverID);
    public static event TwoWayLeverCompleted twoWayLeverCompleted;

    public delegate void ThreeWayLeverCompleted(int leverID);
    public static event ThreeWayLeverCompleted threeWayLeverCompleted;

    public delegate void KeypadCompleted(int keypadID);
    public static event KeypadCompleted keypadCompleted;

    public delegate void Validate(string code, int keypadID);
    public static event Validate testKeypadCode;

    public delegate void mapPosUpdater(Vector3 newPos, float newZoom);
    public static event mapPosUpdater updatePlayerCamPosition;
    public static event mapPosUpdater updateEnergyCamPosition;
    public static event mapPosUpdater updateTaskCamPosition;

    public delegate void RigidbodyUpdater(int id, Vector3 newPosition, Vector3 newRotation);
    public static event RigidbodyUpdater rigidbodyUpdater;

    public delegate void RigidbodyTouchedUpdate(int playerID, int rigidbodyID);
    public static event RigidbodyTouchedUpdate rigidbodyTouchedUpdate;

    public delegate void EnergyUserUpdated(int id, bool on);
    public static event EnergyUserUpdated energyUserUpdated;


    protected override void Awake()
    {
        base.Awake();
        MapGenerator.onCompletion += SendMapMadeMessage;
        StationHealth.stationTookDamage += SendStationHealth;
        TaskManager.taskHasError += SendNewTask;

        TwoWayLever.leverPulled += SendTwoWayLeverPos;
        ThreeWayLever.leverPulled += SendThreeWayLeverPos;
        Keypad.keypadUsed += SendKeypadStatus;

        KeypadCodeEnterer.messageUpdated += SendKeypadCode;
        KeypadTask.codeChecked += SendKeypadOutcome;
        KeypadTask.validateCode += SendValidationMessage;

        Task.taskCompleted += SendTaskCompleted;


        MapPositionUpdater.positionChanged += SendMapCameraPosition;

        EnergyUser.energyChanged += SendEnergyUserStatus;
    }

    private void OnDestroy()
    {
        MapGenerator.onCompletion -= SendMapMadeMessage;
        StationHealth.stationTookDamage -= SendStationHealth;
        TaskManager.taskHasError -= SendNewTask;

        TwoWayLever.leverPulled -= SendTwoWayLeverPos;
        ThreeWayLever.leverPulled -= SendThreeWayLeverPos;
        Keypad.keypadUsed -= SendKeypadStatus;

        KeypadCodeEnterer.messageUpdated -= SendKeypadCode;
        KeypadTask.codeChecked -= SendKeypadOutcome;

        Task.taskCompleted -= SendTaskCompleted;
        KeypadTask.validateCode -= SendValidationMessage;

        MapPositionUpdater.positionChanged -= SendMapCameraPosition;

        EnergyUser.energyChanged -= SendEnergyUserStatus;
    }

    //send a message to the server that we loaded the new scene
    //sends the udp connection data to the server
    private void Start()
    {
        SendGameLoadedMessage();
        InputManager.SetActiveActionMap(InputManager.game);

        int i = 0;
        bool finishedInitialization = false;

        while (finishedInitialization == false && i < 20)
        {
            try
            {
                client = new UdpClient(40004 + i);
                Debug.Log("listening on " + Extensions.GetLocalIPAddress() + " port " + 40004 + i);

                PlayerInfo.udpReceivePort = 40004 + i;

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

    //handle udp messages
    void recv(IAsyncResult res)
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
        else if (TempOBJ is UpdatePlayerCamPosition)
        {
            UpdatePlayerCamPosition message = TempOBJ as UpdatePlayerCamPosition;
            HandleUpdatePlayerCamPosition(message);
        }
        else if (TempOBJ is UpdateEnergyCamPosition)
        {
           UpdateEnergyCamPosition message = TempOBJ as UpdateEnergyCamPosition;
           HandleUpdateEnergyCamPosition(message);
        }
        else if (TempOBJ is UpdateTaskCamPosition)
        {
            UpdateTaskCamPosition message = TempOBJ as UpdateTaskCamPosition;
            HandleUpdateTaskCamPosition(message);
        }
        else if(TempOBJ is UpdateRigidbodyPositionResponse)
        {
            UpdateRigidbodyPositionResponse message = TempOBJ as UpdateRigidbodyPositionResponse;
            HandleUpdateRigidbodyResponse(message);
        }
        client.BeginReceive(new AsyncCallback(recv), null);
    }

    //handle tcp messages
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
                else if (tempOBJ is HeartBeat)
                {
                    RefreshHeartbeat();
                }
                else if (tempOBJ is MakenewPlayerCharacterMessage)
                {
                    MakenewPlayerCharacterMessage message = tempOBJ as MakenewPlayerCharacterMessage;
                    HandleMakePlayerCharacterMessage(message);
                }
                else if(tempOBJ is PlaceWorldObjects)
                {
                    PlaceWorldObjects message = tempOBJ as PlaceWorldObjects;
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
                else if (tempOBJ is ThreeWayLeverCompletedMessage)
                {
                    ThreeWayLeverCompletedMessage message = tempOBJ as ThreeWayLeverCompletedMessage;
                    HandleThreeWayLeverCompleted(message);
                }
                else if (tempOBJ is KeypadCompletedMessage)
                {
                    KeypadCompletedMessage message = tempOBJ as KeypadCompletedMessage;
                    HandleKeypadCompleted(message);
                }
                else if(tempOBJ is KeypadValidationMessage)
                {
                    KeypadValidationMessage message = tempOBJ as KeypadValidationMessage;
                    HandleValidationMessage(message);
                }
                else if (tempOBJ is JoinRoomMessage)
                {
                    JoinRoomMessage message = tempOBJ as JoinRoomMessage;
                    HandleJoinRoomMessage(message);
                } 
                else if(tempOBJ is RemovePlayerCharacterMessage)
                {
                    RemovePlayerCharacterMessage message = tempOBJ as RemovePlayerCharacterMessage;
                    HandleRemovePlayerCharacterMessage(message);
                }
                else if(tempOBJ is LastPlayerTouchedMessage)
                {
                    LastPlayerTouchedMessage message = tempOBJ as LastPlayerTouchedMessage;
                    HandleLastPlayerTouchedMessage(message);
                }
                else if(tempOBJ is UpdateEnergyUserStatusResponse)
                {
                    UpdateEnergyUserStatusResponse message = tempOBJ as UpdateEnergyUserStatusResponse;
                    HandleEnergyStatusResponse(message);
                }
                else if(tempOBJ is KeypadCodeUpdateResponse)
                {
                    KeypadCodeUpdateResponse message = tempOBJ as KeypadCodeUpdateResponse;
                    HandleKeypadUpdateResponse(message);
                }
                else if(tempOBJ is KeypadCodeOutcomeResponse)
                {
                    KeypadCodeOutcomeResponse message = tempOBJ as KeypadCodeOutcomeResponse;
                    HandleKeypadCodeOutcomeResponse(message);
                }else if(tempOBJ is PlayerStatusMessage)
                {
                    PlayerStatusMessage message = tempOBJ as PlayerStatusMessage;
                    HandlePlayerStatus(message);
                }
            }
            HandleHeartbeatStatus();
        }
        catch (Exception e)
        {
            if(tcpClientNetwork != null)
            {
                Debug.Log(e.Message + e.StackTrace);
                if (tcpClientNetwork.tcpClient.Connected)
                {
                    tcpClientNetwork.tcpClient.Close();
                }
            }
        }
    }

    //if the server has no more heartbeat, we leave the server
    void HandleHeartbeatStatus()
    {
        if (CheckHeartbeat() == false)
        {
            SendLeaveServerMessage();
            sceneManager.LoadScene("Main Menu");
            InputManager.SetActiveActionMap(InputManager.mainMenu);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Debug.Log("Disconnected");
            Destroy(this.gameObject);
        }
    }

    void SendLeaveServerMessage()
    {
        LeaveServermessage message = new LeaveServermessage();
        tcpClientNetwork.SendObjectThroughTCP(message);
    }


    //handle receiving

    //starts map generation on a certain worldSeed
    void HandleMakeGameMapMessage(MakeGameMapMessage message)
    {
        Extensions.SetSeed(message.worldSeed);
        mapGenerator.Initizlize(message.amountOfSectors, message.roomsPerSector);
    }

    //makes a new player character
    void HandleMakePlayerCharacterMessage(MakenewPlayerCharacterMessage message)
    {
        gameManager.MakePlayerCharacter(message.isPlayer, message.playerName, message.playerID, message.colorS);
    }

    //moves a player to the new position
    void HandleUpdatePlayerPosition(UpdatePlayerPositionUDP message)
    {
        try
        {
            UnityMainThread._instance.AddJob(() =>
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

    //makes a task manager
    void HandleMakeTaskManager(PlaceWorldObjects message)
    {
        gameManager.MakeWorldObjects(message.playerIsLeader, message.maxErrors, message.tasksOfTypeToSpawn);
    }

    //update station health
    void HandleUpdateStationHealthResponse(UpdateStationHealthResponse message)
    {
        stationHealthUpdated?.Invoke(message.stationHealth);
    }

    //add Tasks to display
    void HandleAddKeypadTask(AddKeypadTaskMessage message)
    {
        makeKeypadTask?.Invoke(message.task, message.keypadID);
    }

    void HandleAddTwoWayLeverTask(AddTwoWayLeverTask message)
    {
        makeTwoWayleverTask?.Invoke(message.task, message.leverID);
    }

    void HandleAddThreeWayLeverTask(AddThreeWayLeverTask message)
    {
        makeThreeWayLeverTask?.Invoke(message.task, message.leverID);
    }

    //updates task status-info
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


    //handles task completion
    void HandleTwoWayLeverCompleted(TwoWayLeverCompletedMessage message)
    {
        twoWayLeverCompleted?.Invoke(message.leverID);
    }

    void HandleThreeWayLeverCompleted(ThreeWayLeverCompletedMessage message)
    {
        threeWayLeverCompleted?.Invoke(message.leverID);
    }

    void HandleKeypadCompleted(KeypadCompletedMessage message)
    {
        keypadCompleted?.Invoke(message.keypadID);
    }

    //checks if the code is the correct code
    //usually the server owner is the only one that validates tasks
    void HandleValidationMessage(KeypadValidationMessage message)
    {
        testKeypadCode?.Invoke(message.code, message.keypadID);
    }

    //Handle map cam updates
    void HandleUpdatePlayerCamPosition(UpdatePlayerCamPosition message)
    {
        try
        {
            UnityMainThread._instance.AddJob(() =>
            {
                // Will run on main thread, hence issue is solved
                updatePlayerCamPosition?.Invoke(message.cameraPosition, message.zoom);
            });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void HandleUpdateEnergyCamPosition(UpdateEnergyCamPosition message)
    {
        try
        {
            UnityMainThread._instance.AddJob(() =>
            {
                // Will run on main thread, hence issue is solved
                updateEnergyCamPosition?.Invoke(message.cameraPosition, message.zoom);
            });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void HandleUpdateTaskCamPosition(UpdateTaskCamPosition message)
    {
        try
        {
            UnityMainThread._instance.AddJob(() =>
            {
                // Will run on main thread, hence issue is solved
                updateTaskCamPosition?.Invoke(message.cameraPosition, message.zoom);
            });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //loads the end scene
    void HandleJoinRoomMessage(JoinRoomMessage message)
    {
        if (message.roomToJoin == JoinRoomMessage.rooms.endScreen)
        {
            GameObject.FindObjectOfType<MySceneManager>().LoadScene("EndScene");
        }
        else
        {
            Debug.Log("request to join room failed");
        }
    }

    void HandleRemovePlayerCharacterMessage(RemovePlayerCharacterMessage message)
    {
        gameManager.RemovePlayerCharacter(message.playerID);
    }

    void HandleUpdateRigidbodyResponse(UpdateRigidbodyPositionResponse messaage)
    {
        try
        {
            UnityMainThread._instance.AddJob(() =>
            {
                // Will run on main thread, hence issue is solved
                rigidbodyUpdater?.Invoke(messaage.rigidbodyID, messaage.rigidbodyPosition, messaage.rigidbodyRotation);
            });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void HandleLastPlayerTouchedMessage(LastPlayerTouchedMessage message)
    {
        rigidbodyTouchedUpdate?.Invoke(message.playerID, message.rigidbodyID);
    }

    void HandleEnergyStatusResponse(UpdateEnergyUserStatusResponse message)
    {
        energyUserUpdated?.Invoke(message.id, message.on);
    }

    void HandleKeypadUpdateResponse(KeypadCodeUpdateResponse message)
    {
        updateKeypadScreen?.Invoke(message.id, message.message);
    }

    void HandleKeypadCodeOutcomeResponse(KeypadCodeOutcomeResponse message)
    {
        setKeypadOutcomeMessage?.Invoke(message.id, message.correct);
    }

    void HandlePlayerStatus(PlayerStatusMessage message)
    {
        if (message.muted)
        {
            gameManager.DeleteMusicPlayer();

        }
    }


    //sending
    void SendGameLoadedMessage()
    {
        SceneLoadedMessage message = new SceneLoadedMessage(SceneLoadedMessage.scenes.game);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    //sends player data
    void SendPlayerInfo()
    {
        UpdateClientInfoMessage message = new UpdateClientInfoMessage(Extensions.GetLocalIPAddress(), PlayerInfo.udpSendPort, PlayerInfo.udpReceivePort);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    //sends player position
    public void SendPlayerPosition(Vector3 position, Vector3 rotation, Vector3 targetRotation)
    {        
        UpdatePlayerPositionUDP message = new UpdatePlayerPositionUDP(position, rotation, targetRotation);
        udpClientNetwork.SendObjectThroughUDP(message);
    }

    //sends map made message
    void SendMapMadeMessage()
    {
        MapMadeMessage message = new MapMadeMessage();
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    //sends station health
    //under normal conditions the server owner is the only one that sends station health
    void SendStationHealth(int newHealth)
    {
        UpdateStationHealthRequest request = new UpdateStationHealthRequest(newHealth);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    //sends the newly generated task to all other users
    void SendNewTask(Task task, int taskID)
    {
        if(task is KeypadTask)
        {
            KeypadTask keypadTask = task as KeypadTask;
            AddKeypadTaskMessage message = new AddKeypadTaskMessage(keypadTask, keypadTask.keyPad.keypadID);
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
            AddThreeWayLeverTask message = new AddThreeWayLeverTask(threeWayLeverTask, threeWayLeverTask.lever.leverID);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
    }

    //send task status
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

    //send the completed task
    void SendTaskCompleted(Task task)
    {
        if(task is TwoWayLeverTask)
        {
            TwoWayLeverTask twoWayLeverTask = task as TwoWayLeverTask;
            TwoWayLeverCompletedMessage message = new TwoWayLeverCompletedMessage(twoWayLeverTask, twoWayLeverTask.lever.leverID);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
        else if (task is ThreeWayLeverTask)
        {
            ThreeWayLeverTask threeWayLeverTask = task as ThreeWayLeverTask;
            ThreeWayLeverCompletedMessage message = new ThreeWayLeverCompletedMessage(threeWayLeverTask, threeWayLeverTask.lever.leverID);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
        else if (task is KeypadTask)
        {
            KeypadTask keypadTask = task as KeypadTask;
            KeypadCompletedMessage message = new KeypadCompletedMessage(keypadTask, keypadTask.keyPad.keypadID);
            tcpClientNetwork.SendObjectThroughTCP(message);
        }
    }

    //send keypadCode to validate
    void SendValidationMessage(string code, int ID)
    {
        KeypadValidationMessage message = new KeypadValidationMessage(code, ID);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void SendMapCameraPosition(Vector3 newPos, float newZoom, MapPositionUpdater updater)
    {
        if(updater is PlayerMapPositionUpdater)
        {
            UpdatePlayerCamPosition updatePlayerCamPosition = new UpdatePlayerCamPosition(newPos, newZoom);
            udpClientNetwork.SendObjectThroughUDP(updatePlayerCamPosition);
        }
        else if(updater is EnergyMapPositionUpdater)
        {
            UpdateEnergyCamPosition updateEnergyCamPosition = new UpdateEnergyCamPosition(newPos, newZoom);
            udpClientNetwork.SendObjectThroughUDP(updateEnergyCamPosition);
        }
        else if (updater is TaskMapPositionUpdater)
        {
            UpdateTaskCamPosition updateTaskCamPosition = new UpdateTaskCamPosition(newPos, newZoom);
            udpClientNetwork.SendObjectThroughUDP(updateTaskCamPosition);
        }
    }

    public void SendRigidBodyPosition(int ID, Vector3 position, Vector3 rotation)
    {
        UpdateRigidbodyPositionRequest updateRigidbodyPositionRequest = new UpdateRigidbodyPositionRequest(ID, position, rotation);
        udpClientNetwork.SendObjectThroughUDP(updateRigidbodyPositionRequest);
    }

    public void SendLastPlayerTouchedMessage(int playerID, int rigidbodyID)
    {
        LastPlayerTouchedMessage message = new LastPlayerTouchedMessage(playerID, rigidbodyID);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    public void SendEnergyUserStatus(int userID, bool on)
    {
        UpdateEnergyUserStatusResponse request = new UpdateEnergyUserStatusResponse(userID, on);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    void SendKeypadCode(int id, string message)
    {
        KeypadCodeUpdateRequest request = new KeypadCodeUpdateRequest(id, message);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    void SendKeypadOutcome(int id, bool correct)
    {
        KeypadCodeOutcomeRequest request = new KeypadCodeOutcomeRequest(id, correct);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }
}
