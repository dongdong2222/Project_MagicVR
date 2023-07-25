
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class toggleMusic : UdonSharpBehaviour
{
    private void FunctionToggle(bool _bool)
    {
        Debug.Log(_bool);
    }
}
