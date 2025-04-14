using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class JoystickTCPControl : MonoBehaviour
{
    TcpClient client;
    StreamReader reader;
    Rigidbody rb;

    public string serverIP = "192.168.126.254";  // IP of Raspberry Pi
    public int serverPort = 65431;
    public float moveSpeed = 20f;  // Increased speed for better responsiveness

    void Start()
    {
        try
        {
            // Establish connection to the Raspberry Pi
            client = new TcpClient(serverIP, serverPort);
            reader = new StreamReader(client.GetStream());
            Debug.Log("Connected to joystick server.");

            // Get the Rigidbody component for physics-based movement
            rb = GetComponent<Rigidbody>();
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e.Message);
        }
    }

    void Update()
    {
        if (client != null && client.Connected)
        {
            string data = "";

            // Check if data is available from the server
            if (client.GetStream().DataAvailable)
            {
                data = reader.ReadLine();
                Debug.Log("Received: " + data);
            }

            Vector3 move = Vector3.zero;

            // Determine movement based on received data
            if (data.Contains("W")) move += Vector3.forward;   // Move forward
            if (data.Contains("S")) move -= Vector3.forward;   // Move backward
            if (data.Contains("A")) move -= Vector3.right;     // Move left
            if (data.Contains("D")) move += Vector3.right;     // Move right

            // If there is movement data, apply movement using Rigidbody
            if (move.magnitude > 0)
            {
                rb.AddForce(move.normalized * moveSpeed, ForceMode.VelocityChange);  // Apply force in the direction
            }
        }
    }

    // Clean up the TCP connection when the application quits
    void OnApplicationQuit()
    {
        reader?.Close();
        client?.Close();
    }
}
