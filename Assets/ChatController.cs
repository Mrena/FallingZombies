using System;
using System.Xml.Schema;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ChatController : MonoBehaviour
{

    private static string name = "";
    private static int port = 8080;
    private static IPAddress ip;
    private static Socket sck;
    private static Thread rec;

    static void recV()
    {
        while (true)
        {
            Thread.Sleep(500);
            byte[] Buffer = new byte[255];
            int rec = sck.Receive(Buffer, 0, Buffer.Length, 0);
            Array.Resize(ref Buffer,rec);
            Debug.Log(Encoding.Default.GetString(Buffer));
        }
    }

    void Awake()
    {
        /*
        rec = new Thread(recV);

        ip = IPAddress.Parse("27.0.0.1");
        sck = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        sck.Connect(new IPEndPoint(ip,port));
        rec.Start();

        if (sck.Connected)
        {
            byte[] sdata = Encoding.Default.GetBytes("Hello World!");
            sck.Send(sdata, 0, sdata.Length, 0);
        }
        else
        {
            Debug.Log("Could not connect to the socket server!");
        }
        */

    }

    public void SendText(string message)
    {
        if (sck.Connected)
        {
            byte[] sdata = Encoding.Default.GetBytes(message);
            sck.Send(sdata, 0, sdata.Length, 0);
        }
        else
        {
            Debug.Log("Socket is not connected");
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
