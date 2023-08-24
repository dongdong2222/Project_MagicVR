
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

enum GameState
{
    Match,
    Start,
    Ready,
    Play,
    End,
}

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GameManagerDong : UdonSharpBehaviour
{

    int[] redTeamPlayerIDs;
    int[] blueTeamPlayerIDs;

    [SerializeField]
    GameObject playerPrefab; //게임에 필요한 collider object, 

    [SerializeField]
    LobbyManager lobbyManager;

    [SerializeField]
    GameObject gameUI;

    //[SerializeField]
    //Timer timer;

    //red = -1 or blue = 1
    //-1:before game, 0:during game, 1:after game

    public Vector3 winnerResultMap;
    public Vector3 loserResultMap;

    public GameObject vehicle;
    public int winnerTeam = 0;

    private float waitTime = 5;
    private float timer = 0;

    void Start()
    {

        //PlayerStat = Resources.Load<GameObject>("Prefabs/");
    }

    //owner에게만 이벤트 전달된다고 생각
    public void OnGameStart() //lobbyManager가 SendCustomNetworkEvent(OnGameStart);
    {
        Debug.Log("OnGameStart");
        //RequestSerialization(); // 변수 Sync 시킨 뒤에
        //InitPlayer();
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InitTeam");
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InitUI");
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InitPlayer");
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InitObject");
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "OnGameReady");
        //OnGameReady();

    }
    //public void OnGameReady() //시작 위치로, 모두 load 후 n초 뒤 시작
    //{
    //    Debug.Log("OnGameReady");
    //    //timer += Time.deltaTime;
    //    if (timer > waitTime)
    //    {
    //        //OnGamePlay();
    //    }
    //}
    //public void OnGamePlay()
    //{
    //    Debug.Log("OnGamePlay");

    //}
    //public void OnGameEnd()
    //{

    //}

    //void InitTeam()
    //{
    //    blueTeamPlayerIDs = new int[3];
    //    redTeamPlayerIDs = new int[3];

    //    blueTeamPlayerIDs[0] = lobbyManager.playerTeamBlueOne;
    //    blueTeamPlayerIDs[1] = lobbyManager.playerTeamBlueTwo;
    //    blueTeamPlayerIDs[2] = lobbyManager.playerTeamBlueThree;
    //    redTeamPlayerIDs[0] = lobbyManager.playerTeamRedOne;
    //    redTeamPlayerIDs[1] = lobbyManager.playerTeamRedTwo;
    //    redTeamPlayerIDs[2] = lobbyManager.playerTeamRedThree;
    //}
    //void InitPlayer()
    //{
    //    for (int i = 0; i < blueTeamPlayerIDs.Length; i++)
    //    {
    //        GameObject.Instantiate(playerPrefab, VRCPlayerApi.GetPlayerById(blueTeamPlayerIDs[0]).gameObject.transform);
    //    }
    //    for (int i = 0; i < redTeamPlayerIDs.Length; i++)
    //    {
    //        GameObject.Instantiate(playerPrefab, VRCPlayerApi.GetPlayerById(redTeamPlayerIDs[0]).gameObject.transform);
    //    }

    //}
    //void InitUI()
    //{
    //    //to do : 화면 전체 UI 아니면 플레이어에 붙어있는 UI로 모든 정보 표시? -> OO
    //    // 진행 시간, 화물 진행 바, 점령 진행도
    //    GameObject.Instantiate(gameUI);
    //}
    //void InitObject()
    //{
    //    //to do : 화물, 화물 장애물, 
    //    //vehicle.GetComponent<VehicleController>().Init();
    //}

    //public void EndGame(int winnerTeam)
    //{
    //    this.winnerTeam = winnerTeam;
    //    RemoveVehicle();
    //    RemovePlayer();
    //    RemoveUI();
    //    RemoveScroll();
    //    MoveToResultMap();
    //}

    //public void RemoveVehicle()
    //{
    //    Destroy(vehicle);
    //}
    //public void RemovePlayer()
    //{
    //    //for(int i=0; i<playerObjects.Length; i++) {
    //    Destroy(playerPrefab);
    //    //}
    //}
    //public void RemoveUI()
    //{
    //    //Timer도 같이 관리
    //    Destroy(gameUI);
    //}
    //public void RemoveScroll()
    //{
    //    //Scroll Object or Class destroy
    //}
    //public void MoveToResultMap()
    //{
    //    VRCPlayerApi player = Networking.LocalPlayer;
    //    foreach (int playerId in redTeamPlayerIDs)
    //    {
    //        if (player.playerId == playerId)
    //        {
    //            if (winnerTeam == -1)
    //            {
    //                player.TeleportTo(winnerResultMap, player.GetRotation());
    //            }
    //            else
    //            {
    //                player.TeleportTo(loserResultMap, player.GetRotation());
    //            }
    //        }
    //    }
    //    foreach (int playerId in blueTeamPlayerIDs)
    //    {
    //        if (player.playerId == playerId)
    //        {
    //            if (winnerTeam == 1)
    //            {
    //                player.TeleportTo(winnerResultMap, player.GetRotation());
    //            }
    //            else
    //            {
    //                player.TeleportTo(loserResultMap, player.GetRotation());
    //            }
    //        }
    //    }
    //}
}
