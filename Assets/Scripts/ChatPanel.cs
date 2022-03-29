using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChatPanel : MonoBehaviour
{
    Text content;
    Button btn;
    InputField inputField;
    StringBuilder sb = new StringBuilder();
    // Start is called before the first frame update
    void Start()
    {
        content = transform.Find("Scroll View").Find("Viewport").Find("Content").GetComponent<Text>();
        btn = transform.Find("Button").GetComponent<Button>();
        inputField = transform.Find("InputField").GetComponent<InputField>();
        //��ť�ص�
        btn.onClick.AddListener(OnSend);
        NetManager.Instance.updataChat += UpDataContent;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// �������������Ϣ
    /// </summary>
    private void OnSend()
    {
        //�����Ϊ�շ���
        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }
        NetManager.Instance.SendMessageToServer(inputField.text);
        //��ʾ��������Ϣ
        UpDataContent(inputField.text);
        //�ÿ�
        inputField.text = "";
    }
    /// <summary>
    /// �����Լ�����ʾ��
    /// </summary>
    /// <param name="message">�ı�����</param>
    private void UpDataContent(string message)
    {
        sb.Append(message + "\n");
        Debug.Log(sb.ToString());
        content.text = sb.ToString();
    }
}
