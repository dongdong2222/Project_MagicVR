
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestBtn : UdonSharpBehaviour
{
    public override void Interact()
    {
        Debug.Log("Btn Active");
    }
}
