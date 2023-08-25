
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SafeBoundary : UdonSharpBehaviour
{
    public void Open()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(OpenEvent));
    }

    public void Close()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(CloseEvent));
    }

    public void OpenEvent()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    public void CloseEvent()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);

    }
}
