
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class EscortManager : UdonSharpBehaviour
{
    [SerializeField]
    VehicleData vehicleData;

    //vehicle 초기 위치 (0,0,0)에서 변경해보기!!
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        base.OnPlayerCollisionEnter(player);
        Debug.Log("object Enter");
        vehicleData.AddEscortCount(1);
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerCollisionExit(player);
        Debug.Log("object Exit");
        vehicleData.SubEscortCount(1);
    }


}
