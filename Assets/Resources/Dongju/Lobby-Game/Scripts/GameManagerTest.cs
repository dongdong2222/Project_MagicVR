
using System.Threading;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GameManagerTest : UdonSharpBehaviour
{
    [SerializeField] private GameDataTest gameData;
    [SerializeField] private SafeBoundary[] SafeBoundary;


    [UdonSynced] private float timer;
    [UdonSynced] private bool timerflag = false;
    public void OnGameStart()
    {
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,  nameof(SetStartTime));
        //SpawnPlayersToStart();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SpawnPlayersToStart));
        SetStartTime();
    }

    public void OnGamePlay()
    {
        Debug.Log("OnGamePlay");
        for (int i = 0; i < SafeBoundary.Length; i++)
            SafeBoundary[i].Open();
    }

    public void OnGameEnd(int winner)
    {
        gameData.SetWinner(winner);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SpawnPlayersToEnd));
    }






    







    public void SpawnPlayersToStart() // to do with 끼잉 : 다른 애 teleport 왜 안담..
    {
        int id = Networking.LocalPlayer.playerId;

        int team = gameData.GetPlayerTeam(id);
        if (team == 0)
            VRCPlayerApi.GetPlayerById(id).TeleportTo(gameData.BlueTeamStartPoint.position, gameData.BlueTeamStartPoint.rotation);
        else
            VRCPlayerApi.GetPlayerById(id).TeleportTo(gameData.RedTeamStartPoint.position, gameData.RedTeamStartPoint.rotation);


        //for (int i = 0; i < gameData.BlueTeamIDs_size; i++)
        //{

        //    VRCPlayerApi.GetPlayerById(gameData.GetPlayerID(0, i)).TeleportTo(gameData.BlueTeamStartPoint.position, gameData.BlueTeamStartPoint.rotation);
        //}
        //for (int i = 0; i < gameData.RedTeamIDs_size; i++)
        //{
        //    VRCPlayerApi.GetPlayerById(gameData.GetPlayerID(1, i)).TeleportTo(gameData.RedTeamStartPoint.position, gameData.RedTeamStartPoint.rotation);
        //}
    }
    public void SpawnPlayersToEnd()
    {
        //int id = Networking.LocalPlayer.playerId;
        //int team = gameData.GetPlayerTeam(id);
        //if (team == 0)
        //    VRCPlayerApi.GetPlayerById(id).TeleportTo(gameData.BlueTeamEndPoint.position, gameData.BlueTeamEndPoint.rotation);
        //else
        //    VRCPlayerApi.GetPlayerById(id).TeleportTo(gameData.RedTeamEndPoint.position, gameData.RedTeamEndPoint.rotation);
    }

    private void SetStartTime()
    {
        Debug.Log("SetStartTime");
        timerflag = true;
        timer = 5;
    }
    private void Update()
    {
        //Debug.Log($"timerflag : ");
        if (timerflag)
        {
            //Debug.Log($"timer : {timer}");
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                OnGamePlay();
                timerflag = false;
            }
        }

    }
}
