
using System;
using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class InteractTeamButton : UdonSharpBehaviour
{
    [SerializeField] private VRCPlayerApi playerWhoPressed;
    [SerializeField] private LobbyManager LobbyManager;

    [SerializeField] private int teamNow; // Red = 0, Blue = 1
    [SerializeField] private int teamEnemy;
    [SerializeField] private int maxSize = 3;

    public override void Interact()
    {
        playerWhoPressed = Networking.LocalPlayer;

        if (LobbyManager.CheckTeam(playerWhoPressed.playerId, teamNow) == true)
        {
            LobbyManager.ErasePlayer(playerWhoPressed.playerId, teamNow);
        }
        else
        {
            //상대 팀에 플레이어 있는지 검사
            if (LobbyManager.CheckTeam(playerWhoPressed.playerId, teamEnemy) == true)
            {
                LobbyManager.ErasePlayer(playerWhoPressed.playerId, teamEnemy);
            }
            LobbyManager.AddPlayer(playerWhoPressed.playerId, teamNow);
        }
    }
}