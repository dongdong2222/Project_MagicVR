
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WandSystem : UdonSharpBehaviour
{
    private bool _isHeld = false;
    public TrailRenderer trailRenderer;
    private int posTime = 0;
    private Vector3[] pos = new Vector3[4];
    private Vector3[] line = new Vector3[3];
    private float[] angle = new float[2];
    void Start()
    {
        
    }
    
    void OnPickup()                                                                 //set owner to localplayer when localplayer picked up
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        _isHeld = true;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TrailTrue");
    }

    void OnDrop()                                                                   //set _ispickedup to false
    {
        _isHeld = false;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TrailFalse");
    }

    public void TrailTrue()
    {
        trailRenderer.enabled = true;
    }

    public void TrailFalse()
    {
        trailRenderer.enabled = false;
    }

    public Vector3 GetPosition() {
        return gameObject.transform.position;
    }

    public Vector3 GetVector(Vector3 start, Vector3 end) {
        return new Vector3(end.x - start.x, end.y - start.y, end.z - start.z);
    }

    /*public float GetAngle(Vector3 one, Vector3 two) {
        return Mathf.Acos(
            (one.x*two.x+one.y*two.y+one.z*two.z)
            /
            (Mathf.Sqrt(Mathf.Pow(one.x,2)+Mathf.Pow(one.y,2)+Mathf.Pow(one.z,2))
            *Mathf.Sqrt(Mathf.Pow(two.x,2)+Mathf.Pow(two.y,2)+Mathf.Pow(two.z,2)))
            );
    }*/

    public override void OnPickupUseDown()
    {
        //One Click, One Position Get
        pos[posTime%4] = GetPosition();
        Debug.Log("Pos " + posTime%4 + ": " + pos[posTime%4]);
        if(posTime%4 > 0) line[posTime%4 - 1] = GetVector(pos[posTime%4 - 1], pos[posTime%4]);
        if(posTime%4 > 1) {
            angle[posTime%4 - 2] = Vector3.SignedAngle(transform.up, line[posTime%4 - 1], line[posTime%4 - 2]);
            if(angle[posTime%4 - 2] < 0) angle[posTime%4 - 2] += 360;
            //GetAngle(line[posTime%4 - 2],line[posTime%4 - 1]);
            Debug.Log("Angle " + (posTime%4 - 2) + ": " + angle[posTime%4 - 2]);//Mathf.Rad2Deg * 
        }
        if(posTime%4 == 3)  {
            ClassifyMagic();
        }
        posTime++;
    }

    // 나중에 magic 클래스로 옮길 테스트용 함수
    public void ClassifyMagic()
    {
        if(angle[0] > 0 && angle[0] < 90) {
            if(angle[1] > 180 && angle[1] < 270) {
                Debug.Log("Magic is classified: Thunder");
            }
            else if(angle[1] > 0 && angle[1] < 90) {
                Debug.Log("Magic is classified: Triangle");
            }
        }
        else return;
    }
}
