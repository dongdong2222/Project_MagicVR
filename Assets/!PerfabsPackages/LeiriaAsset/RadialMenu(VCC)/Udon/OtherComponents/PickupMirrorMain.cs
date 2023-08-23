
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PickupMirrorMain : UdonSharpBehaviour
{
    [Header("PickupMirrorの追従")]
    [Space]
    [Header("PickupMirrorMainの子のPickupMirrorを入れる")]
    [SerializeField] GameObject PickupMirror;
    [Header("PickupMirrorのDefault位置(LocalPosition)を設定")]
    [SerializeField] Vector3 defaultPosition = new Vector3(0, 1, 0.5f);

    void OnEnable()
    {
        var player = Networking.LocalPlayer;
        gameObject.transform.position = player.GetPosition();
        gameObject.transform.rotation = player.GetRotation();
        PickupMirror.transform.localPosition = defaultPosition;
        PickupMirror.transform.localRotation = Quaternion.identity;
    }
    
}
