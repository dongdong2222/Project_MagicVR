
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GameManagerTest : UdonSharpBehaviour
{
    [SerializeField] private GameDataTest gameData;
    [SerializeField] private SafeBoundary[] SafeBoundary;
    public void OnGameStart(GameDataTest gameData)
    {
        this.gameData = gameData;
        SpawnPlayers();
        for(int i = 0; i < SafeBoundary.Length; i++)
            SafeBoundary[i].Open();
    }






    public void SpawnPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"{gameData.GetPlayerID(0, i)}");
            if (gameData.GetPlayerID(0, i) != -1)
                VRCPlayerApi.GetPlayerById(gameData.GetPlayerID(0, i)).TeleportTo(gameData.BlueTeamStartPoint.position, gameData.BlueTeamStartPoint.rotation);
        }
        for (int i = 0; i < 4; i++)
        {
            if (gameData.GetPlayerID(1, i) != -1)
                VRCPlayerApi.GetPlayerById(gameData.GetPlayerID(1, i)).TeleportTo(gameData.RedTeamStartPoint.position, gameData.RedTeamStartPoint.rotation);
        }

    }
}
