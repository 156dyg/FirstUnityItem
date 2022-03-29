using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using Protocol;
using System.Threading;
/// <summary>
/// 与服务器进行通讯
/// </summary>
public class NetManager : MonoBehaviour
{
    public static NetManager Instance = null;
    Socket client_socket;
    byte[] message = new byte[1024];
    //网络信息要先进先出
    //消息队列
    Queue<string> message_queue = new Queue<string>();
    public event Action<string> updataChat;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //1多线程在unity中可以使用
        OnConnectedToServer();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SendMessageToServer("你不要过来啊！");
        if (message_queue.Count > 0)
        {
            string message = message_queue.Dequeue();
            //根据具体去做
            //TODO 实现聊天功能
            updataChat(message);

        }

    }
    /// <summary>
    /// 将客户端连上服务器
    /// </summary>
    private void OnConnectedToServer()
    {
        try
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client_socket.Connect(new IPEndPoint(IPAddress.Parse(ProtocoCofig.ip), ProtocoCofig.port));
            Debug.Log("连接成功");
            //从服务器收到消息
            client_socket.BeginReceive(message, 0, message.Length, SocketFlags.None, ReceiveMessage, client_socket);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// 从服务器收到消息
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveMessage(IAsyncResult ar)
    {
        try
        {
            int length = client_socket.EndReceive(ar);
            string server_message = Encoding.UTF8.GetString(message, 0, length);
            message_queue.Enqueue(server_message);
            client_socket.BeginReceive(message, 0, message.Length, SocketFlags.None, ReceiveMessage, client_socket);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    /// <summary>
    /// 向服务器发送信息
    /// </summary>
    /// <param name="message">要发送的信息</param>
    public void SendMessageToServer(string message)
    {
        client_socket.Send(Encoding.UTF8.GetBytes(message));
    }
}