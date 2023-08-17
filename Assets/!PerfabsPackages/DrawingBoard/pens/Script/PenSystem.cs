
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class PenSystem : UdonSharpBehaviour
{
    public Animator ClearAnimator;                                                      //animator use to clear drawing zone                            
    public TrailRenderer penTrailRenderer;                                              //drawing trail, use to change color
    public Renderer mesh;                                                               //preview of the drawing trail
    private float defaultWidth;                                                         //default width of the drawing trail
    private Vector3 defaultSize;                                                        //default scale of the preview point


    [UdonSynced] private Color _drawColor;                                              //use to set and sync drawing trail color
    public Transform _penResetPoint;                                                    //transform the pen reset to
    public Slider[] _valueChange;                                                       //sliders that value need to change, 0:red; 1:green; 2:blue; 3:size
    public Image _colorPreview;
    public GameObject _board;
    public Image _lockIndi;

    [UdonSynced] float _size = 0.5f;                   //synced sliders values
    [UdonSynced] bool _isLocked = false;
    bool _isHeld = false;                                                  //is the pen been picked up
    float _time = 0;                                                                    //how long the pen been droped

    void Start()                                                                        //Initialize the default value
    {
        defaultWidth = penTrailRenderer.startWidth;
        defaultSize = mesh.transform.localScale;
        _drawColor = new Color(0, 0, 0);
        _colorPreview.color = _drawColor;
        for (int i = 0; i < 3; i++)
        {
            _valueChange[i].value = 0;
        }
        penTrailRenderer.startWidth = defaultWidth * _size;
        mesh.transform.localScale = defaultSize * _size;
        SetActiveInd();
    }


    private void Update()
    {
        mesh.material.SetColor("_Color", _drawColor);                                                   //update preview point and drawing trail color
        penTrailRenderer.material.SetColor("_Color", _drawColor);                                       
        if (!_isHeld)                                                                                   //if pen been droped for more than 5 seconds, reset pen position
        {
            _time += Time.deltaTime;
            if(_time>5)
            {
                if(Networking.LocalPlayer.IsOwner(this.gameObject)) PenReset();
                _time = 0;
            }
        }
    }

    void OnDeserialization()                                                        //when parameters been synced, update color and size
    {
        _colorPreview.color = _drawColor;
        _valueChange[3].value = _size;
        ChangeSize();
    }

    void OnPickup()                                                                 //set owner to localplayer when localplayer picked up
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        _isHeld = true;
        _time = 0;
    }

    void OnDrop()                                                                   //set _ispickedup to false
    {
        _isHeld = false;
    }

    public void ChangeLockState()
    {
        if (Networking.LocalPlayer.IsOwner(this.gameObject))
        {
            _isLocked = !_isLocked;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncLockState");
        }
    }

    public void SyncLockState()
    {
        _board.GetComponent<BoxCollider>().enabled = !_isLocked;
        if (_isLocked) _lockIndi.color = new Color(1, 0.2f, 0.2f);
        else _lockIndi.color = new Color(1, 1, 1);
    }


    public void ClearBoard()                                                        //sync action for clear drawing zone
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ClearBoardAction");
    }

    public void ClearBoardAction()
    {
        ClearAnimator.Play("clearBoardEnable");
    }


    public void ChangeColor()                                                       //set three sliders' value to color of the drawing trail
    {
        _drawColor = new Color(_valueChange[0].value, _valueChange[1].value, _valueChange[2].value);
        _drawColor.r = _valueChange[0].value;                                               //sync value
        _drawColor.g = _valueChange[1].value;
        _drawColor.b = _valueChange[2].value;
        _colorPreview.color = _drawColor;
        UpdateValue();
    }

    public void ChangeSize()                                                        //use size slider's value to set trail and preview point size
    {
        mesh.transform.localScale = defaultSize * _valueChange[3].value;
        penTrailRenderer.startWidth = defaultWidth * _valueChange[3].value;
        _size = _valueChange[3].value;                                              //sync value
    }

    public void UpdateValue()                                                       //update sliders
    {
        _valueChange[0].value = _drawColor.r;
        _valueChange[1].value = _drawColor.g;
        _valueChange[2].value = _drawColor.b;
        _valueChange[3].value = _size;
    }

    public void PenReset()                                                          //sync action for reset pen position
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SycnReset");
    }

    public void SycnReset()
    {
        this.transform.position = _penResetPoint.position;
        this.transform.rotation = _penResetPoint.rotation;
    }


    public void SizeSliderChange(float value)                                   //use for pentouch system to change the slider value
    {
        _valueChange[3].value = value;
        ChangeSize();
    }

    public void ColorSliderChange(int index,float value)                        //use for pentouch system to change the slider value
    {
        switch (index)                                                          //index=1 red; index=2 green; index=3 blue
        {
            case 1:
                _valueChange[0].value = value;
                break;
            case 2:
                _valueChange[1].value = value;
                break;
            case 3:
                _valueChange[2].value = value;
                break;
            default:
                break;
        }
        ChangeColor();                                                            //set color to slider value
    }


#region 颜色储存

    private int _savedColorButtonIndex = 0;
    public GameObject[] _savedColorButton;



    public void LoadSavedColor()
    {
        _drawColor = _savedColorButton[_savedColorButtonIndex].GetComponent<Image>().color;
        _colorPreview.color = _drawColor;
    }


    public void SaveCurrentColor()
    {
        _savedColorButton[_savedColorButtonIndex].GetComponent<Image>().color = _drawColor;
    }

    public void SetActiveInd()                                                          //change the active color button using _savedColorButtonIndex parameter
    {
        for(int i = 0; i < 8; i++)
        {
            var targetimg = _savedColorButton[i].transform.Find("Image");
            if (_savedColorButtonIndex == i) targetimg.gameObject.SetActive(true);
            else targetimg.gameObject.SetActive(false);
        }
    }

    public void SelectSavedColor1()                                                     //change the value of _savedColorButtonIndex
    {
        _savedColorButtonIndex = 0;
        SetActiveInd();
    }
    public void SelectSavedColor2()
    {
        _savedColorButtonIndex = 1;
        SetActiveInd();
    }

    public void SelectSavedColor3()
    {
        _savedColorButtonIndex = 2;
        SetActiveInd();
    }

    public void SelectSavedColor4()
    {
        _savedColorButtonIndex = 3;
        SetActiveInd();
    }

    public void SelectSavedColor5()
    {
        _savedColorButtonIndex = 4;
        SetActiveInd();
    }

    public void SelectSavedColor6()
    {
        _savedColorButtonIndex = 5;
        SetActiveInd();
    }

    public void SelectSavedColor7()
    {
        _savedColorButtonIndex = 6;
        SetActiveInd();
    }

    public void SelectSavedColor8()
    {
        _savedColorButtonIndex = 7;
        SetActiveInd();
    }


#endregion 颜色储存


#region 操作

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TrailTrue");
    }

    public override void OnPickupUseUp()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TrailFalse");
    }

    public void TrailTrue()
    {
        penTrailRenderer.enabled = true;
    }

    public void TrailFalse()
    {
        penTrailRenderer.enabled = false;
    }


    #endregion


}


