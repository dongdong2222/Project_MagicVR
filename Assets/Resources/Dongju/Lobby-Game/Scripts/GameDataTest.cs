
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GameDataTest : UdonSharpBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerUIPrefab;
    [SerializeField] public GameObject vehicle;
    [SerializeField] private GameObject blockPrefab;

    [SerializeField] private GameObject[] UIList;

    [SerializeField] private Transform blueTeamStartPoint;
    public Transform BlueTeamStartPoint { get { return blueTeamStartPoint; } }
    [SerializeField] private Transform redTeamStartPoint;
    public Transform RedTeamStartPoint { get { return redTeamStartPoint; } }

    [SerializeField] private Transform winnerTeamEndPoint;
    public Transform WinnerTeamEndPoint { get { return winnerTeamEndPoint; } }
    [SerializeField] private Transform loserTeamEndPoint;
    public Transform LoserTeamEndPoint { get { return loserTeamEndPoint; } }


    [UdonSynced] private int[] blueTeamIDs;
    [UdonSynced] private int[] redTeamIDs;

    [UdonSynced]private int blueTeamIDs_size;
    public int BlueTeamIDs_size { get { return blueTeamIDs_size; } }
    [UdonSynced]private int redTeamIDs_size;
    public int RedTeamIDs_size { get { return redTeamIDs_size; } }
    [UdonSynced] private int winnerTeam = -1;
    public int WinnerTeam { 
        get { return winnerTeam; } 
        set
        {
            winnerTeam = value;
            RequestSerialization();

            OnDeserialization();
        }
    }


    private void Start()
    {
        blueTeamIDs = new int[5] {-1, -1, -1, -1, -1}; //to do with 끼잉: 그냥 왕창 늘릴까?
        redTeamIDs = new int[5] {-1, -1, -1, -1, -1};
        blueTeamIDs_size = 0;
        redTeamIDs_size = 0;
    }

    public int GetPlayerID(int team, int index)
    {
        if (team == 0)
        {
            if (index >= blueTeamIDs_size)
                return -1;
            return blueTeamIDs[index];
        }
        else if (team == 1)
        {
            if (index >= redTeamIDs_size)
                return -1;
            return redTeamIDs[index];
        }
        else
            return -1;
    }

    public int GetPlayerTeam(int id)
    {
        for(int i=0; i< blueTeamIDs.Length; i++)
        {
            if (blueTeamIDs[i] == id)
                return 0;
        }
        for (int i = 0; i < redTeamIDs.Length; i++)
        {
            if (redTeamIDs[i] == id)
                return 1;
        }
        return -1;
    }

    public void AddPlayer(int team, int id)
    {
        if (team == 0)
        {
            if (blueTeamIDs_size >= 4)
                return;
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }
            blueTeamIDs[blueTeamIDs_size] = id;
            blueTeamIDs_size++;
            RequestSerialization();
        }
        else if (team == 1)
        {
            if (redTeamIDs_size >= 4)
                return;
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }
            redTeamIDs[redTeamIDs_size] = id;
            redTeamIDs_size++;
            RequestSerialization();
        }
        else
            Debug.Log("Wrong Team info");

        OnDeserialization();
    }

    public void ReMovePlayer(int team, int id)
    {
        if (team == 0)
        {
            int index = Array.IndexOf<int>(blueTeamIDs, id);
            //Debug.Log($"index is {index}");
            if (index == -1)
                return;
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }
            for (int i = index; i < blueTeamIDs_size; i++)
            {
                blueTeamIDs[index] = blueTeamIDs[index + 1];
            }
            blueTeamIDs_size--;
            RequestSerialization();
        }
        else if (team == 1)
        {
            int index = Array.IndexOf<int>(redTeamIDs, id);
            //Debug.Log($"index is {index}");
            if (index == -1)
                return;
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }
            for (int i = index; i < redTeamIDs_size; i++)
            {
                redTeamIDs[index] = redTeamIDs[index + 1];
            }
            redTeamIDs_size--;
            RequestSerialization();
        }
        else
            Debug.Log("Wrong Team info");

        OnDeserialization();
    }

    public override void OnDeserialization()
    {
        //to do with 끼잉 : UI Update, ui별 component를 알고 있어야 한다...
        UIList[0].GetComponent<LobbyUITest>().UpdateUI();
    }
}
