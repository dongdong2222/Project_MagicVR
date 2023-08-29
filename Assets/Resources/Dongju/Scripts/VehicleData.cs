
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//Team
//Blue: 0  Red: 1  None: -1

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class VehicleData : UdonSharpBehaviour
{
    //Point : 위치
    [SerializeField] public Transform blueTeamEndPoint;
    [SerializeField] public Transform redTeamEndPoint;
    [SerializeField] public Transform[] pathPoints;
    [SerializeField] public int moveSpeed;


    // public method로
    //public int State; //sync. property
    public int EscortTeam { get { return escortTeam; } } //
    public int RedTeamEscortCount { get { return redTeamEscortCount; } } //UI sync
    public int BlueTeamEscortCount { get { return blueTeamEscortCount; } } //UI sync
    public Transform BlueTeamEndPoint { get { return blueTeamEndPoint; } }
    public Transform RedTeamEndPoint { get { return redTeamEndPoint; } }
    public float CurrentPoint { get; set; }
    public int isBlocked { get; set; } = 0;

    [UdonSynced] private int redTeamEscortCount = 0;
    [UdonSynced] private int blueTeamEscortCount = 0;
    private int escortTeam = -1;

    private float[] p2pDistances;
    private float totalDistance;

    private void Start()
    {
        p2pDistances = new float[pathPoints.Length-1];
        totalDistance = 0f;
        for(int i=0; i < pathPoints.Length-1; i++)
        {
            p2pDistances[i] = Vector3.Distance(pathPoints[i].position, pathPoints[i + 1].position);
            totalDistance += p2pDistances[i];
        }
        //to do : path의 정가운데로 init하기
        transform.position = GameObject.Find("Path").transform.position;
        CurrentPoint = 2.5f;
    }

    //sync
    //transform -> sync 필요
    //UI에 띄울 정보 -> sync 필요

    public void AddEscortCount(int team)
    {
        //to do? : 최대 team 수 넘어가지 않도록
        if (team == 1)
            redTeamEscortCount++;
        else
            blueTeamEscortCount++;

        SetEscortTeam();
    }

    public void SubEscortCount(int team)
    {
        //to do? : -로 떨어지지 않도록
        if (team == 1)
            redTeamEscortCount--;
        else
            blueTeamEscortCount--;
        
        SetEscortTeam();
    }

    private void SetEscortTeam()
    {
        //to do : add sub 할때 team 변경 계산
        if (RedTeamEscortCount > BlueTeamEscortCount)
            escortTeam = 1;
        else if (BlueTeamEscortCount > RedTeamEscortCount)
            escortTeam = 0;
        else
            escortTeam = -1;

    }


    public float GetVehicleProgress()
    {
        float progress = GetCurrentDistance() / totalDistance;
        //Debug.Log($"progress : {progress}");
        return GetCurrentDistance() / totalDistance;
    }

    private float GetCurrentDistance()
    {
        float distance = 0.0f;
        for(int i=0; i < Mathf.FloorToInt(CurrentPoint); i++)
            distance += p2pDistances[i];
        distance += Vector3.Distance(gameObject.transform.position, pathPoints[Mathf.FloorToInt(CurrentPoint)].position);
        return distance;
    }

}
