
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ForSyncTest : UdonSharpBehaviour
{
    
    private VRCPlayerApi player;
    [SerializeField] GameObject child;
    void Start()
    {
        
    }
    void Update()
    {
        Vector3 position = player.GetPosition();
        child.transform.position = new Vector3(position.x, player.GetBonePosition(HumanBodyBones.Head).y * 1.2f, position.z);
    }
    public void SetPlayer(VRCPlayerApi player)
    {
        if (!Networking.LocalPlayer.IsOwner(gameObject)) { Networking.SetOwner(player, gameObject); }
        this.player = player;
    }
}
