
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EscortManager : UdonSharpBehaviour
{
    public int EscortTeam { get; private set; }
    public float EscortGauge
    {
        get { return escortGauge; }
        private set
        {
            if (value >= 1)
            {
                EscortTeam = (redTeamEscortCount > blueTeamEscortCount) ? (int)Team.Red : (int)Team.Blue;
                Debug.Log($"Escort is {EscortTeam}");
                escortGauge = 1;
            }
            else if (value <= 0)
            {
                escortGauge = 0;
            }
            else
            {
                EscortTeam = (int)Team.None;
                escortGauge = value;
            }
            
        }
    }

    [SerializeField] 
    private float gaugeSpeed;
    private int redTeamEscortCount;
    private int blueTeamEscortCount;
    private float escortGauge;


    void Start()
    {
        redTeamEscortCount = 0;
        blueTeamEscortCount = 0;
        EscortTeam = (int)Team.None;
        escortGauge = 0;
    }

    private void Update()
    {
        UpdateEscortGauge();
    }

    // to do : gauge 차는 메커니즈 바꾸기
    // to do : Escort 상황 아닐때 gauge 감소시키기
    private void UpdateEscortGauge()
    {
        if (redTeamEscortCount == blueTeamEscortCount)
        {
            EscortGauge -= gaugeSpeed * Time.deltaTime;
            return;
        }

        if (redTeamEscortCount > blueTeamEscortCount)
        {
            Debug.Log("Red Team More");

            EscortGauge += gaugeSpeed * redTeamEscortCount * Time.deltaTime;
            //Debug.Log($"gauge : {escortGauge}");
        }
        else if (redTeamEscortCount < blueTeamEscortCount)
        {
            EscortGauge += gaugeSpeed * blueTeamEscortCount * Time.deltaTime;
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        base.OnPlayerCollisionEnter(player);
        Debug.Log("object Enter");
        redTeamEscortCount++;
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerCollisionExit(player);
        Debug.Log("object Exit");
        redTeamEscortCount--;
    }
}
