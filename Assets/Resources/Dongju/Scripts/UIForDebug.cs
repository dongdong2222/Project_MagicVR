﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
public class UIForDebug : UdonSharpBehaviour
{
    [SerializeField]
    private VehicleData vehicleData;
    [SerializeField]
    private GameDataTest gameData;


    [SerializeField]
    private Text value_isMaster;
    [SerializeField]
    private Text value_playerID;
    [SerializeField]
    private Text value_winner;

    private VRCPlayerApi player;
    private void Start()
    {
        player = Networking.LocalPlayer;
    }

    void LateUpdate()
    {
        UpdateText();
    }

    void UpdateText()
    {
        value_isMaster.text = "" + player.isMaster;
        value_playerID.text = "" + player.playerId;
        value_winner.text = "" + gameData.WinnerTeam;
    }
}
