
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class BlockManager : UdonSharpBehaviour
{
    [SerializeField] VehicleData vehicleData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("object Block");
        // to do : block object와 충돌 시로 바꾸기, isBlocking 개수에 문제 있을 수도
        //if (other.gameObject.layer == LayerMask.GetMask("Default"))
        //{
        //    Debug.Log("object Block");

        //    vehicleData.isBlocked++;
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("object UnBlock");
        // to do : block object와 충돌 시로 바꾸기
        //if (other.gameObject.layer == LayerMask.GetMask("Default"))
        //{
        //    Debug.Log("object UnBlock");

        //    vehicleData.isBlocked--;
        //}
    }
}
