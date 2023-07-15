
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class VehicleUpdater : UdonSharpBehaviour
{
    [SerializeField]
    VehicleController controller;
    [SerializeField]
    EscortManager escortManager;
    [SerializeField]
    private int speed;
    [SerializeField]
    private Transform[] anchors;
    //private List<Transform> anchors = new List<Transform>();

    private float currentPointIndex;

    void Start()
    {
        //to do : currentPoint 가운데로
        currentPointIndex = 2.5f;
    }

    void LateUpdate()
    {
        Debug.Log($"controller State : {controller.State}");
        switch (controller.State)
        {
            case (int)VehicleState.Idle:
                UpdateIdle();
                break;
            case (int)VehicleState.Escort:
                UpdateEscort();
                break;
            case (int)VehicleState.Block:
                UpdateBlock();
                break;
            case (int)VehicleState.End:
                UpdateEnd();
                break;
        }
    }

    void UpdateIdle()
    {
        Debug.Log("State : Idle");
    }

    //description : RedEnd=lastPoint, BlueEnd=startPoint
    void UpdateEscort()
    {
        Debug.Log("State : Escort");
        int targetIndex = 0;
        if (escortManager.EscortTeam == (int)Team.Red)
            targetIndex = Mathf.CeilToInt(currentPointIndex);
        else
            targetIndex = Mathf.FloorToInt(currentPointIndex);

        MoveToAnchor(targetIndex);

        if (Vector3.Distance(transform.position, anchors[targetIndex].position) < 0.001)
            currentPointIndex = (escortManager.EscortTeam == (int)Team.Red) ? currentPointIndex + 1 : currentPointIndex - 1;
    }

    void MoveToAnchor(int index)
    {
        transform.position = Vector3.MoveTowards(transform.position, anchors[index].position, speed * Time.deltaTime);
        transform.LookAt(anchors[index].position);
    }

    void UpdateBlock()
    {
        Debug.Log("State : Block");
    }

    void UpdateEnd()
    {
        Debug.Log("State : End");
    }
}
