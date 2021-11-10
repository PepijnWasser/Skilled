using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadedMessage : ISerializable
{
    public enum scenes
    {
        lobby,
        game,
        endScreen
    }

    string sceneJoinedS;
    public scenes sceneJoined;

    public void Deserialize(TCPPacket pPacket)
    {
        sceneJoinedS = pPacket.ReadString();
        sceneJoined = (scenes)Enum.Parse(typeof(scenes), sceneJoinedS);
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(sceneJoinedS);
    }

    public SceneLoadedMessage()
    {

    }

    public SceneLoadedMessage(scenes _sceneJoined)
    {
        sceneJoinedS = _sceneJoined.ToString();
    }
}
