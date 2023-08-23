using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RadialMenuMain : UdonSharpBehaviour
{
    [Header("RadialMenuのMain。主要な設定を以下フィールドで行う。")]
    [Space]

    [Header("Radial Menu Mainの子を入れる")]
    [SerializeField] GameObject RadialMenuTracking;
    [SerializeField] GameObject RadialMenuUI;
    [SerializeField] GameObject Radial;
    [SerializeField] GameObject RadialCenterCircle;
    [SerializeField] GameObject RadialCenterArrow;

    [Header("RadialSwitchesの子(計6個)を時計回りの順で入れる")]
    [SerializeField] GameObject[] RadialSwitches;

    [Header("初回起動前に表示する説明を入れる")]
    [SerializeField] GameObject DoubleTriggerDescriptionVR;
    [SerializeField] GameObject TabToOpenDescriptionDesktop;

    [Header("Desktopのキー説明を入れる")]
    [SerializeField] GameObject DesktopKeyGuide;

    [Header("カメラ位置取得用のCanvasを入れる")]
    [SerializeField] GameObject CameraTracking;

    [Header("メニューの位置・回転のオフセットを入力(VR)")]
    [SerializeField] Vector3 RadialMenuPositionOffsetVR = new Vector3(0, 0, 0.1f);
    [SerializeField] Vector3 RadialMenuRotationOffsetVR = new Vector3(0, 90f, 90f);

    [Header("メニューの位置・回転・大きさのオフセットを入力(Desktop)")]
    [SerializeField] Vector3 RadialMenuPositionOffsetDesktop = new Vector3(-0.3f, -0.03f, 0.5f);
    [SerializeField] Vector3 RadialMenuRotationOffsetDesktop = new Vector3(0, -5.0f, 0);
    [SerializeField] Vector3 RadialMenuScaleOffsetDesktop = new Vector3(1.5f, 1.5f, 0);

    [Header("メニューOn/Off時のアニメーション速度")]
    [SerializeField] float AnimationSpeed = 10f;//Open,Closeアニメーションの速度
    private float menuCloseScaleThreshold = 0.2f;//Close時、メニューのサイズが閾値以下になったらdisableする。

    [Header("RadialMenuMainが初回起動するまでの時間[s]")]
    [Header("現状、LocalTestでIsUserInVRの取得が早いと、VRかDesktopか正しく判別できないので2秒ほど設定。")]
    [SerializeField] float MenuStartDelay = 2.0f;

    //[Header("デバッグ用")]
    //[SerializeField] Text DebugText;


    [NonSerialized] public bool IsMenuEnable = false;//メニューOn・Off管理
    [NonSerialized] public bool IsVolumeSwitch;//VolumeSwitchを使っているかVolumeSwitchのUdonから受け取る

    /*****Private*****/
    private float triggerTimeThreshold = 0.3f;//double triggerを検出判定する時間
    private float triggerInputDownThreshold = 0.1f;//トリガーDownを検知する閾値
    private float triggerInputUpThreshold = 0.095f;//トリガーUpを検知する閾値

    private bool isTriggerDown = false;
    private float prevTriggerUpTime;

    private int debugSingleTrigerCont = 0;
    private int debugDoubleTrigerCont = 0;
    private float gripThreshold = 0.1f;//grip中はメニューを表示しないようにする為の閾値
    private float joystickMagnitudeThreshold = 0.1f;//joystick倒したかを判定する閾値

    private Vector3 radialMenuUIScale;//身長によってサイズを変えるための変数(仕様上、Zは0にする)
    private float playerHeightFactor = 1.2f;//アバターの身長によってメニューのサイズがどの程度変化するか

    private Vector3 radialMenuDefaultUIScale;
    private Image radialImage;//UI Image格納

    private float divideDegree;// (360/分割数)
    private float radialDegreeOffset = 300;//シェーダーの0度がずれているため、offset。
    private float radialDegreeDesktop = 0;//Desktop矢印回転

    private bool isSwitched = false;//switchが押されたか
    private bool isUserInVR;//VR?

    private int menuOpenCont = 0;//メニューがOpenされた回数

    private bool isInitMenuFinished = false;


    void Start()
    {
        RadialMenuTracking.SetActive(false);
        SendCustomEventDelayedSeconds(nameof(InitMenu), MenuStartDelay);
    }

    void Update()
    {
        if (isInitMenuFinished)
        {

            //初回起動前、左手に説明を表示する(VR)
            if (DoubleTriggerDescriptionVR.activeSelf)
            {
                var trackingDataLeftHand = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
                RadialMenuTracking.transform.position = trackingDataLeftHand.position;
                RadialMenuTracking.transform.rotation = trackingDataLeftHand.rotation;
            }

            //(2回連続トリガーが押されたら or Tabが押されたら) && VolumeSwitch処理中のOff防止。
            if ((IsDoubleTriggerUp("left") || Input.GetKeyDown(KeyCode.Tab)) && !IsVolumeSwitch)
            {

                //VRの場合、アナログスティックが倒れている・Gripされている場合はメニューを表示しない。(メニューを閉じることは可能)
                if (!isUserInVR || ((GetGripInput("left") < gripThreshold) && (GetJoystickInput("left").magnitude < joystickMagnitudeThreshold)) || IsMenuEnable)
                {
                    IsMenuEnable = !IsMenuEnable;
                    menuOpenCont++;

                    //VRならプレイヤーを固定・固定解除し、アバターの身長に合わせてメニューの大きさを設定
                    if (isUserInVR)
                    {
                        Networking.LocalPlayer.Immobilize(IsMenuEnable);
                        radialMenuUIScale = radialMenuDefaultUIScale * Mathf.Pow(GetPlayerHeight(), playerHeightFactor);
                    }
                    //Desktopの場合、Desktopの大きさのオフセットを適用
                    else
                    {
                        radialMenuUIScale = Vector3.Scale(radialMenuDefaultUIScale, RadialMenuScaleOffsetDesktop);
                    }
                }

            }

            //メニューがactiveなら
            if (IsMenuEnable)
            {
                //VRならメニューを左手に追従させる。Desktopならカメラに追従させる。
                if (isUserInVR)
                {
                    var trackingDataLeftHand = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
                    RadialMenuTracking.transform.position = trackingDataLeftHand.position;
                    RadialMenuTracking.transform.rotation = trackingDataLeftHand.rotation;
                }
                else
                {
                    RadialMenuTracking.transform.position = CameraTracking.transform.position;
                    RadialMenuTracking.transform.rotation = CameraTracking.transform.rotation;
                }

                //オープン時のアニメーション
                OpenAnimation(RadialMenuUI);

                //Volume Switch操作中は、矢印を消す
                RadialCenterArrow.SetActive(!IsVolumeSwitch);

                //VolumeSwitch操作中、メニューを薄くする
                radialImage.material.SetFloat("_Globalopacity", IsVolumeSwitch ? 0.2f : 1.0f);

                //(スティックが一定以上倒されたら || Desktopなら) && VolumeSwitch処理中のOff防止 && Volume Switchから戻ってトリガーが上がっているか?
                if ((GetJoystickInput("left").magnitude > joystickMagnitudeThreshold || !isUserInVR) && !IsVolumeSwitch)
                {
                    //スティックの角度の取得・6方位丸め (90°,270°の時の丸め誤差対策で0.5を足す)
                    float stickDegree = isUserInVR ? Mathf.Min(GetJoystickDegree("left") + 0.5f, 360.0f) : GetDesktopRotationDegree();

                    //マテリアルに角度入力(シェーダーとの角度ずれ考慮)・塗りつぶし
                    radialImage.material.SetFloat("_Rotation", Mathf.Round((radialDegreeOffset - stickDegree) / divideDegree) * divideDegree);
                    radialImage.material.SetFloat("_Fillpercentage", divideDegree / 360.0f);

                    //中央の丸に角度入力・矢印active
                    RadialCenterCircle.transform.localRotation = Quaternion.Euler(stickDegree * new Vector3(0, 0, -1));
                    RadialCenterArrow.SetActive(true);

                    //トリガーが戻らない限り、switchの処理に入らせない(VR)
                    isSwitched &= GetTriggerInput("left") > triggerInputUpThreshold;

                    //(トリガーが閾値を超えた瞬間 || マウスをクリック(Down)した瞬間)、スティックの角度に応じて各switchにイベントを投げる
                    if (((GetTriggerInput("left") > triggerInputDownThreshold) && !isSwitched) || Input.GetMouseButtonDown(0))
                    {
                        isSwitched = true;
                        int itemNumber = (int)Mathf.Round(stickDegree / divideDegree) % RadialSwitches.Length;
                        ((UdonBehaviour)RadialSwitches[itemNumber].GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Switch");
                    }
                }
                else
                {
                    //マテリアルの塗りつぶしを解除・矢印disable
                    radialImage.material.SetFloat("_Fillpercentage", 0);
                    RadialCenterArrow.SetActive(false);
                }
            }

            //メニューがOffの場合、プレイヤーの固定解除・Closeアニメーション
            else if (!IsMenuEnable)
            {
                Networking.LocalPlayer.Immobilize(IsMenuEnable);

                //初回起動後の場合、closeアニメーション・説明off
                if(menuOpenCont > 0)
                {
                    CloseAnimation(RadialMenuUI);
                    DoubleTriggerDescriptionVR.SetActive(false);
                    TabToOpenDescriptionDesktop.SetActive(false);
                }
                //初回起動前はUIの表示を消しておく。
                else
                {
                    RadialMenuUI.SetActive(false);
                }

            }
        }

        //デバッグ用
        //DebugRadial(DebugText);
    }

    //ジョイスティックの入力をVector2で取得
    private Vector2 GetJoystickInput(string hand)
    {
        Vector2 stick = new Vector2(0, 0);
        if (hand == "left")
        {
            stick.x = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal");
            stick.y = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical");
        }
        else if (hand == "right")
        {
            stick.x = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal");
            stick.y = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical");
        }
        return stick;
    }

    //ジョイスティックの倒れた角度を取得(0～360)
    private float GetJoystickDegree(string hand)
    {
        Vector2 axis = GetJoystickInput(hand);
        float degree = Mathf.Rad2Deg * Mathf.Atan2(axis.x, axis.y);
        return degree < 0 ? degree + 360 : degree;
    }

    //デスクトップの場合、Q・Eキーで角度を加算・減算
    private float GetDesktopRotationDegree()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            radialDegreeDesktop += divideDegree;
            if (radialDegreeDesktop > 360) radialDegreeDesktop -= 360;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            radialDegreeDesktop -= divideDegree;
            if (radialDegreeDesktop < 0) radialDegreeDesktop += 360;
        }
        return radialDegreeDesktop;
    }

    //トリガーの入力を取得
    private float GetTriggerInput(string hand)
    {
        float triggerInput = 0;
        if (hand == "left") triggerInput = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
        if (hand == "right") triggerInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
        return triggerInput;
    }

    //TriggerUp1回の判定
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
                debugSingleTrigerCont++;
            }
        }
        //Trigger押し込みを検知
        else if (GetTriggerInput(hand) > triggerInputDownThreshold)
        {
            isTriggerDown = true;
        }

        return isSingleTriggerUp;
    }

    //TriggerUp2回連続の判定
    private bool IsDoubleTriggerUp(string hand)
    {
        //返り値用変数
        bool isDoubleTriggerUp = false;

        if (IsSingleTriggerUp(hand))
        {
            //前回のTrigger Up検知から一定時間内にTriggerUpしていれば検出判定
            if (Time.time < prevTriggerUpTime + triggerTimeThreshold) {
                isDoubleTriggerUp = true;
                debugDoubleTrigerCont++;
            }
            prevTriggerUpTime = Time.time;
        }

        return isDoubleTriggerUp;
    }

    //Gripの入力を取得
    private float GetGripInput(string hand)
    {
        float gripInput = 0;
        if (hand == "left") gripInput = Input.GetAxis("Oculus_CrossPlatform_PrimaryHandTrigger");
        if (hand == "right") gripInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryHandTrigger");
        return gripInput;
    }

    //メニューを開いたときに拡大するアニメーション
    private void OpenAnimation(GameObject radialMenuUi)
    {
        radialMenuUi.SetActive(true);
        if (radialMenuUi.transform.localScale.x < radialMenuUIScale.x)
        {
            radialMenuUi.transform.localScale = Vector3.Lerp(radialMenuUi.transform.localScale, radialMenuUIScale, Time.deltaTime * AnimationSpeed);
        }
    }

    //メニューを閉じたときに縮小するアニメーション。一定より小さくなったらdisable。
    private void CloseAnimation(GameObject radialMenuUi)
    {
        if (radialMenuUi.transform.localScale.x > menuCloseScaleThreshold)
        {
            radialMenuUi.transform.localScale = Vector3.Lerp(radialMenuUi.transform.localScale, Vector3.zero, Time.deltaTime * AnimationSpeed);
        }
        else
        {
            radialMenuUi.SetActive(false);
        }
    }

    //アバターの身長を取得
    private float GetPlayerHeight()
    {
        //カメラ(ViewPoint)とアバターのFootBoneの距離を求める。
        //高身長アバター対応と割り切り、１～2の間でクランプ。
        var player = Networking.LocalPlayer;
        return Mathf.Clamp(Vector3.Distance(CameraTracking.transform.position, player.GetBonePosition(HumanBodyBones.LeftFoot)), 1.0f, 2.0f);
    }

    public void InitMenu()
    {
        //マテリアルへの変数代入の準備・UIのスケールを0に(Openアニメーション用初期化)・矢印disable
        radialImage = Radial.GetComponent<Image>();
        radialImage.material.EnableKeyword("_Rotation");
        radialImage.material.EnableKeyword("_Fillpercentage");
        radialImage.material.EnableKeyword("_Globalopacity");
        radialMenuDefaultUIScale = RadialMenuUI.transform.localScale;
        RadialMenuUI.transform.localScale = Vector3.zero;
        RadialCenterArrow.SetActive(false);

        isUserInVR = Networking.LocalPlayer.IsUserInVR();

        //VRの場合
        if (isUserInVR)
        {
            //位置のOffsetを入力
            RadialMenuUI.transform.localPosition = RadialMenuPositionOffsetVR;
            RadialMenuUI.transform.localRotation = Quaternion.Euler(RadialMenuRotationOffsetVR);
            DoubleTriggerDescriptionVR.transform.localPosition = RadialMenuPositionOffsetVR;
            DoubleTriggerDescriptionVR.transform.localRotation = Quaternion.Euler(RadialMenuRotationOffsetVR);
            //説明表示
            DoubleTriggerDescriptionVR.SetActive(true);
            TabToOpenDescriptionDesktop.SetActive(false);
            DesktopKeyGuide.SetActive(false);
        }
        //デスクトップの場合
        else
        {
            //位置のOffsetを入力
            RadialMenuUI.transform.localPosition = RadialMenuPositionOffsetDesktop;
            RadialMenuUI.transform.localRotation = Quaternion.Euler(RadialMenuRotationOffsetDesktop);
            RadialMenuUI.transform.localScale = RadialMenuScaleOffsetDesktop;
            //説明表示
            DoubleTriggerDescriptionVR.SetActive(false);
            TabToOpenDescriptionDesktop.SetActive(true);
            DesktopKeyGuide.SetActive(true);
        }

        IsVolumeSwitch = false;
        divideDegree = 360 / Mathf.Max(RadialSwitches.Length, 1);

        RadialMenuTracking.SetActive(true);
        isInitMenuFinished = true;
    }

    //Debug用
    private void DebugRadial(Text text)
    {
        float stickDegree = GetJoystickDegree("left");
        float stickDegreeRound = Mathf.Round(stickDegree / divideDegree) * divideDegree;

        text.text = "---RadialMenuTransform---\r\n";
        text.text += string.Format("RadialMenuTracking.position: {0}\r\n", RadialMenuTracking.transform.localPosition.ToString());
        text.text += string.Format("RadialMenuTracking.rotation: {0}\r\n", RadialMenuTracking.transform.localRotation.eulerAngles.ToString());
        text.text += "---RadialMenuUiTransform---\r\n";
        text.text += string.Format("RadialMenuUI.position: {0}\r\n", RadialMenuUI.transform.localPosition.ToString());
        text.text += string.Format("RadialMenuUI.rotation: {0}\r\n", RadialMenuUI.transform.localRotation.eulerAngles.ToString());
        text.text += "---JoyStick---\r\n";
        text.text += string.Format("Left: {0}\r\n", GetJoystickInput("left").ToString());
        text.text += string.Format("Left Degree: {0}\r\n", GetJoystickDegree("left").ToString());
        text.text += string.Format("Right: {0}\r\n", GetJoystickInput("right").ToString());
        text.text += string.Format("Right Degree: {0}\r\n", GetJoystickDegree("right").ToString());
        text.text += string.Format("Left Stick Degree Round: {0}\r\n", stickDegreeRound.ToString());
        text.text += string.Format("stickDegreeRound / divideDegree: {0}\r\n", (stickDegreeRound / divideDegree).ToString());
        text.text += "---Trigger---\r\n";
        text.text += string.Format("Trigger Left: {0}\r\n", GetTriggerInput("left").ToString());
        text.text += string.Format("Trigger Right: {0}\r\n", GetTriggerInput("right").ToString());
        text.text += string.Format("Single Trigger Cont: {0}\r\n", debugSingleTrigerCont.ToString());
        text.text += string.Format("Double Trigger Cont: {0}\r\n", debugDoubleTrigerCont.ToString());
        text.text += "---Camera---\r\n";
        text.text += string.Format("Camera Tracking: {0:0.00}\r\n", CameraTracking.transform.position.ToString());
        text.text += "---Avater Height---\r\n";
        text.text += string.Format("Avater Height(distance(Cam,LeftFoot): {0:0.00}\r\n", (Vector3.Distance(CameraTracking.transform.position, Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftFoot)).ToString()));
        text.text += string.Format("IsUserInVR: {0}\r\n", Networking.LocalPlayer.IsUserInVR());
    }
}
