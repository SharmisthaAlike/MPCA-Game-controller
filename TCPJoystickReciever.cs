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
    public int serverPort = 5001;
    public float moveSpeed = 20f;  // Increased speed for better responsiveness
    public float jumpForce = 10f;

    private bool isJumping = false;

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

            // Diagonal movement handling
            if (data.Contains("W") && data.Contains("D")) move += Vector3.forward + Vector3.right;   // WD
            if (data.Contains("W") && data.Contains("A")) move += Vector3.forward - Vector3.right;   // WA
            if (data.Contains("S") && data.Contains("D")) move -= Vector3.forward - Vector3.right;   // SD
            if (data.Contains("S") && data.Contains("A")) move -= Vector3.forward + Vector3.right;   // SA

            // Single direction movement
            if (data.Contains("W") && !data.Contains("A") && !data.Contains("D") && !data.Contains("S")) move += Vector3.forward;
            if (data.Contains("S") && !data.Contains("A") && !data.Contains("D") && !data.Contains("W")) move -= Vector3.forward;
            if (data.Contains("A") && !data.Contains("W") && !data.Contains("S") && !data.Contains("D")) move -= Vector3.right;
            if (data.Contains("D") && !data.Contains("W") && !data.Contains("S") && !data.Contains("A")) move += Vector3.right;

            // Apply movement
            if (move.magnitude > 0)
            {
                rb.AddForce(move.normalized * moveSpeed, ForceMode.VelocityChange);
            }

            // Handle jump
            if (data.Contains("+JUMP") && !isJumping)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
            }
            else if (!data.Contains("+JUMP"))
            {
                isJumping = false;
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
