
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
public class UIForDebug : UdonSharpBehaviour
{
    [SerializeField]
    private EscortManager escortManager;

    [SerializeField]
    private Text escortGaugeText;
    [SerializeField]
    private Text masterCheckText;

    private VRCPlayerApi player;
    private void Start()
    {
        player = Networking.LocalPlayer;
    }

    void LateUpdate()
    {
        UpdateText();
    }

    void UpdateText()
    {
        escortGaugeText.text = "" + escortManager.EscortGauge;
        masterCheckText.text = "" + player.isMaster;
    }
}
