
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class toggleMusic : UdonSharpBehaviour
{
    public UnityEngine.UI.Toggle musicToggle;

    private void Start()
    {
        //musicToggle.onValueChanged.AddListener(FunctionToggle);
    }

    private void FunctionToggle(bool _bool)
    {
        Debug.Log(_bool);
    }
}
