
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class VehicleController : UdonSharpBehaviour
{
    [SerializeField] VehicleData vehicleData;
    [SerializeField] private GameManagerTest gameManager;


    private void Update()
    {
        int winner = -1;
        if (CheckEnd(out winner))
        {
            UpdateEnd(winner);
            return;
        }
        if (CheckBlock())
        {
            UpdateBlock();
            return;
        }
        if (CheckEscort())
        {
            UpdateEscort();
            return;
        }

        UpdateIdle();
    }
    bool CheckEnd(out int winner)
    {
        if (Vector3.Distance(gameObject.transform.position, vehicleData.BlueTeamEndPoint.position) < 0.001)
        {
            winner = 0;
            return true;
        }
        else if (Vector3.Distance(gameObject.transform.position, vehicleData.BlueTeamEndPoint.position) < 0.001)
        {
            winner = 1;
            return true;
        }
        winner = -1;
        return false;
    }

    void UpdateEnd(int winner)
    {
        //GameManager.End(Team.Red);
        Debug.Log("State : End");
        gameManager.OnGameEnd(winner);
    }

    bool CheckBlock()
    {
        if (vehicleData.IsBlocked)
            return true;
        return false;
    }

    void UpdateBlock()
    {
        Debug.Log("State : Block");
        
    }

    bool CheckEscort()
    {
        int escortTeam = vehicleData.EscortTeam;
        if (escortTeam != -1)
        {
            return true;
        }
        return false;
    }

    void UpdateEscort()
    {
        Debug.Log("State : Escort");

        MoveVehicle();
    }

    void MoveVehicle()
    {
        int targetPoint = 0;

        if (vehicleData.EscortTeam ==1)
            targetPoint = Mathf.CeilToInt(vehicleData.CurrentPoint);
        else if (vehicleData.EscortTeam == 0)
            targetPoint = Mathf.FloorToInt(vehicleData.CurrentPoint);
        else
            Debug.Log("MoveVehicle Team None");

        transform.position = Vector3.MoveTowards(transform.position, vehicleData.pathPoints[targetPoint].position, vehicleData.moveSpeed * Time.deltaTime);
        //to do : 팀 변경시 뒤돌기 안하도록, lerp하게 회전하도록
        transform.LookAt(vehicleData.pathPoints[targetPoint].position);

        if (Vector3.Distance(transform.position, vehicleData.pathPoints[targetPoint].position) < 0.001)
            vehicleData.CurrentPoint = (vehicleData.EscortTeam == 1) ? vehicleData.CurrentPoint + 1 : vehicleData.CurrentPoint - 1;
    }

    void UpdateIdle()
    {
        Debug.Log("State : Idle");
    }
}

