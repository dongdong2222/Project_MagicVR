
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class GameUI : UdonSharpBehaviour
{
    [SerializeField]
    private VehicleData vehicle;

    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private Text redTeamText;
    [SerializeField]
    private Text blueTeamText;

    void Start()
    {
        progressBar.value = 0.5f;
        redTeamText.text = "" + 10;
        blueTeamText.text = "" + 11;
    }

    private void Update()// sync 된 정보만 들고오면 같은 정보보기 간으!
    {
        progressBar.value = vehicle.GetProgress();
        redTeamText.text = "Red : " + vehicle.RedTeamEscortCount;
        blueTeamText.text = "Blue : " + vehicle.BlueTeamEscortCount;
    }

}
