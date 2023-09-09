
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
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        playerManager = gameObject.GetComponent<PlayerSetting>();
        
        //플레이어 수 만큼 순회
        foreach(VRCPlayerApi player in players) {
            if(player == null) {
                continue;
            }
            else {
                //playerObject 스폰
                pool.TryToSpawn();
                Networking.SetOwner(Networking.LocalPlayer, playerManager.players[i].gameObject);
                
                /*if (!Networking.LocalPlayer.IsOwner(GameObject.Find("PlayerManager"))) { Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("PlayerManager")); }
                if (!Networking.LocalPlayer.IsOwner(clientPlayerObject)) { Networking.SetOwner(Networking.LocalPlayer, clientPlayerObject); }*/
                
                //collider에 따라다닐 player 연결
                playerManager.players[i].playerCollider.SetPlayer(player.playerId);
                //stat 초기화, player 연결
                playerManager.players[i].stat.SetPlayer(player.playerId);
            }
            i++;
        }
    }

}
