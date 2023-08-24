
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RedTeamButton : UdonSharpBehaviour
{
    [SerializeField] private GameDataTest gameData;

    public override void Interact()
    {
        if (gameData.GetPlayerTeam(Networking.LocalPlayer.playerId) != 1)
        {
            gameData.AddPlayer(1, Networking.LocalPlayer.playerId);
            gameData.ReMovePlayer(0, Networking.LocalPlayer.playerId);
        }
    }
}
