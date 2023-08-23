using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MagicThree : UdonSharpBehaviour
{
    // 각도 판별
    public void ClassifyMagic(float[] angle)
    {
        if(angle[0] > 90 && angle[0] < 180) {
            if(angle[1] > 90 && angle[1] < 180) {
                Thunder();
            }
            else if(angle[1] > 0 && angle[1] < 90) {
                Triangle();
            }
        }
        else return;
    }

    public void Thunder() {
        //Check OffenseBuffs
        //Create Magic Object will be here
        return;
    }

    public void Triangle() {

    }
}
