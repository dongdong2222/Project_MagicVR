using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using static VRC.SDKBase.Networking;
using static VRC.SDKBase.VRCPlayerApi;
using VRC.Udon;
using UdonSharp;


//Utilities.isValid 쓸것
public class PlayerStat : UdonSharpBehaviour
{
    /*
        Variables for Player
    */
    public VRCPlayerApi playerId;
    [SerializeField] private float hp;
    private Text hpText;
    private float originHp = 100;
    [SerializeField] private float mp;
    [SerializeField] private int myTeam;
    
    private int[] OffenseBuffIds = {};
    private int[] DefenseBuffIds = {};

    // Start is called before the first frame update
    void Start()
    {  
        SetOwner(LocalPlayer, gameObject);
        hp = originHp;
        hpText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = hp.ToString();
    }

    private void GetDamage(float damage) 
    {
        if(hp > damage) {
            hp -= damage;
        }
        else {
            Respawn();
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
    }
}
