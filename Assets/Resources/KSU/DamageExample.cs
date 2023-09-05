using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.Udon;
using UdonSharp;
using VRC.SDKBase;
using static VRC.SDKBase.Networking;
using static VRC.SDKBase.VRCPlayerApi;
using VRC.Udon.Common;
using UnityEngine.UI;

public class DamageExample : UdonSharpBehaviour
{
    [SerializeField] private float defaultDamage = 10;

    public void Cast(PlayerStat playerStat) {
        //Check OffenseBuffs
        playerStat.MagicCast(defaultDamage);
        //Create Magic Object will be here 
    }

    public override void OnPickupUseDown()
    {
        PlayerStat playerStat = FindStat(Networking.LocalPlayer);
        playerStat.MagicHit(defaultDamage);

    }
    public void OnTriggerEnter(Collider collision) {
        PlayerStat playerStat = FindStat(collision.gameObject);
        playerStat.MagicHit(defaultDamage);

        //실험용
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(Test));
    }

    /*public void Test() {
        GameObject playerManagerTest = GameObject.Find("PlayerManagerTest");
        Text testText = playerManagerTest.GetComponent<Text>();
        PlayerSetting playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerSetting>();
        testText.text = "";
        for(int i=0; i<playerManager.GetPlayerNum(); i++) {
            if(playerManager.players[i] != null)
                testText.text = string.Concat(testText.text, playerManager.players[i].stat.GetHp());
        }
    }*/
    private PlayerStat FindStat(VRCPlayerApi player) {
        if(!Networking.LocalPlayer.IsOwner(GameObject.Find("PlayerManager"))) { Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("PlayerManager"));}
        PlayerSetting playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerSetting>();
        for(int i=0; i<playerManager.players.Length; i++) {
            if(playerManager.players[i].stat.player == player) {
                return playerManager.players[i].stat;
            }
        }
        return null;
    }
    private PlayerStat FindStat(GameObject collision) {
        return collision.GetComponent<Player>().stat;
        /*PlayerSetting playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerSetting>();
        VRCPlayerApi localPlayer = Networking.LocalPlayer;

        for(int i=0; i<playerManager.players.Length; i++) {
            if(playerManager.players[i].player == localPlayer) {
                return playerManager.players[i].stat;
            }
        }*/

        /*GameObject playerObject = collision;
        if(IsOwner(localPlayer, playerObject) && playerObject.GetComponent<PlayerStat>() != null){// && playerObject.CompareTag("Player")) {
            return playerObject.GetComponent<PlayerStat>();
        }
        else return null*/
    }
}