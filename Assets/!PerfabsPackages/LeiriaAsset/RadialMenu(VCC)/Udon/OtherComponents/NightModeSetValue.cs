
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

//NightMode用のスライダーに合わせてマテリアルを変化させる

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class NightModeSetValue : UdonSharpBehaviour
{
    [Header("Sliderでマテリアルの値を変えられないので、専用のUdonで中継する。")]
    [Space]
    [Header("Night Modeのスライダーを入れる")]
    public Slider NightModeSlider;
    private SkinnedMeshRenderer nightModeMesh;

    void Start()
    {
        nightModeMesh = gameObject.GetComponent<SkinnedMeshRenderer>();
        nightModeMesh.material.EnableKeyword("_Alpha");
    }

    void Update()
    {
        nightModeMesh.material.SetFloat("_Alpha", NightModeSlider.value);
    }

}
