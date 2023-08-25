
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LobbyUITest : UdonSharpBehaviour
{
    [SerializeField] private GameDataTest gameData;

    [SerializeField] private TextMeshProUGUI[] blueTeamTexts;
    [SerializeField] private TextMeshProUGUI[] redTeamTexts;

    private void Update()
    {
        Debug.Log("UI update");
    }
    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        Debug.Log("UpdateUI");
        for(int i=0; i< blueTeamTexts.Length; i++)
        {
            if (gameData.GetPlayerID(0, i) != -1)
                blueTeamTexts[i].text = "" + VRCPlayerApi.GetPlayerById(gameData.GetPlayerID(0, i)).displayName; //to do : name 출력으로 바꾸기
            else
                blueTeamTexts[i].text = "-";
        }
        for(int i=0; i< redTeamTexts.Length; i++)
        {
            if (gameData.GetPlayerID(1, i) != -1)
                redTeamTexts[i].text = "" + VRCPlayerApi.GetPlayerById(gameData.GetPlayerID(1, i)).displayName;
            else
                redTeamTexts[i].text = "-";
        }
    }
}
