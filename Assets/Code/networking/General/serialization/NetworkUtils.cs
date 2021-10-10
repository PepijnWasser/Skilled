﻿using System;
using System.Net.Sockets;
using System.Linq;
using UnityEngine;

public static class NetworkUtils
{
	/**
	 * Writes the size of the given byte array into the stream and then the bytes themselves.
	 */
	public static void Write(NetworkStream pStream, byte[] pMessage)
	{
		//convert message length to 4 bytes and write those bytes into the stream
		pStream.Write(BitConverter.GetBytes(pMessage.Length), 0, 4);
		//now send the bytes of the message themselves
		pStream.Write(pMessage, 0, pMessage.Length);
	}


	/**
 * Writes the size of the given byte array into the stream and then the bytes themselves.
 */
	public static void Write(UdpClient client, byte[] pMessage)
	{
		//byte[] length = BitConverter.GetBytes(pMessage.Length);
		//byte[] combined = length.Concat(pMessage).ToArray();

		//client.Send(pMessage, pMessage.Length);
	}
	/**
	 * Reads the amount of bytes to receive from the stream and then the bytes themselves.
	 */
	public static byte[] Read(NetworkStream pStream)
	{
		//get the message size first
		int byteCountToRead = BitConverter.ToInt32(Read(pStream, 4), 0);
		//then read that amount of bytes
		return Read(pStream, byteCountToRead);
	}

	/**
 * Reads the amount of bytes to receive from the stream and then the bytes themselves.
 */
	public static byte[] Read(byte[] incomingBytes)
	{
		//get the message size first
		//int byteCountToRead = BitConverter.ToInt32(Read(pStream, 4), 0);
		//then read that amount of bytes
		return incomingBytes;
	}

	/**
	 * Read the given amount of bytes from the stream
	 */
	private static byte[] Read(NetworkStream pStream, int pByteCount)
	{
		//create a buffer to hold all the requested bytes
		byte[] bytes = new byte[pByteCount];
		//keep track of how many bytes we read last read operation
		int bytesRead = 0;
		//and keep track of how many bytes we've read in total
		int totalBytesRead = 0;

		try
		{
			//keep reading bytes until we've got what we are looking for or something bad happens.
			while (
				totalBytesRead != pByteCount &&
				(bytesRead = pStream.Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0
			)
			{
				totalBytesRead += bytesRead;
			}
		}
		catch { }

		return (totalBytesRead == pByteCount) ? bytes : null;
	}
}


