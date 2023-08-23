
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class ArmUi : UdonSharpBehaviour
{
    public GameObject savedMagicPanel;
    public int maxMagicNumber;
    public GameObject[] imagePrefabs;
    int magicCount = 0;
    Transform savedPanelTf;
    RectTransform savedPanelRtf;
    GameObject[] magicArray;

    // Start is called before the first frame update
    void Start()
    {
        magicArray = new GameObject[maxMagicNumber];
        savedPanelTf = savedMagicPanel.GetComponent<Transform>();
        savedPanelRtf = savedMagicPanel.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         Debug.Log("WInd");
    //         AddMagic("Wind");
    //     }

    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         Debug.Log("Water");
    //         AddMagic("Water");
    //     }

    //     if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         Debug.Log("Lighting");
    //         AddMagic("Lighting");
    //     }

    //     if (Input.GetKeyDown(KeyCode.Alpha4))
    //     {
    //         Debug.Log("Fire");
    //         AddMagic("Fire");
    //     }

    //     if (Input.GetKeyDown(KeyCode.Alpha5))
    //     {
    //         UseMagic();
    //     }
    // }

    void AddMagic(string magic)
    {
        if (magicCount >= maxMagicNumber)
        {
            return;
        }

        GameObject newMagic;

        switch (magic)
        {
            case "Wind":
                newMagic = Instantiate(imagePrefabs[0], savedPanelTf);
                magicArray[magicCount] = newMagic;
                break;

            case "Water":
                newMagic = Instantiate(imagePrefabs[1], savedPanelTf);
                magicArray[magicCount] = newMagic;
                break;

            case "Lighting":
                newMagic = Instantiate(imagePrefabs[2], savedPanelTf);
                magicArray[magicCount] = newMagic;
                break;

            case "Fire":
                newMagic = Instantiate(imagePrefabs[3], savedPanelTf);
                magicArray[magicCount] = newMagic;
                break;

            default:
                Debug.Log("올바르지 않는 입력입니다.");
                break;
        }

        magicCount++;
        DrawSavedListPanel();
    }

    void UseMagic()
    {
        if (magicCount < 1)
        {
            return;
        }

        Destroy(magicArray[0]);

        for (int i = 0; i < magicCount - 1; i++)
        {
            magicArray[i] = magicArray[i + 1];
        }

        magicArray[magicCount - 1] = null;
        magicCount--;
        DrawSavedListPanel();
    }

    void DrawSavedListPanel()
    {
        if (magicCount > 1)
        {
            savedPanelRtf.offsetMin = new Vector2(175f - 25 * magicCount - 2.5f * (magicCount - 1), savedPanelRtf.offsetMin.y);
            savedPanelRtf.offsetMax = new Vector2(-(175f - 25 * magicCount - 2.5f * (magicCount - 1)), savedPanelRtf.offsetMax.y);
        }
        else
        {
            savedPanelRtf.offsetMin = new Vector2(150f, savedPanelRtf.offsetMin.y);
            savedPanelRtf.offsetMax = new Vector2(-150f, savedPanelRtf.offsetMax.y);
        }

        for (int i = 0; i < magicCount; i++)
        {
            Vector2 pos = new Vector2((27.5f * (magicCount - 1)) - (55 * i), 0);
            magicArray[i].GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
