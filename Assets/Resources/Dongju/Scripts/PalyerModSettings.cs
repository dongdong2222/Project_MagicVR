
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PalyerModSettings : UdonSharpBehaviour
{
    VRCPlayerApi player;

    [Header("Player Settings")]
    [SerializeField] float jumpImpulse = 3;
    [SerializeField] float walkSpeed = 3;
    [SerializeField] float runSpeed = 3;
    [SerializeField] float gravityStrengh = 1;
    void Start()
    {
        player = Networking.LocalPlayer;
        player.SetJumpImpulse(jumpImpulse);
        player.SetWalkSpeed(walkSpeed);
        player.SetRunSpeed(runSpeed);
        player.SetGravityStrength(gravityStrengh);

    }
}
