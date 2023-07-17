using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.Udon;
using UdonSharp;
using VRC.SDKBase;
using static VRC.SDKBase.Networking;
using static VRC.SDKBase.VRCPlayerApi;
using VRC.Udon.Common;

public class DamageExample : UdonSharpBehaviour
{
    [SerializeField] private float defaultDamage = 10;

    public void Cast(PlayerStat playerStat) {
        //Check OffenseBuffs
        playerStat.MagicCast(defaultDamage);
        //Create Magic Object will be here
        
    }

    private void OnTriggerEnter(Collider collision) {
        PlayerStat playerStat = FindStat(collision.gameObject);
        playerStat.MagicHit(defaultDamage);
    }

    private PlayerStat FindStat(GameObject collision) {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        GameObject playerObject = collision;
        if(IsOwner(localPlayer, playerObject) && playerObject.GetComponent<Player>() != null){// && playerObject.CompareTag("Player")) {
            Debug.Log("Owner");
            return playerObject.GetComponent<PlayerStat>();
        }
        else return null;
    }
}