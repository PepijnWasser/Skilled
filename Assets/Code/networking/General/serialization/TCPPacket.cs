using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * The Packet class provides a simple wrapper around an array of bytes (in the form of a MemoryStream), 
 * that allows us to write/read values to/from the Packet easily. 
 * Additionally it abstract/decouples how (de)serialization is done from the rest of the application.
 * 
 * All the application knows about are packets, no matter how you implemented the (de)serialization itself.
 */

public class TCPPacket
{
	private BinaryWriter writer;    //only used in write mode, to write bytes into a byte array
	private BinaryReader reader;    //only used in read mode, to read bytes from a byte array

	/**
	 * Create a Packet for writing.
	 */
	public TCPPacket()
	{
		//BinaryWriter wraps a Stream, in this case a MemoryStream, which in turn wraps an array of bytes
		writer = new BinaryWriter(new MemoryStream());
	}

	/**
	 * Create a Packet from an existing byte array so we can read from it
	 */
	public TCPPacket(byte[] pSource)
	{
		//BinaryReader wraps a Stream, in this case a MemoryStream, which in turn wraps an array of bytes
		reader = new BinaryReader(new MemoryStream(pSource));
	}

	/// WRITE METHODS

	public void Write(int pInt) { writer.Write(pInt); }
	public void Write(string pString) { writer.Write(pString); }
	public void Write(bool pBool) { writer.Write(pBool); }
	public void Write(float pFloat) { writer.Write(pFloat); }

	public void Write(IPAddress pIP) { string ips = pIP.ToString(); writer.Write(ips); }
	public void Write(Vector3 pVector) { writer.Write(pVector.x); writer.Write(pVector.y); writer.Write(pVector.z); }

	public void Write(TwoWayLeverTask task) { writer.Write(task.taskName); writer.Write(task.targetPosition); }
	public void Write(ThreeWayLeverTask task) { writer.Write(task.taskName); writer.Write(task.targetPosition); }
	public void Write(KeypadTask task) { writer.Write(task.taskName); writer.Write(task.code); }

	public void Write(ISerializable pSerializable)
	{
		Write(pSerializable.GetType().FullName);
		pSerializable.Serialize(this);
	}

	/// READ METHODS

	public int ReadInt() { return reader.ReadInt32(); }
	public string ReadString() { return reader.ReadString(); }
	public bool ReadBool() { return reader.ReadBoolean(); }
	public float ReadFloat() { return reader.ReadSingle(); }

	public IPAddress ReadIP() { return IPAddress.Parse(reader.ReadString()); }
	public Vector3 ReadVector3() { return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()); }

	public ThreeWayLeverTask ReadThreeWayLeverTask() 
	{ 
		GameObject tempO = new GameObject(); 
		ThreeWayLeverTask task = tempO.AddComponent<ThreeWayLeverTask>();
		task.taskName = reader.ReadString(); task.targetPosition = reader.ReadInt32();
		GameObject.Destroy(tempO);
		return task;
	}

	public TwoWayLeverTask ReadTwoWayLeverTask()
	{
		GameObject tempO = new GameObject();
		TwoWayLeverTask task = tempO.AddComponent<TwoWayLeverTask>();
		task.taskName = reader.ReadString(); task.targetPosition = reader.ReadInt32();
		GameObject.Destroy(tempO);
		return task;
	}

	public KeypadTask ReadKeypadTask()
	{
		GameObject tempO = new GameObject();
		KeypadTask task = tempO.AddComponent<KeypadTask>();
		task.taskName = reader.ReadString(); task.code = reader.ReadString();
		GameObject.Destroy(tempO);
		return task;
	}

	public ISerializable ReadObject()
	{
		Type type = Type.GetType(ReadString());
		ISerializable obj = (ISerializable)Activator.CreateInstance(type);
		obj.Deserialize(this);
		return obj;
	}

	public T Read<T>() where T : ISerializable
	{
		return (T)ReadObject();
	}

	/**
	 * Return the bytes that have been written into this Packet.
	 * Only works in Write mode.
	 */
	public byte[] GetBytes()
	{
		//If we opened the Packet in writing mode, we'll probably need to send it at some point.
		//MemoryStream can either return the whole buffer or simply the part of the buffer that has been filled,
		//which is what we do here using ToArray()
		return ((MemoryStream)writer.BaseStream).ToArray();
	}

}

