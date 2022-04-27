using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
public class LoginServer : MonoBehaviour
{
    /*
    void OnDestroy()
    {
        ConnectionManager.Instance.Client.MessageReceived -= OnMessage;
    }
    public void ConnectedisTime()
    {

        ConnectionManager.Instance.Connected();
        if (ConnectionManager.Instance.stateServer == StateServer.DISCONNECTED)
        {
            if (toggle.isOn)
            {
                ConnectionManager.Instance.Connect("127.0.0.1", 4296);
            }
            else
            {
                ConnectionManager.Instance.Connect(IpInput.text, 4296);
            }
            InfoServe.text = "Servidor OffLine";
        }
        if (ConnectionManager.Instance.stateServer == StateServer.CONNECTED)
        {

            InfoServe.text = "Servidor Connectado";
        }

    }
    //recebe a mensagem 
    private void OnMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            switch ((Tags)message.Tag)
            {
                case Tags.LoginRequestDenied:
                    InfoServe.text = "Negado";
                    break;
                case Tags.LoginRequestAccepted:
                    OnLoginAccept();
                    break;
            }
        }
    }

    //funcao acoplado no button que envia mensagem para o servidor
    public void OnSubmitLogin()
    {
        if (!String.IsNullOrEmpty(EmailInput.text))
        {

            using (Message message = Message.Create((ushort)Tags.LoginRequest, new LoginRequestData(EmailInput.text)))
            {
                ConnectionManager.Instance.Client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    public void ServerHost()
    {
        ConnectionManager.Instance.SceneController.LoadScene("SERVIDOR");
        Destroy(ConnectionManager.Instance.Pgameobject);
    }

    private void OnLoginAccept()
    {
        ConnectionManager.Instance.SceneController.LoadScene("Lobby");

    }


    */
}
