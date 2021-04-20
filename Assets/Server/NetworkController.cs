using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkController : MonoBehaviour {

    private TcpClient client;

    void Start() {
        client = new TcpClient();
        client.Connect("localhost", 12000);
    }


    private int lastAvaiable = 0;
    private void Update()
    {
        if(client != null && client.Connected)
        {
            NetworkStream stream = client.GetStream();
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];

                Debug.Log(stream.Read(buffer, 0, client.ReceiveBufferSize));
                string message = Encoding.ASCII.GetString(buffer);
                Debug.Log(message);


                buffer = Encoding.UTF8.GetBytes("{\"type\":5,\"message\":\"Hello World!\"}");
                stream.Write(buffer, 0, buffer.Length);

            }
        }
    }

    private void OnDestroy()
    {
        client.Close();
        Debug.Log("The End!");
    }

}
