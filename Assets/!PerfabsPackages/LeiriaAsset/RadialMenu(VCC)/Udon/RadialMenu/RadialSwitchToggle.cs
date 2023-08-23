
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

//ローカルのToggleのみ対応
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RadialSwitchToggle : UdonSharpBehaviour
{
    [Header("対象をトグルする(Local)")]
    [Space]
    [Header("トグルする対象を入れる(1番上に設定したObjectのactive.selfがアイコンのOn/Off表示に反映される)")]
    [SerializeField] GameObject[] TargetObjects;
    [Header("Switchの子のLabelを入れる")]
    [SerializeField] Text Label;

    [Header("色選択")]
    [Header("Enable時の色")]
    [SerializeField] Color EnableColor = new Color(50, 255, 0, 255);
    [Header("Disable時の色")]
    [SerializeField] Color DisableColor = new Color(255, 255, 255, 255);

    private Image switchImage;

    void Start()
    {
        switchImage = gameObject.GetComponent<Image>();
        switchImage.color = TargetObjects[0].activeSelf ? EnableColor : DisableColor;
        Label.color = TargetObjects[0].activeSelf ? EnableColor : DisableColor;

    }

    public void Switch()
    {
        for (int i = 0; i < TargetObjects.Length; i++)
        {
            TargetObjects[i].SetActive(!TargetObjects[i].activeSelf);
        }

        switchImage.color = TargetObjects[0].activeSelf ? EnableColor : DisableColor;
        Label.color = TargetObjects[0].activeSelf ? EnableColor : DisableColor;
    }
}
