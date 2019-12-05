using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Init : MonoBehaviour {
    private const int PORT = 9005;
    private GameObject bridge;
    private Vector3 target;
    public struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }
    //void Awake()
    //{
    //    UnityThread.initUnityThread();
    //}
    // Use this for initialization
    void Start () {

        target = new Vector3(274.033f, 59.02f, 213.2148f);
        Thread tcpListenerThread = new Thread(new ThreadStart(()=>
        {
            IPAddress localAddr = IPAddress.Any;
            TcpListener listener = new TcpListener(localAddr, PORT);
            listener.Start();
            Debug.Log("TCP Listener started");
            TcpClient mobileApp = listener.AcceptTcpClient();
            Debug.Log("TCP Client connected");

            byte[] bytes = new byte[sizeof(double)];
            using (NetworkStream stream = mobileApp.GetStream())
            {
                int length;
                // Read incomming stream into byte arrary. 					
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    double offset = BitConverter.ToDouble(bytes, 0);
                    bridge.transform.eulerAngles = new Vector3((float)(3 * offset), 0, 0);
                    //UnityThread.executeInUpdate(() =>
                    //{
                    //    transform.eulerAngles = new Vector3((float)(3 * offset), 0, 0);
                    //});
                    Debug.Log("Server message received as: " + offset.ToString());
                }
            }
        }));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();


        IPEndPoint broadcastAddr = new IPEndPoint(IPAddress.Any, PORT);
        UdpClient client = new UdpClient();
        UdpState s = new UdpState();
        s.e = broadcastAddr;
        s.u = client;
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        client.Client.Bind(broadcastAddr);
        Debug.Log("Waiting for mobile app");
        client.BeginReceive(new AsyncCallback((ar) =>
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);

            Debug.Log("Mobile app found: " + e.Address.ToString() + ":" + e.Port.ToString());

            //IPAddress localAddr = IPAddress.Any;
            //TcpListener listener = new TcpListener(localAddr, PORT);
            //listener.Start();
            //Debug.Log("TCP Listener started");

            byte[] sendBytes = Encoding.ASCII.GetBytes("WALK-ON-THE-BRIDGE-VR");
            u.Send(sendBytes, sendBytes.Length, e);

            Debug.Log("Responded with a message: WALK-ON-THE-BRIDGE-VR");

            //TcpClient mobileApp = listener.AcceptTcpClient();
            //Socket socket = listener.AcceptSocket();
            //Debug.Log("TCP Client connected");

            //byte[] bytes = new byte[sizeof(double)];
            //using (NetworkStream stream = mobileApp.GetStream())
            //{
            //    int length;
            //    // Read incomming stream into byte arrary. 					
            //    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
            //    {
            //        double offset = BitConverter.ToDouble(bytes, 0);
            //        //UnityThread.executeInUpdate(() =>
            //        //{
            //        //    transform.eulerAngles = new Vector3((float)(3 * offset), 0, 0);
            //        //});
            //        Debug.Log("Server message received as: " + offset.ToString());
            //    }
            //}
        }), s);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
