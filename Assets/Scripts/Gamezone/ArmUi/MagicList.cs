using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicList : MonoBehaviour
{
    public Transform magicListPanel;
    public Image[] imagePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnImage("Wind");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnImage("Water");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnImage("Lighting");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnImage("Fire");
        }
    }

    public void OnNumberKeyPressed(int number)
    {
        switch (number)
        {
            case 1:
                SpawnImage("Wind");
                break;

            case 2:
                SpawnImage("Water");
                break;

            case 3:
                SpawnImage("Lighting");
                break;

            case 4:
                SpawnImage("Fire");
                break;

            default:
                Debug.Log("지정된 키가 아닙니다.");
                break;
        }
    }

    void SpawnImage(string magic)
    {
        Image newMagic;

        switch (magic)
        {
            case "Wind":
                newMagic = Instantiate(imagePrefabs[0], magicListPanel);
                break;

            case "Water":
                newMagic = Instantiate(imagePrefabs[1], magicListPanel);
                break;

            case "Lighting":
                newMagic = Instantiate(imagePrefabs[2], magicListPanel);
                break;

            case "Fire":
                newMagic = Instantiate(imagePrefabs[3], magicListPanel);
                break;

            default:
                Debug.Log("올바르지 않는 입력입니다.");
                break;
        }
    }
}
