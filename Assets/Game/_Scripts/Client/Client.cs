using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;

public class Client : MonoBehaviour {

    private string clientName;
    private int TcpPortToConnect = 1234;
    private int UdpPortToConnect = 1234;
    private string hostToConnect = "127.0.0.1";
    private TcpClient TcpSocket;
    private UdpClient UdpSocket;
    private bool TcpReady;
    private NetworkStream TcpStream;
    private StreamWriter TcpWriter;
    private StreamReader TcpReader;



    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
	}

    public bool TcpConnectToServer(string host, int port)
    {
        if (TcpReady)
            return false;

        try
        {
            TcpSocket = new TcpClient(host, port);
            TcpStream = TcpSocket.GetStream();
            TcpWriter = new StreamWriter(TcpStream);
            TcpReader = new StreamReader(TcpStream);

            TcpReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error " + e.Message);
        }

        return TcpReady;
    }

    public void TcpSend(string data)
    {
        if (!TcpReady)
            return;

        TcpWriter.WriteLine(data);
        TcpWriter.Flush();
    }

    public void UdpSend(string data)
    {
        Byte[] sendBytes = Encoding.ASCII.GetBytes(data);

        UdpSocket.Send(sendBytes, sendBytes.Length, hostToConnect, UdpPortToConnect);
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (!TcpReady)
        {
            return;
        }
        else
        {
            TcpWriter.Close();
            TcpReader.Close();
            TcpSocket.Close();
            TcpReady = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
