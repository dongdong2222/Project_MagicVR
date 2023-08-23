
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using UnityEngine.UI;
using Unity.Collections;
using VRC.Udon;
using VRC.Udon.Common;
public class ForPlayerTest : UdonSharpBehaviour
{
    public GameObject playerPrefab;
    public PlayerSetting playerManager;
    public GameObject playerManagerTest;
    
    //게임 시작시 한번 실행되어야 함
    public override void OnPickupUseDown()
    {
        playerManagerTest = GameObject.Find("PlayerManagerTest");
        Text testText = playerManagerTest.GetComponent<Text>();
        testText.text = "";
        testText.text = string.Concat(testText.text," pickup\n");
        VRCPlayerApi[] players = new VRCPlayerApi[30];
        VRCPlayerApi.GetPlayers(players);
        int i = 0;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerSetting>();
        foreach(VRCPlayerApi player in players) {
            if(player == null) {
                testText.text = string.Concat(testText.text, "null\n");
                continue;
            }
            else {
                GameObject clientPlayerObject = VRCInstantiate(playerPrefab);
                clientPlayerObject.SetActive(true);
                clientPlayerObject.name = string.Concat("playerPrefab", i);
                playerManager.players[i] = GameObject.Find(string.Concat("playerPrefab", i)).GetComponent<Player>();
                playerManager.players[i].stat = clientPlayerObject.GetComponent<PlayerStat>();
                playerManager.players[i].stat.Initialize();
                playerManager.players[i].player = player;
                Networking.SetOwner(Networking.LocalPlayer, clientPlayerObject);
                testText.text = string.Concat(testText.text , "playerPrefab" , i , ": " , 
                                            playerManager.players[i].player.displayName , ", HP :" , 
                                            playerManager.players[i].stat.GetHp() , "\n");
            }
            i++;
            i %= 30;
            Debug.Log(player.displayName);
        }
    }
}
