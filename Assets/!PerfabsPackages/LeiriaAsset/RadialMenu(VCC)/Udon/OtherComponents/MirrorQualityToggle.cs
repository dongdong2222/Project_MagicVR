
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class MirrorQualityToggle : UdonSharpBehaviour
{
    [Header("PickupMirrorのLQ・HQの切り替え")]
    [Space]
    [SerializeField] GameObject MirrorLQ;
    [SerializeField] GameObject MirrorHQ;
    [SerializeField] GameObject SelectLQ;
    [SerializeField] GameObject SelectHQ;

    public override void OnPickupUseDown()
    {
        MirrorLQ.SetActive(!MirrorLQ.activeSelf);
        MirrorHQ.SetActive(!MirrorHQ.activeSelf);
        SelectLQ.SetActive(!SelectLQ.activeSelf);
        SelectHQ.SetActive(!SelectHQ.activeSelf);
    }
}
