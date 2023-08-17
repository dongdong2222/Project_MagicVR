
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class PenTouch : UdonSharpBehaviour
{
    public Transform redslider;
    public Transform greenslider;
    public Transform blueslider;
    public Transform sizeslider;

    private float _time = 0;                                        //use for clear screen delay
    private Transform redmax, redmin, greenmax, greenmin, bluemax, bluemin, sizemax, sizemin;

    public PenSystem _pen;

    private void Start()
    {
        redmax = redslider.Find("redmax");                          //Initialize sliders' min&max vaule point
        redmin = redslider.Find("redmin");
        greenmax = greenslider.Find("greenmax");
        greenmin = greenslider.Find("greenmin");
        bluemax = blueslider.Find("bluemax");
        bluemin = blueslider.Find("bluemin");
        sizemax = sizeslider.Find("sizemax");
        sizemin = sizeslider.Find("sizemin");
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "clear")
        {                                                           //stay in clear trigger for 3 seconds to clear
            _time += Time.deltaTime;
            if (_time > 3)
            {
                _pen.ClearBoard();
                _time = 0;
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        _time = 0;
    }



    private void OnTriggerEnter(Collider triggerObj)
    {
        switch (triggerObj.name)                                                    //get which trigger entered
        {
            case "r":
                SetSliderValue(1);                                                  //calculate red slider value
                Debug.Log(triggerObj.name);
                break;
            case "g":
                SetSliderValue(2);                                                  //calculate green slider value
                Debug.Log(triggerObj.name);
                break;
            case "b":                                                               //calculate blue slider value
                SetSliderValue(3);
                Debug.Log(triggerObj.name);
                break;
            case "size":                                                            //calculate size slider value
                SetSliderValue(4);
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor1":                                                     //select saved color button to be active
                _pen.SelectSavedColor1();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor2":
                _pen.SelectSavedColor2();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor3":
                _pen.SelectSavedColor3();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor4":
                _pen.SelectSavedColor4();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor5":
                _pen.SelectSavedColor5();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor6":
                _pen.SelectSavedColor6();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor7":
                _pen.SelectSavedColor7();
                Debug.Log(triggerObj.name);
                break;
            case "savedcolor8":
                _pen.SelectSavedColor8();
                Debug.Log(triggerObj.name);
                break;
            case "savecolor":                                                       //save current color to active color button
                _pen.SaveCurrentColor();
                Debug.Log(triggerObj.name);
                break;
            case "loadcolor":                                                       //load active color button color to pen
                _pen.LoadSavedColor();
                Debug.Log(triggerObj.name);
                break;
            case "lock":                                                            //toggle board can be pickup
                _pen.ChangeLockState();
                Debug.Log(triggerObj.name);
                break;
            default:
                Debug.Log("default");
                break;
        }
    }


    void SetSliderValue(int index)                                      //if trigger is a slider, calculate value, send to slider
    {
        Vector3 min = Vector3.zero, max = Vector3.zero;
        switch (index)
        {
            case 1:
                min = redmin.position;
                max = redmax.position;
                break;
            case 2:
                min = greenmin.position;
                max = greenmax.position;
                break;
            case 3:
                min = bluemin.position;
                max = bluemax.position;
                break;
            case 4:
                min = sizemin.position;
                max = sizemax.position;
                break;
            default: break;
        }
        Debug.Log("min:" + min + "max:" + max);
        Vector3 mintomax = max - min;
        Vector3 mintopen = this.transform.position - min;
        Vector3 targetpoint = Vector3.Project(mintopen, mintomax);
        float _value = Vector3.Magnitude(targetpoint) / Vector3.Magnitude(mintomax);                //use Vector3 to calculate value
        Debug.Log("Value=" + _value);
        if (index == 4) _pen.SizeSliderChange(_value);
        else _pen.ColorSliderChange(index, _value);
    }
}
