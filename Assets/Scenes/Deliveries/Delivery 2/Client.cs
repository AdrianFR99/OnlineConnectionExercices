using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;


using System.Threading;

public class Client : MonoBehaviour
{

    int recv;
    byte[] data;
    String input, stringData;
    IPEndPoint ipep;
    Socket server;
    Thread listener = null;
    EndPoint Remote;


    // Start is called before the first frame update
    void Start()
    {

        data = new byte[1024];
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"),9050);//Defining local IP

        server = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);//defining socket and protocol 

        String welcome = "hello???";
        data = Encoding.ASCII.GetBytes(welcome);//encode initial message
        server.SendTo(data,data.Length,SocketFlags.None,ipep);

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
         Remote = (EndPoint)sender;


        if (listener==null)
        {
            listener = new Thread(ListenForMessages);
            listener.Start();

        }

    }


    void ListenForMessages() {


        data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        Debug.Log("Message recieve form:" + Remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));


        while (true)
        {
            input = Console.ReadLine();
            if (input == "exit")
                break;
            server.SendTo(Encoding.ASCII.GetBytes("Ping"), Remote);
            data = new byte[1024];
            recv = server.ReceiveFrom(data, ref Remote);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            Debug.Log(stringData);
        }




    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}