
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LobbyManagerTest : UdonSharpBehaviour
{
    [SerializeField] GameDataTest gameData;
    [SerializeField] GameManagerTest gameManager;
    
    public void GameStart()
    {
        Debug.Log("GameStart");
        if (Networking.IsMaster) //to do with 끼잉: master만 버튼 누르게?
        {
            gameManager.OnGameStart(gameData);
        }
    }
}
