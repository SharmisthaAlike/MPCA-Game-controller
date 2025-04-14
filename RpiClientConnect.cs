using System;
using System.Net.Sockets;
using UnityEngine;

public class RpiConnectionCheck : MonoBehaviour
{
    public string raspberryPiIP = "XXX.XXX.XXX.XXX";  // ← Replace with your RPi's IP
    public int port = 65432;

    void Start()
    {
        CheckConnection();
    }

    void CheckConnection()
    {
        try
        {
            using (TcpClient client = new TcpClient())
            {
                client.ReceiveTimeout = 2000;
                client.SendTimeout = 2000;

                client.Connect(raspberryPiIP, port);
                if (client.Connected)
                {
                    Debug.Log("✅ Connected to Raspberry Pi!");
                }
                else
                {
                    Debug.LogWarning("⚠️ Failed to connect to Raspberry Pi.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Connection failed: " + e.Message);
        }
    }
}
