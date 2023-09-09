
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WheelTest : UdonSharpBehaviour
{
    [SerializeField] private Animator[] wheels;

    private bool test = false;
    public override void Interact()
    {
        if (!Networking.IsOwner(gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject); }

        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(WheelPlay));
    }

    public void WheelPlay()
    {
        Debug.Log("zz");
        test = !test;

        for (int i=0; i <wheels.Length; i++)
        {
            wheels[i].SetBool("Go", test);
        }
    }
}
