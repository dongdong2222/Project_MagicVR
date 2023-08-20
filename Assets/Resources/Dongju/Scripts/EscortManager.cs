
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EscortManager : UdonSharpBehaviour
{
    [SerializeField]
    VehicleData vehicle;

    //vehicle 초기 위치 (0,0,0)에서 변경해보기!!
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        base.OnPlayerCollisionEnter(player);
        Debug.Log("object Enter");
        vehicle.AddEscortCount((int)Team.Red);
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerCollisionExit(player);
        Debug.Log("object Exit");
        vehicle.SubEscortCount((int)Team.Red);
    }
}
