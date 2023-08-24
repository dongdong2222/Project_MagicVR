
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SafeBoundary : UdonSharpBehaviour
{
    public void Open()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y+5, transform.position.z);
    }

    public void Close()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y-5, transform.position.z);
    }
}
