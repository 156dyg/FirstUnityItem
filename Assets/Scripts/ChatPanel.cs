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
        //按钮回调
        btn.onClick.AddListener(OnSend);
        NetManager.Instance.updataChat += UpDataContent;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 向服务器发送消息
    /// </summary>
    private void OnSend()
    {
        //输入框为空返回
        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }
        NetManager.Instance.SendMessageToServer(inputField.text);
        //显示发出的消息
        UpDataContent(inputField.text);
        //置空
        inputField.text = "";
    }
    /// <summary>
    /// 更新自己的显示框
    /// </summary>
    /// <param name="message">文本内容</param>
    private void UpDataContent(string message)
    {
        sb.Append(message + "\n");
        Debug.Log(sb.ToString());
        content.text = sb.ToString();
    }
}
