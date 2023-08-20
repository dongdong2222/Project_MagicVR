
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnObject : UdonSharpBehaviour
{
    public override void Interact()
    {
        base.Interact();
        if (Networking.IsOwner(this.gameObject))
        {
            this.gameObject.SetActive(false);
        }
    }
}
