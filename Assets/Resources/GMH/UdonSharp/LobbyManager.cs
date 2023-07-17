using System;
using System.Linq;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class LobbyManager : UdonSharpBehaviour
{
    [SerializeField] private int maxSize = 3;
    [SerializeField] private int idx;
    [SerializeField] private int[] tempArrRed; // temp
    [SerializeField] private int[] tempArrBlue; // temp
    [SerializeField] private GameObject redPlace; // Place where red team members are spawn
    [SerializeField] private GameObject bluePlace; // Place where blue team members are spawn.

    public GameObject pushedBtnRed;
    public GameObject pushedBtnBlue;

    [SerializeField, UdonSynced(), FieldChangeCallback(nameof(playerTeamRed))] private int[] _playerTeamRed;
    public int[] playerTeamRed
    {
        get => _playerTeamRed;
        set
        {
            _playerTeamRed = value;
            pushedBtnRed.GetComponent<InteractTeamButton>().UpdateTextArr(); // update mainUI
        }
    }
    [SerializeField, UdonSynced(), FieldChangeCallback(nameof(playerTeamBlue))] private int[] _playerTeamBlue;
    public int[] playerTeamBlue
    {
        get => _playerTeamBlue;
        set
        {
            _playerTeamBlue = value;
            pushedBtnBlue.GetComponent<InteractTeamButton>().UpdateTextArr(); // update mainUI
        }
    }

    public void Start()
    {
        tempArrRed = new int[maxSize];
        tempArrBlue = new int[maxSize];
        idx = 0;
        for (int i = 0; i < maxSize; i++)
        {
            tempArrRed[i] = -1;
            tempArrBlue[i] = -1;
            playerTeamRed[i] = -1;
            playerTeamBlue[i] = -1;
        }


        // To Do : 팀 매치하던 중간에 플레이어 튕기면 Master 기준으로 Update 한번 더 하도록 구현
    }

    public bool CheckTeam(int playerId, int teamNow) // Check what team the player is on
    {
        if (teamNow == 0)
        {
            for (int i = 0; i < maxSize; i++)
            {
                if (playerTeamRed[i] == playerId)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < maxSize; i++)
            {
                if (playerTeamBlue[i] == playerId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ErasePlayer(int playerId, int teamNow) // Remove the player's name if the player already belongs to the team
    { 
        if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }

        if (teamNow == 0)
        {
            idx = Array.IndexOf(playerTeamRed, playerId);
            //playerTeamRed[idx] = -1;
            tempArrRed[idx] = -1;
            playerTeamRed = tempArrRed;
        }
        else
        {
            idx = Array.IndexOf(playerTeamBlue, playerId);
            //playerTeamBlue[idx] = -1;
            tempArrBlue[idx] = -1;
            playerTeamBlue = tempArrBlue;
        }

        RequestSerialization();
    }

    public void AddPlayer(int playerId, int teamNow) // Add the player's name if the player already not belongs to the team
    {
        if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }

        if (teamNow == 0)
        {
            for (int i = 0; i < maxSize; i++)
            {
                if (playerTeamRed[i] == -1)
                {
                    tempArrRed[i] = playerId;
                    playerTeamRed = tempArrRed;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerTeamBlue.Length; i++)
            {
                if (playerTeamBlue[i] == -1)
                {
                    tempArrBlue[i] = playerId;
                    playerTeamBlue = tempArrBlue;
                    break;
                }
            }
        }

        RequestSerialization();
    }

    public void spawnPlayers()
    {
        // To do : Check the number of redTeamMembers equals the number of blueTeamMembers by using if-

        // Check how many players want to join in the game
        for (int i=0; i<maxSize; i++)
        {
            if (playerTeamRed[i] != -1)
            {
                // Red Players spawn in placeRed
                VRCPlayerApi.GetPlayerById(playerTeamRed[i]).TeleportTo(redPlace.transform.position, redPlace.transform.rotation);

            }
            
            if (playerTeamBlue[i] != -1)
            {
                // Blue Players spawn in placeBlue
                VRCPlayerApi.GetPlayerById(playerTeamBlue[i]).TeleportTo(bluePlace.transform.position, bluePlace.transform.rotation);
            }
        }
    }
}
