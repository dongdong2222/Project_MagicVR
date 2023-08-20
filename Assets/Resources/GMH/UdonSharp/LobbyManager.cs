using JetBrains.Annotations;
using System;
using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class LobbyManager : UdonSharpBehaviour
{
    [SerializeField] private int maxSize = 3; // 인원수
    [SerializeField] public GameObject redPlace; // Place where red team members are spawn
    [SerializeField] public GameObject bluePlace; // Place where blue team members are spawn.
    [SerializeField] private GameObject[] redTeamMembers;
    [SerializeField] private GameObject[] blueTeamMembers;

    public GameObject pushedBtnRed;
    public GameObject pushedBtnBlue;

    [SerializeField, UdonSynced()] public int playerTeamRedOne = -1;
    [SerializeField, UdonSynced()] public int playerTeamRedTwo = -1;
    [SerializeField, UdonSynced()] public int playerTeamRedThree = -1;
    [SerializeField, UdonSynced()] public int playerTeamBlueOne = -1;
    [SerializeField, UdonSynced()] public int playerTeamBlueTwo = -1;
    [SerializeField, UdonSynced()] public int playerTeamBlueThree = -1; // Array Sync가 안먹어서 일일히 변수 삽입..

    public void Start()
    {
        UpdateUI();
    }

    //public void Update()
    //{
    //    // To Do : 팀 매치하던 중간에 플레이어 튕기거나, 중간에 새로운 플레이어가 들어오면 Master 기준으로 Update 한번 더 하도록 구현
    //}

    public void UpdateUI()
    {
        InputText(playerTeamRedOne, redTeamMembers[0]);
        InputText(playerTeamRedTwo, redTeamMembers[1]);
        InputText(playerTeamRedThree, redTeamMembers[2]);

        InputText(playerTeamBlueOne, blueTeamMembers[0]);
        InputText(playerTeamBlueTwo, blueTeamMembers[1]);
        InputText(playerTeamBlueThree, blueTeamMembers[2]);
    }

    public void InputText(int checkId, GameObject name)
    {
        if (checkId == -1)
        {
            name.GetComponent<TextMeshProUGUI>().text = "";
        } else
        {
            name.GetComponent<TextMeshProUGUI>().text = VRCPlayerApi.GetPlayerById(checkId).displayName;
        }
    }

    public bool CheckTeam(int playerId, int teamNow) // Check what team the player is on
    {
        if (teamNow == 0)
        {
            if (playerTeamRedOne == playerId || playerTeamRedTwo == playerId || playerTeamRedThree == playerId)
            {
                return true;
            }
        }
        else
        {
            if (playerTeamBlueOne == playerId || playerTeamBlueTwo == playerId || playerTeamBlueThree == playerId)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangePlayerId(int checkId, int playerId, int teamNow)
    {
        if (teamNow == 0)
        {
            if (playerTeamRedOne == checkId)
            {
                playerTeamRedOne = playerId;
            }
            else if (playerTeamRedTwo == checkId)
            {
                playerTeamRedTwo = playerId;
            }
            else if (playerTeamRedThree == checkId)
            {
                playerTeamRedThree = playerId;
            }
        }
        else
        {
            if (playerTeamBlueOne == checkId)
            {
                playerTeamBlueOne = playerId;
            }
            else if (playerTeamBlueTwo == checkId)
            {
                playerTeamBlueTwo = playerId;
            }
            else if (playerTeamBlueThree == checkId)
            {
                playerTeamBlueThree = playerId;
            }
        }

        RequestSerialization(); // 변수 Sync 시킨 뒤에
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(UpdateUI)); // 모든 유저에게 SendEvent. 단, 
    }

    public void ErasePlayer(int playerId, int teamNow) // Remove the player's name if the player already belongs to the team
    { 
        if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }

        ChangePlayerId(playerId, -1, teamNow);
    }

    public void AddPlayer(int playerId, int teamNow) // Add the player's name if the player already not belongs to the team
    {
        if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }

        ChangePlayerId(-1, playerId, teamNow);
    }
}
