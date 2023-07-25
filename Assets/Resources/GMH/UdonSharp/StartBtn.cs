
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class StartBtn : UdonSharpBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;
    public void BtnClick()
    {
        // Checking player who pressed button is master
        if (Networking.LocalPlayer.isMaster)
        {
            // spawn players to each teams by LobbyManager
            lobbyManager.spawnPlayers();           
        }

    }
}
