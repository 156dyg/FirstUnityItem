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
/// �����������ͨѶ
/// </summary>
public class NetManager : MonoBehaviour
{
    public static NetManager Instance = null;
    Socket client_socket;
    byte[] message = new byte[1024];
    //������ϢҪ�Ƚ��ȳ�
    //��Ϣ����
    Queue<string> message_queue = new Queue<string>();
    public event Action<string> updataChat;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //1���߳���unity�п���ʹ��
        OnConnectedToServer();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SendMessageToServer("�㲻Ҫ��������");
        if (message_queue.Count > 0)
        {
            string message = message_queue.Dequeue();
            //���ݾ���ȥ��
            //TODO ʵ�����칦��
            updataChat(message);

        }

    }
    /// <summary>
    /// ���ͻ������Ϸ�����
    /// </summary>
    private void OnConnectedToServer()
    {
        try
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client_socket.Connect(new IPEndPoint(IPAddress.Parse(ProtocoCofig.ip), ProtocoCofig.port));
            Debug.Log("���ӳɹ�");
            //�ӷ������յ���Ϣ
            client_socket.BeginReceive(message, 0, message.Length, SocketFlags.None, ReceiveMessage, client_socket);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// �ӷ������յ���Ϣ
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
    /// �������������Ϣ
    /// </summary>
    /// <param name="message">Ҫ���͵���Ϣ</param>
    public void SendMessageToServer(string message)
    {
        client_socket.Send(Encoding.UTF8.GetBytes(message));
    }
}