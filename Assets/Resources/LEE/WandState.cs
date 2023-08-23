
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WandState : UdonSharpBehaviour
{
    private float triggerTimeThreshold = 0.3f;
    private float triggerInputDownThreshold = 0.1f;
    private float triggerInputUpThreshold = 0.095f;
    private bool isTriggerDown = false;
    private float prevTriggerUpTime;

    void Start()
    {
        
    }

    void Update()
    {
        switch (IsDoubleTriggerUp("left"))
        {
            case 0:
                return;

            case 1:
                return;

            case 2:
                return;
                

            default:
                return;
        }
    }

    private float GetTriggerInput(string hand)
    {
        float triggerInput = 0;
        if (hand == "left") triggerInput = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
        if (hand == "right") triggerInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
        return triggerInput;
    }

    private bool IsSingleTriggerUp(string hand)
    {
        //返り値用変数
        bool isSingleTriggerUp = false;

        //トリガーが離されたら
        if (isTriggerDown)
        {
            if (GetTriggerInput(hand) < triggerInputUpThreshold)
            {
                isSingleTriggerUp = true;
                isTriggerDown = false;
            }
        }
        //Trigger押し込みを検知
        else if (GetTriggerInput(hand) > triggerInputDownThreshold)
        {
            isTriggerDown = true;
        }

        return isSingleTriggerUp;
    }

    private int IsDoubleTriggerUp(string hand)
    {
        //返り値用変数
        int isDoubleTriggerUp = 0;

        if (IsSingleTriggerUp(hand))
        {
            isDoubleTriggerUp = 1;
            //前回のTrigger Up検知から一定時間内にTriggerUpしていれば検出判定
            if (Time.time < prevTriggerUpTime + triggerTimeThreshold) {
                isDoubleTriggerUp = 2;
            }
            prevTriggerUpTime = Time.time;
        }

        return isDoubleTriggerUp;
    }
}
