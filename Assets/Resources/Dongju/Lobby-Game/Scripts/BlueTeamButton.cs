
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BlueTeamButton : UdonSharpBehaviour //to do : 그냥 하나로 합칠까?? BtnSelectTeam으로
{
    [SerializeField] private GameDataTest gameData;

    public override void Interact()
    {
        if (gameData.GetPlayerTeam(Networking.LocalPlayer.playerId) != 0)
        {
            gameData.AddPlayer(0, Networking.LocalPlayer.playerId);
            gameData.ReMovePlayer(1, Networking.LocalPlayer.playerId);

        }
    }
}
