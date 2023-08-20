
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnTest : UdonSharpBehaviour
{
    [SerializeField]
    GameObject go;
    [SerializeField]
    Transform location;

    public void Start()
    {
    }
    public override void Interact()
    {
        base.Interact();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "Spawn");
    }

    public void Spawn()
    {
        GameObject instacne = GameObject.Instantiate(go);
        instacne.transform.position = location.position;
    }
    
}
