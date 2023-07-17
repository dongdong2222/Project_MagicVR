
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DefenseBuffs : UdonSharpBehaviour
{
    public float SeeBuffId(int buffId, float damage) {
        float tempDamage = damage;
        switch(buffId) {
            //more buffs here
            default:
                return tempDamage;
        }
    }

    //private buff functions here
}
