﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.Udon;
using UdonSharp;
using VRC.SDKBase;

public class PlayerSetting : UdonSharpBehaviour
{
    public GameObject PlayerPrefab;
    public Player[] players;
    //Later StartGame으로 변경할 것 
    
    void Start()
    {
        //players = new Player[30];
        GameObject clientStatObject = VRCInstantiate(PlayerPrefab);
        Networking.SetOwner(Networking.LocalPlayer, clientStatObject);
        /*players[0] = GameObject.Find("Stat").GetComponent<Player>();
        players[0].stat = clientStatObject.GetComponent<PlayerStat>();
        players[0].player = Networking.LocalPlayer;*/
        /*
        VRCPlayerApi[] players = new VRCPlayerApi[30];
        VRCPlayerApi.GetPlayers(players);
        int i = 0;
        foreach(VRCPlayerApi player in players) {
            if(player == null) continue;
            else {
                if(playerInfos[i].player == null) {
                    playerInfos[i].player = player;*/
                    //GameObject clientStatObject = VRCInstantiate(statPrefab);
                    //clientStatObject.SetActive(true);
        /*            playerInfos[i].stat = clientStatObject.GetComponent<PlayerStat>();
                    SetOwner(LocalPlayer, clientStatObject);
                }
            }
            i++;
            i %= 30;
            Debug.Log(player.displayName);
        }*/
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}