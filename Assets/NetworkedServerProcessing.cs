using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkedServerProcessing
{
   

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromClient(string msg, int clientConnectionID)
    {
        Debug.Log("msg received = " + msg + ".  connection id = " + clientConnectionID);

        string[] csv = msg.Split('|');
        int check = 0;
        int signifier = 0;
        if (int.TryParse(csv[0],out check))
        {
            signifier = int.Parse(csv[0]);
        }

      switch (signifier)
        {
            case ClientToServerSignifiers.DestroyBaloonInPos:
                gameLogic.DestroyBaloon(float.Parse(csv[1]), float.Parse(csv[2]));
                break;
            case ClientToServerSignifiers.Disconnect:
                if (gameLogic.ids.Contains(clientConnectionID))
                {
                    gameLogic.ids.Remove(clientConnectionID);
                }
                break;

        }
    }
    static public void SendMessageToClient(string msg, int clientConnectionID)
    {
        networkedServer.SendMessageToClient(msg, clientConnectionID);
    }

    #endregion

    #region Connection Events

    static public void ConnectionEvent(int clientConnectionID)
    {
        Debug.Log("New Connection, ID == " + clientConnectionID);
        gameLogic.ids.AddLast(clientConnectionID);
        gameLogic.SendAllBaloonsToNewClient(clientConnectionID);
    }
    static public void DisconnectionEvent(int clientConnectionID)
    {
        Debug.Log("New Disconnection, ID == " + clientConnectionID);
        if(gameLogic.ids.Contains(clientConnectionID))
        {
            gameLogic.ids.Remove(clientConnectionID);
        }
        
    }

    #endregion

    #region Setup
    static NetworkedServer networkedServer;
    static GameLogic gameLogic;

    static public void SetNetworkedServer(NetworkedServer NetworkedServer)
    {
        networkedServer = NetworkedServer;
    }
    static public NetworkedServer GetNetworkedServer()
    {
        return networkedServer;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion
}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{

    public const int DestroyBaloonInPos = 1;
    public const int Disconnect = 2;

}

static public class ServerToClientSignifiers
{
    public const int PutNewBaloonToPos = 1;
    public const int DestroyBaloonByPos = 2;
}

#endregion

