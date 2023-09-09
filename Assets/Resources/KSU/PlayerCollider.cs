
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerCollider : UdonSharpBehaviour
{
    public VRCPlayerApi player;
    [UdonSynced] public int playerId;
    public Text test;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        SetCollider();
    }

    public void SetCollider() 
    {
        if(player != null) {
            Vector3 position = player.GetPosition();
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(position.x, player.GetBonePosition(HumanBodyBones.Head).y/2.0f * 1.2f, position.z);
            gameObject.GetComponent<CapsuleCollider>().height = player.GetBonePosition(HumanBodyBones.Head).y * 1.2f;
            test.text = gameObject.GetComponent<CapsuleCollider>().bounds.center.ToString();
        }
    }

    public void SetPlayer(int playerId)
    {
        this.playerId = playerId;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(GetPlayer));
    }

    public void GetPlayer()
    {
        this.player = VRCPlayerApi.GetPlayerById(playerId);
    }
}
