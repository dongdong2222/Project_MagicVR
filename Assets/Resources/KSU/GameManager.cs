
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameManager : UdonSharpBehaviour
{
    private GameObject vehicle;
    //get player api or id(int) and divide it into two teams
    public VRCPlayerApi[] players;
    public VRCPlayerApi[] redPlayers;
    public VRCPlayerApi[] bluePlayers;
    //red = -1 or blue = 1
    public int winner = 0;
    public GameObject[] playerObjects;
    public GameObject gameUI;
    public Vector3 winnerResultMap;
    public Vector3 loserResultMap;
    //-1:before game, 0:during game, 1:after game
    private int gameState;

    public void EndGame(int winnerTeam) {
        //dsfsdfsdf
        winner = winnerTeam;
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
        for(int i=0; i<playerObjects.Length; i++) {
            Destroy(playerObjects[i]);
        }
    }
    public void RemoveUI() {
        Destroy(gameUI);
    }
    public void RemoveScroll() {
        //Scroll Object or Class destroy
    }
    public void MoveToResultMap() {
        foreach(VRCPlayerApi player in redPlayers){
            if(winner == -1) {
                player.TeleportTo(winnerResultMap, player.GetRotation());
            }
            else {
                player.TeleportTo(loserResultMap, player.GetRotation());
            }
        }
        foreach(VRCPlayerApi player in bluePlayers){
            if(winner == 1) {
                player.TeleportTo(winnerResultMap, player.GetRotation());
            }
            else {
                player.TeleportTo(loserResultMap, player.GetRotation());
            }
        }
    }
}
