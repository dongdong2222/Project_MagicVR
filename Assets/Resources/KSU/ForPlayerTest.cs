
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using UnityEngine.UI;
using Unity.Collections;
using VRC.Udon;
using VRC.Udon.Common;
using VRC.SDKBase.Network;
using VRC.SDK3.Components;

public class ForPlayerTest : UdonSharpBehaviour
{
    public GameObject playerPrefab;
    public PlayerSetting playerManager;
    public GameObject playerManagerTest;
    public VRCObjectPool pool;
    
    //게임 시작시 한번 실행되어야 함
    public override void OnPickupUseDown()
    {
        MakePlayers();//SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(MakePlayers));
    }

    public void MakePlayers()
    {
        //players에 현재 모든 유저 불러오기(시험용), 나중에 로비 매니저에서 특정 참가 유저들만 넣을것
        VRCPlayerApi[] players = new VRCPlayerApi[30];
        VRCPlayerApi.GetPlayers(players);
        
        int i = 0;
        Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("PlayerManager"));
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerSetting>();
        
        playerManager.SetPlayerNum(players.Length);
        //플레이어 수 만큼 순회
        foreach(VRCPlayerApi player in players) {
            if(player == null) {
                continue;
            }
            else {
                //playerObject 스폰
                GameObject clientPlayerObject = pool.TryToSpawn();//VRCInstantiate(playerPrefab);
                Networking.SetOwner(Networking.LocalPlayer, clientPlayerObject);
                
                /*if (!Networking.LocalPlayer.IsOwner(GameObject.Find("PlayerManager"))) { Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("PlayerManager")); }
                if (!Networking.LocalPlayer.IsOwner(clientPlayerObject)) { Networking.SetOwner(Networking.LocalPlayer, clientPlayerObject); }*/
                
                //collider에 따라다닐 player 연결
                playerManager.players[i].playerCollider.SetPlayer(player.playerId);
                //stat 초기화, player 연결
                playerManager.players[i].stat.SetPlayer(player.playerId);
                playerManager.players[i].stat.Initialize();

            }
            i++;
            i %= 30;
        }
    }

}
