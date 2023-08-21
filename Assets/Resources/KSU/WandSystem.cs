
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WandSystem : UdonSharpBehaviour
{
    private bool _isHeld = false;
    public TrailRenderer trailRenderer;
    private int posTime = 0;
    private Vector3[] pos = new Vector3[8];
    private Vector3[] line = new Vector3[7];
    private float[] angle = new float[6];
    private bool drawMode = false;

    void Start()
    {
        
    }
    
    void OnPickup()                                                                 //set owner to localplayer when localplayer picked up
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        _isHeld = true;
        drawMode = true;
        transform.rotation = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Chest);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TrailTrue");
    }

    void OnDrop()                                                                   //set _ispickedup to false
    {
        _isHeld = false;
        drawMode = false;
        posTime = 0;
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
        if(drawMode) {
            //One Click, One Position Get
            pos[posTime] = GetPosition();
            //If posTime
            if(posTime > 0) {
                line[posTime%4 - 1] = GetVector(pos[posTime - 1], pos[posTime]);
            }
            if(posTime > 1) {
                Vector3 cross = Vector3.Cross(line[posTime - 1],line[posTime - 2]);
                angle[posTime - 2] = Vector3.SignedAngle(line[posTime - 1], line[posTime - 2], cross);
                //angle[posTime%4 - 2] = Vector3.SignedAngle(transform.up, line[posTime%4 - 1], line[posTime%4 - 2]);
                //if(angle[posTime%4 - 2] < 0) angle[posTime%4 - 2] += 360;
                //GetAngle(line[posTime%4 - 2],line[posTime%4 - 1]); 
            }
            //drawMode에서 벗어날 때로 옮길 것
            if(posTime == 3)  {
                ClassifyMagic();
                posTime = 0;
            }
            posTime++;
        }
    }

    // posTime 수에 따라 Magic클래스를 호출하는 함수로 바뀔 예정
    public void ClassifyMagic()
    {
        if(angle[0] > 90 && angle[0] < 180) {
            if(angle[1] > 90 && angle[1] < 180) {
                Debug.Log("Magic is classified: Thunder");
            }
            else if(angle[1] > 0 && angle[1] < 90) {
                Debug.Log("Magic is classified: Triangle");
            }
        }
        else return;
    }

    
    
}
