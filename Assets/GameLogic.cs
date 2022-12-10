using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameLogic : MonoBehaviour
{
    public struct Baloon
    {
        public Vector2 baloonPos;
    }
    public LinkedList<Baloon> baloonList = new LinkedList<Baloon>();
    public LinkedList<int> ids = new LinkedList<int>();
    
    
    float durationUntilNextBalloon;
    void Start()
    {
        NetworkedServerProcessing.SetGameLogic(this);
    }

    void Update()
    {
        durationUntilNextBalloon -= Time.deltaTime;

        if (durationUntilNextBalloon < 0)
        {
            durationUntilNextBalloon = 1f;

            float screenPositionXPercent = Random.Range(0.0f, 1.0f);
            float screenPositionYPercent = Random.Range(0.0f, 1.0f);
            Vector2 screenPosition = new Vector2(screenPositionXPercent, screenPositionYPercent);
            if(ids.Count > 0) { SpawnNewBalloon(screenPosition); }
        }
        Debug.Log(ids.Count);
    }
    public void SpawnNewBalloon(Vector2 screenPosition)
    {
        
        Baloon newBaloon = new Baloon();
        newBaloon.baloonPos = new Vector2(screenPosition.x, screenPosition.y);
        baloonList.AddLast(newBaloon);
        foreach (int id in ids)
        {
            NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.PutNewBaloonToPos.ToString() + '|' + screenPosition.x + '|' + screenPosition.y, id);
        }
    }
    public void DestroyBaloon(float posX, float posY)
    {
        foreach(Baloon baloon in baloonList)
        {
            if(baloon.baloonPos == new Vector2(posX,posY))
            {
                baloonList.Remove(baloon);
                foreach (int id in ids)
                {
                    NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.DestroyBaloonByPos.ToString() + '|' + posX + '|' + posY, id);
                }
                break;
            }
        }
    }

    public void SendAllBaloonsToNewClient(int id)
    {
        foreach(Baloon baloon in baloonList)
        {
            NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.PutNewBaloonToPos.ToString() + '|' + baloon.baloonPos.x + '|' + baloon.baloonPos.y, id);
        }
    }

}
