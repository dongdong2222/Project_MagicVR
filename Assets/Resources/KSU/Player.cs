
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Player : UdonSharpBehaviour
{
    public PlayerStat stat;
    public PlayerCollider playerCollider;
    public VRCPlayerApi player;
    [UdonSynced] public int playerId;
    void Start()
    {
        
    }

    public void SetPlayer(VRCPlayerApi player)
    {
        this.playerId = player.playerId;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(GetPlayer));
    }

    public void GetPlayer()
    {
        this.player = VRCPlayerApi.GetPlayerById(playerId);
    }
}
