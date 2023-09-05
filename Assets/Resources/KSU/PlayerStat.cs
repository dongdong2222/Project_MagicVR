using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using static VRC.SDKBase.Networking;
using static VRC.SDKBase.VRCPlayerApi;
using VRC.Udon;
using UdonSharp;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
//Utilities.isValid 쓸것
public class PlayerStat : UdonSharpBehaviour
{
    /*
        Variables for Player
    */
    public VRCPlayerApi player;
    [UdonSynced] public int playerId;
    
    [SerializeField] [UdonSynced] private float hp;
    [SerializeField] private Text hpText;
    private float originHp = 100;
    [SerializeField] private float mp;
    [SerializeField] private int myTeam;
    
    private int[] OffenseBuffIds = {};
    private int[] DefenseBuffIds = {};

    // Start is called before the first frame update
    void Start()
    {  
        
    }

    public void Initialize() {
        if(!Networking.LocalPlayer.IsOwner(gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject);};
        hp = originHp;
        //SetOwner를 켰을때 한명만 분배되는
        RequestSerialization();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Networking.LocalPlayer.IsOwner(gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject);};
        if(Networking.LocalPlayer.playerId == playerId) hpText.text = hp.ToString();
    }

    private void GetDamage(float damage) 
    {
        if(!Networking.LocalPlayer.IsOwner(gameObject)) { Networking.SetOwner(Networking.LocalPlayer, gameObject);}
        if(hp > damage && Networking.LocalPlayer == player) {
            hp -= damage;
            RequestSerialization();
        }
        else {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(Respawn));
        }
    }

    public float MagicCast(float magicDamage)
    {
        float tempDamage = magicDamage;
        OffenseBuffs buff = gameObject.GetComponent<OffenseBuffs>();
        //Buff effect calculate
        foreach (int buffId in OffenseBuffIds) {
            tempDamage = buff.SeeBuffId(buffId, tempDamage);
        }
        return tempDamage;
    }

    public void MagicHit(float magicDamage)
    {
        float tempDamage = magicDamage;
        DefenseBuffs buff = gameObject.GetComponent<DefenseBuffs>();
        //Buff effect calculate
        foreach (int buffId in DefenseBuffIds) {
            tempDamage = buff.SeeBuffId(buffId, tempDamage);
        }
        GetDamage(tempDamage);
    }

    public void Respawn() 
    {
        //Death UI
        //Time wait
        hp = originHp;
        //location move
        player.TeleportTo(GameObject.Find("TestRespawnPoint").transform.position, player.GetRotation());
    }

    public float GetHp()
    {
        return hp;
    }

    public void SetPlayer(int playerId)
    {
        this.playerId = playerId;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(GetPlayer));
    }

    public void GetPlayer()
    {
        this.player = VRCPlayerApi.GetPlayerById(playerId);
    }
}
