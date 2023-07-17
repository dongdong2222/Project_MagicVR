
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
public class ForPlayerTest : UdonSharpBehaviour
{
    public GameObject statPrefab;
    public GameObject colliderPrefab;
    public PlayerSetting playerManager;
    public void Start()
    {
        VRCPlayerApi[] players = new VRCPlayerApi[30];
        VRCPlayerApi.GetPlayers(players);
        //int i = 0;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerSetting>();
        /*foreach(VRCPlayerApi player in players) {
            if(player == null) continue;
            else {
                if(playerManager.players[i].player == null) {*/
                    GameObject clientStatObject = VRCInstantiate(statPrefab);
                    clientStatObject.SetActive(true);
                    clientStatObject.name = "Stat";
                    playerManager.players[0] = GameObject.Find("Stat").GetComponent<Player>();
                    playerManager.players[0].stat = clientStatObject.GetComponent<PlayerStat>();
                    playerManager.players[0].player = Networking.LocalPlayer;
                    Networking.SetOwner(Networking.LocalPlayer, clientStatObject);
                /*}
            }
            i++;
            i %= 30;
            Debug.Log(player.displayName);
        }*/
        GameObject colliderObject = VRCInstantiate(colliderPrefab);
        colliderObject.SetActive(true);
        colliderObject.name = "Collider";
        Networking.SetOwner(Networking.LocalPlayer, colliderObject);

    }
}
