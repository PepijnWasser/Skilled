using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public static class Extensions
{

	public static int seed;
	static int listItemsReturned = 0;	
	
	public static void SetSeed(int _seed)
    {
		seed = _seed;
		Debug.Log("setting seed: " + _seed);
		UnityEngine.Random.InitState(seed);
	}

	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) + 1;
		if (Arr.Length == j)
		{
			return Arr[0];
		}
		else
		{
			return Arr[j];
		}
	}

	public static T Previous<T>(this T src) where T : struct
	{
		//if its not an enum throw an error
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		//get an array of all keys in enum
		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) - 1;
		if (j < 0)
		{
			return Arr[Arr.Length - 1];
		}
		else
		{
			return Arr[j];
		}
	}

	public static T Next<T>(this IList<T> list, T item)
	{
		var nextIndex = list.IndexOf(item) + 1;

		if (nextIndex == list.Count)
		{
			return list[0];
		}

		return list[nextIndex];
	}

	public static T Previous<T>(this IList<T> list, T item)
	{
		var previousIndex = list.IndexOf(item) - 1;

		if (previousIndex == -1)
		{
			return list[list.Count - 1];
		}

		return list[previousIndex];
	}


	public static T RandomListItem<T>(this IList<T> list)
	{
		int index = UnityEngine.Random.Range(0, list.Count);

		return list[index];
	}

	public static IPAddress GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				string ips = ip.ToString();
				return IPAddress.Parse(ips);
			}
		}
		throw new Exception("No network adapters with an IPv4 address in the system!");
	}
}
