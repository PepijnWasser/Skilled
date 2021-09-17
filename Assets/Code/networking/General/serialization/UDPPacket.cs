using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class UDPPacket 
{
	private BinaryWriter writer;    //only used in write mode, to write bytes into a byte array
	private BinaryReader reader;    //only used in read mode, to read bytes from a byte array

	/**
	 * Create a Packet for writing.
	 */
	public UDPPacket()
	{
		//BinaryWriter wraps a Stream, in this case a MemoryStream, which in turn wraps an array of bytes
		writer = new BinaryWriter(new MemoryStream());
	}

	/**
	 * Create a Packet from an existing byte array so we can read from it
	 */
	public UDPPacket(byte[] pSource)
	{
		//BinaryReader wraps a Stream, in this case a MemoryStream, which in turn wraps an array of bytes
		reader = new BinaryReader(new MemoryStream(pSource));
	}

	/// WRITE METHODS

	public void Write(int pInt) { writer.Write(pInt); }
	public void Write(string pString) { writer.Write(pString); }
	public void Write(bool pBool) { writer.Write(pBool); }
	public void Write(float pFloat) { writer.Write(pFloat); }
	public void Write(Vector3 pVector) { writer.Write(pVector.x); writer.Write(pVector.y); writer.Write(pVector.z); }

	public void Write(USerializable pSerializable)
	{
		Write(pSerializable.GetType().FullName);
		pSerializable.Serialize(this);
	}


	/// READ METHODS

	public int ReadInt() { return reader.ReadInt32(); }
	public string ReadString() { return reader.ReadString(); }
	public bool ReadBool() { return reader.ReadBoolean(); }
	public float ReadFloat() { return reader.ReadSingle(); }
	public Vector3 ReadVector3() { return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()); }


	public USerializable ReadObject()
	{
		Type type = Type.GetType(ReadString());
		USerializable obj = (USerializable)Activator.CreateInstance(type);
		obj.Deserialize(this);
		return obj;
	}

	public T Read<T>() where T : USerializable
    {
        return (T)ReadObject();

    }
    public byte[] GetBytes()
    {
        //If we opened the Packet in writing mode, we'll probably need to send it at some point.
        //MemoryStream can either return the whole buffer or simply the part of the buffer that has been filled,
        //which is what we do here using ToArray()
        return ((MemoryStream)writer.BaseStream).ToArray();
    }
}
