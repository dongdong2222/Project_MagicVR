
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
public class VehicleController : UdonSharpBehaviour
{
    public int State
    {
        get { return state; }
        private set
        {
            state = value;
            Debug.Log($"Change state : {state}");
            RequestSerialization();

        }
    }
    [SerializeField]
    private Transform redTeamEndPoint;
    [SerializeField]
    private Transform blueTeamEndPoint;
    [SerializeField]
    private EscortManager escortManager;

    [UdonSynced]
    private int state;

    void Start()
    {
        State = (int)VehicleState.Idle;
    }


    private void Update()
    {
        if (CheckEnd())
            return;
        if (CheckBlock())
            return;
        if (CheckEscort())
            return;
        State = (int)VehicleState.Idle;
    }
    bool CheckEnd()
    {
        if (Vector3.Distance(gameObject.transform.position, redTeamEndPoint.position) < 0.001 ||
            Vector3.Distance(gameObject.transform.position, blueTeamEndPoint.position) < 0.001)
        {
            State = (int)VehicleState.End;
            return true;
        }
        return false;
    }

    bool CheckBlock()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red, 1);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
        {
            //to do : block object 일때 -> 임시로 reserved2일때로
            if (hit.collider.gameObject.layer == LayerMask.GetMask("reserved2"))
            {
                State = (int)VehicleState.Block;
                return true;

            }
        }
        return false;
    }

    bool CheckEscort()
    {
        int escortTeam = escortManager.EscortTeam;
        if (escortTeam != 0)
        {
            State = (int)VehicleState.Escort;
            return true;
        }
        return false;
    }

}
