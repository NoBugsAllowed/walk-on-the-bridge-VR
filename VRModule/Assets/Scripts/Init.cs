using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Init : MonoBehaviour
{
    private const string MOBILE_APP_KEY = "WALK-ON-THE-BRIDGE-MOBILE";
    private const string VR_MODULE_KEY = "WALK-ON-THE-BRIDGE-VR";
    private const int PORT = 9009;
    private BridgeRotation bridgeScript;
    private IPEndPoint broadcastAddr;
    private struct UdpState
    {
        public UdpClient client;
        public IPEndPoint endPoint;
        public UdpState(UdpClient c, IPEndPoint e)
        {
            client = c;
            endPoint = e;
        }
    }

    void Start()
    {
        //Get Bridge object
        GameObject bridge = GameObject.Find("StaticBridge");
        bridgeScript = bridge.GetComponent<BridgeRotation>();

        // Start TCP listener
        IPAddress localAddr = IPAddress.Any;
        TcpListener listener = new TcpListener(localAddr, PORT);
        TcpClient mobileApp = null;
        listener.Start();
        Debug.Log("TCP Listener started");
        listener.BeginAcceptTcpClient(new AsyncCallback(AcceptMobileAppCallback), listener);

        // 
        broadcastAddr = new IPEndPoint(IPAddress.Any, PORT);
        UdpClient client = new UdpClient();
        UdpState s = new UdpState(client, broadcastAddr);
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        client.Client.Bind(broadcastAddr);
        Debug.Log("Waiting for mobile app");
        client.BeginReceive(UdpMessageReceiveCallback, s);
    }

    private void UdpMessageReceiveCallback(IAsyncResult ar)
    {
        bool mobileAppFound = false;

        UdpState us = (UdpState)(ar.AsyncState);
        UdpClient u = us.client;
        IPEndPoint e = us.endPoint;

        byte[] receivedBytes = u.EndReceive(ar, ref e);
        string receivedString = Encoding.ASCII.GetString(receivedBytes);
        Debug.Log("Received message: " + receivedString);

        // If it is our mobile app
        if (receivedString.Equals(MOBILE_APP_KEY))
        {
            try
            {
                Debug.Log("Mobile app found: " + e.Address.ToString() + ":" + e.Port.ToString());

                // Send response with proper key
                byte[] sendBytes = Encoding.ASCII.GetBytes(VR_MODULE_KEY);
                u.Send(sendBytes, sendBytes.Length, e);

                Debug.Log("Responded with a message: " + VR_MODULE_KEY);
                mobileAppFound = true;
            }
            catch (SocketException ex)
            {
                Debug.Log("Sending response to mobile app failed: " + ex.Message);
            }
        }
        // Register handler again to wait for mobile app again
        if (!mobileAppFound)
        {
            UdpClient client = new UdpClient();
            UdpState s = new UdpState(client, broadcastAddr);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.Client.Bind(broadcastAddr);
            Debug.Log("Waiting for mobile app");
            client.BeginReceive(UdpMessageReceiveCallback, s);
        }
    }

    private void AcceptMobileAppCallback(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        TcpClient mobileApp = listener.EndAcceptTcpClient(ar);

        byte[] bytes = new byte[sizeof(double)];
        using (NetworkStream stream = mobileApp.GetStream())
        {
            int length;
            // Read incomming message				
            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                Array.Reverse(bytes, 0, bytes.Length);
                double offset = BitConverter.ToDouble(bytes, 0);
                bridgeScript.Offset = offset;
                Debug.Log("Server message received as: " + offset.ToString());
            }
        }
    }
}
