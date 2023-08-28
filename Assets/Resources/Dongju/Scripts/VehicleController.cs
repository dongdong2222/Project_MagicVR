
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class VehicleController : UdonSharpBehaviour
{
    [SerializeField]
    VehicleData vehicle;

    private void Update()
    {
        if (CheckEnd())
        {
            UpdateEnd();
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
    bool CheckEnd()
    {
        if (Vector3.Distance(gameObject.transform.position, vehicle.redTeamEndPoint.position) < 0.001 ||
            Vector3.Distance(gameObject.transform.position, vehicle.blueTeamEndPoint.position) < 0.001)
        {
            return true;
        }
        return false;
    }

    void UpdateEnd()
    {
        //GameManager.End(Team.Red);
        Debug.Log("State : End");
    }

    bool CheckBlock()
    {
        RaycastHit hit;
        //to do : ray pointer의 위치에서 발사
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red, 1);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
        {
            //to do : block object 일때 -> 임시로 reserved2일때로
            if (hit.collider.gameObject.layer == LayerMask.GetMask("Player"))
                return true;
        }

        return false;
    }

    void UpdateBlock()
    {
        Debug.Log("State : Block");
    }

    bool CheckEscort()
    {
        int escortTeam = vehicle.EscortTeam;
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

        if (vehicle.EscortTeam ==1)
            targetPoint = Mathf.CeilToInt(vehicle.CurrentPoint);
        else if (vehicle.EscortTeam == 0)
            targetPoint = Mathf.FloorToInt(vehicle.CurrentPoint);
        else
            Debug.Log("MoveVehicle Team None");

        transform.position = Vector3.MoveTowards(transform.position, vehicle.pathPoints[targetPoint].position, vehicle.moveSpeed * Time.deltaTime);
        //to do : 팀 변경시 뒤돌기 안하도록, lerp하게 회전하도록
        transform.LookAt(vehicle.pathPoints[targetPoint].position);

        if (Vector3.Distance(transform.position, vehicle.pathPoints[targetPoint].position) < 0.001)
            vehicle.CurrentPoint = (vehicle.EscortTeam == 1) ? vehicle.CurrentPoint + 1 : vehicle.CurrentPoint - 1;
    }

    void UpdateIdle()
    {
        Debug.Log("State : Idle");
    }
}

