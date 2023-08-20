
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
        if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }

        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SpawnPlayers));
        
        // START UI 글로벌로 꺼버리기

    }

    public void SpawnPlayers()
    {
        if (lobbyManager.playerTeamRedOne != -1)
        {
            VRCPlayerApi.GetPlayerById(lobbyManager.playerTeamRedOne).TeleportTo(lobbyManager.redPlace.transform.position, lobbyManager.redPlace.transform.rotation);
        }
        if (lobbyManager.playerTeamRedTwo != -1)
        {
            VRCPlayerApi.GetPlayerById(lobbyManager.playerTeamRedTwo).TeleportTo(lobbyManager.redPlace.transform.position, lobbyManager.redPlace.transform.rotation);
        }
        if (lobbyManager.playerTeamRedThree != -1)
        {
            VRCPlayerApi.GetPlayerById(lobbyManager.playerTeamRedThree).TeleportTo(lobbyManager.redPlace.transform.position, lobbyManager.redPlace.transform.rotation);
        }
        if (lobbyManager.playerTeamBlueOne != -1)
        {
            VRCPlayerApi.GetPlayerById(lobbyManager.playerTeamBlueOne).TeleportTo(lobbyManager.bluePlace.transform.position, lobbyManager.bluePlace.transform.rotation);
        }
        if (lobbyManager.playerTeamBlueTwo != -1)
        {
            VRCPlayerApi.GetPlayerById(lobbyManager.playerTeamBlueTwo).TeleportTo(lobbyManager.bluePlace.transform.position, lobbyManager.bluePlace.transform.rotation);
        }
        if (lobbyManager.playerTeamBlueThree != -1)
        {
            VRCPlayerApi.GetPlayerById(lobbyManager.playerTeamBlueThree).TeleportTo(lobbyManager.bluePlace.transform.position, lobbyManager.bluePlace.transform.rotation);
        }

        // 여기서 팀 정보 Send Event 하면 될 것 같음
    }
}
