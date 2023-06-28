using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;
using static VRC.SDKBase.Networking;
using static VRC.SDKBase.VRCPlayerApi;
using VRC.Udon;
using UdonSharp;

public class PlayerSetting : UdonSharpBehaviour
{
    public GameObject prefab;
    void Start()
    {
        prefab = Resources.Load<GameObject>("Prefabs/Player/PlayerStat");
        Debug.Log(0);
        //Debug.Log(1);
        //playerStat.SetActive(true);
        //SetOwner(LocalPlayer, playerStat);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
