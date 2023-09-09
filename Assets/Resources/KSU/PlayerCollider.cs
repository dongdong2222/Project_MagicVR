
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerCollider : UdonSharpBehaviour
{
    //collider 포지션 찍으면 이상하게 찍힘
    public VRCPlayerApi player;
    [UdonSynced, FieldChangeCallback(nameof(playerId))] private int _playerId;
    public int playerId
    {
        set
        {
            _playerId = value; 
            GetPlayer(value);
        }
        get => _playerId;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        SetCollider();
    }

    public void SetCollider() 
    {
        if(player != null && Networking.LocalPlayer == player && Networking.LocalPlayer.IsOwner(gameObject)) {
            Vector3 position = player.GetPosition();
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(position.x, player.GetBonePosition(HumanBodyBones.Head).y/2.0f * 1.2f, position.z);
            gameObject.GetComponent<CapsuleCollider>().height = player.GetBonePosition(HumanBodyBones.Head).y * 1.2f;
            Debug.Log(gameObject.GetComponent<CapsuleCollider>().bounds.center.ToString());
        }
    }

    public void SetPlayer(int value)
    {
        playerId = value;
        RequestSerialization();
        //fieldChangeCallback에서 getplayer바꾸는걸로 수정
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(GetPlayer));
    }

    public void GetPlayer(int value)
    {
        player = VRCPlayerApi.GetPlayerById(value);
    }
}
