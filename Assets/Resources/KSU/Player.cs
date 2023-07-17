
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class Player : UdonSharpBehaviour
{
    public VRCPlayerApi player;
    public PlayerStat stat;
    public float time = 0;
    public void Start() {
        
    }

    /*public void Update() {
        time += Time.deltaTime;
        if(time>=5.0) {
            stat = gameObject.GetComponent<PlayerStat>();
        }
    }*/
}
