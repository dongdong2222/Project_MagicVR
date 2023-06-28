using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;
using static VRC.SDKBase.Networking;
using static VRC.SDKBase.VRCPlayerApi;
using VRC.Udon;

//Utilities.isValid 쓸것

//This Enum will be in GameManager
public enum Team {
    ATeam, BTeam
};

public class Player : MonoBehaviour
{
    /*
        Variables for Player
    */
    [SerializeField] private float Hp;
    [SerializeField] private float Mp;
    [SerializeField] private Team MyTeam;

    // Start is called before the first frame update
    void Start()
    {  
        SetOwner(LocalPlayer, gameObject);
        Hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
