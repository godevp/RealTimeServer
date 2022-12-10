using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
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
            SpawnNewBalloon(screenPosition);
        }
    }
    public void SpawnNewBalloon(Vector2 screenPosition)
    {
        foreach(int id in ids)
        {
            NetworkedServerProcessing.SendMessageToClient(ServerToClientSignifiers.PutNewBaloonToPos.ToString() + '|' + screenPosition.x + ':' + screenPosition.y, id);
        }
    }
}
