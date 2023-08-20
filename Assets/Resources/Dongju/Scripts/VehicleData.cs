
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

enum VehicleState
{
    Idle,
    Escort,
    Block,
    End,
}

enum Team
{
    None,
    Red,
    Blue,
}
public class VehicleData : UdonSharpBehaviour
{

    //Point : 위치
    [SerializeField] public Transform redTeamEndPoint;
    [SerializeField] public Transform blueTeamEndPoint;
    [SerializeField] public Transform[] pathPoints;
    [SerializeField] public int moveSpeed;


    // public method로
    //public int State; //sync. property
    public int EscortTeam { get; private set; } //
    public int RedTeamEscortCount { get; private set; } //UI sync
    public int BlueTeamEscortCount { get; private set; } //UI sync
    public float CurrentPoint { get; set; }

    private int redTeamEscortCount;
    private int blueTeamEscortCount;
    private int escortTeam;


    private float[] pathDistances;
    private float totalDistance;

    private void Start()
    {
        pathDistances = new float[pathPoints.Length-1];
        totalDistance = 0f;
        for(int i=0; i < pathPoints.Length-1; i++)
        {
            pathDistances[i] = Vector3.Distance(pathPoints[i].position, pathPoints[i + 1].position);
            totalDistance += pathDistances[i];
        }

        CurrentPoint = 2.5f;
    }

    //sync
    //transform -> sync 필요
    //UI에 띄울 정보 -> sync 필요

    public void AddEscortCount(int team)
    {
        //to do? : 최대 team 수 넘어가지 않도록
        if (team == (int)Team.Red)
            RedTeamEscortCount++;
        else
            BlueTeamEscortCount++;

        SetEscortTeam();
    }

    public void SubEscortCount(int team)
    {
        //to do? : -로 떨어지지 않도록
        if (team == (int)Team.Red)
            RedTeamEscortCount--;
        else
            BlueTeamEscortCount--;
        
        SetEscortTeam();
    }

    private void SetEscortTeam()
    {
        //to do : add sub 할때 team 변경 계산
        if (RedTeamEscortCount > BlueTeamEscortCount)
            EscortTeam = (int)Team.Red;
        else if (BlueTeamEscortCount > RedTeamEscortCount)
            EscortTeam = (int)Team.Blue;
        else
            EscortTeam = (int)Team.None;

    }


    public float GetProgress()
    {
        float progress = GetCurrentDistance() / totalDistance;
        Debug.Log($"progress : {progress}");
        return GetCurrentDistance() / totalDistance;
    }

    private float GetCurrentDistance()
    {
        float distance = 0.0f;
        for(int i=0; i < Mathf.FloorToInt(CurrentPoint); i++)
            distance += pathDistances[i];
        distance += Vector3.Distance(gameObject.transform.position, pathPoints[Mathf.FloorToInt(CurrentPoint)].position);
        return distance;
    }

}
