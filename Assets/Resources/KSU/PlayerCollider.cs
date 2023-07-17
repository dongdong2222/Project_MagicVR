
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerCollider : UdonSharpBehaviour
{

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        SetCollider();
    }

    public void SetCollider() 
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        Vector3 position = localPlayer.GetPosition();
        if(localPlayer != null) {
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(position.x, localPlayer.GetBonePosition(HumanBodyBones.Head).y/2.0f * 1.2f, position.z);
            gameObject.GetComponent<CapsuleCollider>().height = localPlayer.GetBonePosition(HumanBodyBones.Head).y * 1.2f;
        }
    }
}
