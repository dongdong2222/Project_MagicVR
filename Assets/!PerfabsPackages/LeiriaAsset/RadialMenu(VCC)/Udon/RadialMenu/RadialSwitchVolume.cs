using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

//汎用性のため、uGUIのSliderを利用。

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RadialSwitchVolume : UdonSharpBehaviour
{
    [Header("uGUIのスライダーを参照して、0～100%で値を入力する。")]
    [Space]
    [Header("Radial Menu Mainを入れる")]
    [SerializeField] UdonBehaviour RadialMenuMain;
    [Header("Radial Switch Volumeの子の対象を設定したSliderを入れる")]
    [SerializeField] Slider TargetSlider;
    [Header("Radial Switch Volumeの子のRadialVolumeを入れる")]
    [SerializeField] GameObject RadialVolume;
    [Header("Radial Switch Volumeの子のLabelを入れる")]
    [SerializeField] GameObject Label;
    [Header("Radial Volumeの子のPerccent表示用のTextを入れる")]
    [SerializeField] Text Percent;

    private Material radialVolumeImageMaterial;//UI ImageのMaterial格納
    private float triggerInputThreshold = 0.1f;//トリガー入力を検知する閾値]
    private float joystickMagnitudeThreshold = 0.1f;//joystick倒したかを判定する閾値
    private float volumeSpeedFactorDesktop = 3.0f;//デスクトップの時のボリュームの変化速度

    private float radialDegreeOffset = 270;//シェーダーの0度がずれているため、offset。
    private bool isTriggerUpOnce;//トリガーが一度戻ったか
    private bool isMouseClickUpOnce;//マウスクリックが1度戻ったか?
    private bool isJoystickReturn;//ジョイスティックが元の位置に戻ったか?
    private bool isJoystickTilt;//ジョイスティックが倒れたか?
    private float prevDegree;//前回のスティック入力値

    private bool isUserInVR = false;//VR?
    private float radialDegreeDesktop = 0;//Desktopボリューム入力

    void Start()
    {
        radialVolumeImageMaterial = RadialVolume.GetComponent<Image>().material;
        radialVolumeImageMaterial.EnableKeyword("_Rotation");
        radialVolumeImageMaterial.EnableKeyword("_Fillpercentage");
        radialVolumeImageMaterial.EnableKeyword("_Globalopacity");
        radialVolumeImageMaterial.SetFloat("_Rotation", radialDegreeOffset);
        RadialVolume.SetActive(false);

        isUserInVR = Networking.LocalPlayer.IsUserInVR();
    }

    //LocalTestでIsUserInVRの取得が早過ぎた場合の対策。
    void OnEnable()
    {
        isUserInVR = Networking.LocalPlayer.IsUserInVR();
    }

    public void Update()
    {
        //RadialVolumeがactiveなら、値の更新処理を開始
        if (RadialVolume.activeSelf)
        {
            //Switch遷移時にスティックが倒れているので、一度スティックが戻るまで値の入力処理に入らせない(VRのみ)
            if (!isJoystickReturn && isUserInVR)
            {
                isJoystickReturn = GetJoystickInput("left").magnitude < joystickMagnitudeThreshold;
                radialVolumeImageMaterial.SetFloat("_Globalopacity", isJoystickReturn ? 1.0f : 0.2f);//薄くする。
            }

            //一度でもトリガーが戻ったら
            if (!isTriggerUpOnce && isUserInVR)
            {
                isTriggerUpOnce = GetTriggerInput("left") < triggerInputThreshold;
            }
            //一度でもマウスクリックが戻ったら
            else if (!isMouseClickUpOnce && !isUserInVR)
            {
                isMouseClickUpOnce = Input.GetMouseButtonUp(0);
            }
            //(一度トリガーが戻っていて、トリガーが入力されたら) or (1度マウスクリックが戻っていて、マウスクリック(Up)されたら)、終了処理
            //Tabも追加
            else if ((GetTriggerInput("left") > triggerInputThreshold && isTriggerUpOnce && isUserInVR)
                    || (Input.GetMouseButtonUp(0) && isMouseClickUpOnce && !isUserInVR)
                    || (Input.GetKeyUp(KeyCode.Tab) && !isUserInVR))
            {
                RadialVolume.SetActive(false);
                isTriggerUpOnce = false;
                isMouseClickUpOnce = false;
                RadialMenuMain.SetProgramVariable("IsVolumeSwitch", false);
            }

            //(スティックが一定以上倒されており、一度スティックが戻っていたら) or デスクトップならボリューム入力
            else if ((GetJoystickInput("left").magnitude > joystickMagnitudeThreshold && isJoystickReturn) || !isUserInVR)
            {
                float fillPercentage;
                if (isUserInVR)
                {
                    float degree = GetJoystickDegree("left");

                    if (!isJoystickTilt)
                    {
                        isJoystickTilt = true;
                    }
                    else
                    {
                        //初回スティック入力時以外は境界を越えてスティックを回した場合、数値を0°または360°に固定
                        if ((prevDegree < 20.0f) && (degree > 180.0f))
                            degree = 0.0f;
                        else if ((prevDegree > 340.0f) && (degree < 180.0f))
                            degree = 360.0f;
                    }
                    fillPercentage = degree / 360.0f;
                    prevDegree = degree;
                }
                else
                {
                    //Desktopの場合、値を代入するだけ
                    fillPercentage = GetDesktopRotationDegree() / 360;
                }

                //マテリアルに角度入力(シェーダーとの角度ずれ考慮)・塗りつぶし
                radialVolumeImageMaterial.SetFloat("_Fillpercentage", fillPercentage);

                //スライダーとパーセントに値を入力
                TargetSlider.value = Mathf.Lerp(TargetSlider.minValue, TargetSlider.maxValue, fillPercentage);
                Percent.text = (Mathf.RoundToInt(100 * fillPercentage)).ToString();
            }
            else
            {
                isJoystickTilt = false;
            }
        }
    }

    public void Switch()
    {
        //RadialVolumeをactiveして、メニュー本体の矢印をdisable。
        RadialVolume.SetActive(true);
        RadialMenuMain.SetProgramVariable("IsVolumeSwitch", true);

        //現在のスライダーの値をマテリアル・テキストに設定
        float currentValue = (TargetSlider.value - TargetSlider.minValue) / (TargetSlider.maxValue - TargetSlider.minValue);
        radialVolumeImageMaterial.SetFloat("_Fillpercentage", currentValue);
        Percent.text = ((int)(100 * currentValue)).ToString();

        isJoystickReturn = false;//Switch遷移時はジョイスティックが倒れているはず。
        isJoystickTilt = false;//スティック入力処理の初回判定を初期化
        radialDegreeDesktop = currentValue * 360;//デスクトップの場合の初期値を設定
    }

    private float GetTriggerInput(string hand)
    {
        float triggerInput = 0;
        if (hand == "left") triggerInput = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
        if (hand == "right") triggerInput = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
        return triggerInput;
    }

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
        if (Input.GetKey(KeyCode.E))
        {
            radialDegreeDesktop += 100 * volumeSpeedFactorDesktop * Time.deltaTime;
            radialDegreeDesktop = Mathf.Min(radialDegreeDesktop, 360);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            radialDegreeDesktop -= 100 * volumeSpeedFactorDesktop * Time.deltaTime;
            radialDegreeDesktop = Mathf.Max(radialDegreeDesktop, 0);
        }
        return radialDegreeDesktop;
    }
}
