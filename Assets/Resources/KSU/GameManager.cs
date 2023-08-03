
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameManager : UdonSharpBehaviour
{
    private GameObject vehicle;
    //get player api or id(int) and divide it into two teams
    public int[] playerIds;
    public int[] redPlayerIds;
    public int[] bluePlayerIds;
    //red = -1 or blue = 1
    public int winnerTeam = 0;
    public GameObject playerObject;
    public GameObject gameUI;
    public Vector3 winnerResultMap;
    public Vector3 loserResultMap;
    //-1:before game, 0:during game, 1:after game
    private int gameState;

    public void EndGame(int winnerTeam) {
        this.winnerTeam = winnerTeam;
        RemoveVehicle();
        RemovePlayer();
        RemoveUI();
        RemoveScroll();
        MoveToResultMap();
    }

    public void RemoveVehicle() {
        Destroy(vehicle);
    }
    public void RemovePlayer() {
        //for(int i=0; i<playerObjects.Length; i++) {
        Destroy(playerObject);
        //}
    }
    public void RemoveUI() {
        //Timer도 같이 관리
        Destroy(gameUI);
    }
    public void RemoveScroll() {
        //Scroll Object or Class destroy
    }
    public void MoveToResultMap() {
        VRCPlayerApi player = Networking.LocalPlayer;
        foreach (int playerId in redPlayerIds) {
            if(player.playerId == playerId) {
                if(winnerTeam == -1) {
                    player.TeleportTo(winnerResultMap, player.GetRotation());
                }
                else {
                    player.TeleportTo(loserResultMap, player.GetRotation());
                }
            }
        }
        foreach (int playerId in bluePlayerIds) {
            if(player.playerId == playerId) {
                if(winnerTeam == 1) {
                    player.TeleportTo(winnerResultMap, player.GetRotation());
                }
                else {
                    player.TeleportTo(loserResultMap, player.GetRotation());
                }
            }
        }
    }
}
